using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventoryApp.Models;
using InventoryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.ViewModels;

public partial class MainViewModel : BaseViewModel, IDisposable
{
    private readonly InventoryDbContext _context;

    [ObservableProperty]
    private ObservableCollection<Product> _products = new();

    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();

    [ObservableProperty]
    private Product? _selectedProduct;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private decimal _totalInventoryValue;

    [ObservableProperty]
    private int _lowStockCount;

    public MainViewModel()
    {
        _context = new InventoryDbContext();
        Title = "Envanter Yönetim Sistemi";
        
        // Initialize collections
        Products = new ObservableCollection<Product>();
        Categories = new ObservableCollection<Category>();
    }

    public async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
            
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();
            
            // Load initial data
            await LoadCategoriesAsync();
            await LoadProductsAsync();
            await CalculateStatisticsAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing application: {ex.Message}");
            // Don't crash the app, just log the error
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        try
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();
            
            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading categories: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SearchProductsAsync()
    {
        try
        {
            IsBusy = true;
            
            var query = _context.Products.Include(p => p.Category).AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p => p.Name.Contains(SearchText) || 
                                       p.Barcode.Contains(SearchText) ||
                                       (p.Category != null && p.Category.Name.Contains(SearchText)));
            }
            
            var products = await query.ToListAsync();
            
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error searching products: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void AddProduct()
    {
        // For now, just show a message - will implement product dialog later
        System.Windows.MessageBox.Show("Yeni ürün ekleme özelliği yakında eklenecek!", 
                                      "Bilgi", 
                                      System.Windows.MessageBoxButton.OK, 
                                      System.Windows.MessageBoxImage.Information);
    }

    [RelayCommand]
    private void EditProduct()
    {
        if (SelectedProduct == null)
        {
            System.Windows.MessageBox.Show("Lütfen düzenlemek için bir ürün seçin!", 
                                          "Uyarı", 
                                          System.Windows.MessageBoxButton.OK, 
                                          System.Windows.MessageBoxImage.Warning);
            return;
        }

        // For now, just show a message - will implement product dialog later
        System.Windows.MessageBox.Show($"'{SelectedProduct.Name}' ürünü düzenleme özelliği yakında eklenecek!", 
                                      "Bilgi", 
                                      System.Windows.MessageBoxButton.OK, 
                                      System.Windows.MessageBoxImage.Information);
    }

    [RelayCommand]
    private async Task DeleteProductAsync()
    {
        if (SelectedProduct == null)
        {
            System.Windows.MessageBox.Show("Lütfen silmek için bir ürün seçin!", 
                                          "Uyarı", 
                                          System.Windows.MessageBoxButton.OK, 
                                          System.Windows.MessageBoxImage.Warning);
            return;
        }

        var result = System.Windows.MessageBox.Show($"'{SelectedProduct.Name}' ürünü silinsin mi?", 
                                                   "Silme Onayı", 
                                                   System.Windows.MessageBoxButton.YesNo, 
                                                   System.Windows.MessageBoxImage.Question);

        if (result == System.Windows.MessageBoxResult.Yes)
        {
            try
            {
                _context.Products.Remove(SelectedProduct);
                await _context.SaveChangesAsync();
                
                Products.Remove(SelectedProduct);
                SelectedProduct = null;
                
                await CalculateStatisticsAsync();
                
                System.Windows.MessageBox.Show("Ürün başarıyla silindi!", 
                                              "Başarılı", 
                                              System.Windows.MessageBoxButton.OK, 
                                              System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ürün silinirken hata oluştu: {ex.Message}", 
                                              "Hata", 
                                              System.Windows.MessageBoxButton.OK, 
                                              System.Windows.MessageBoxImage.Error);
            }
        }
    }

    private async Task CalculateStatisticsAsync()
    {
        try
        {
            TotalInventoryValue = await _context.Products
                .SumAsync(p => p.Price * p.StockQuantity);

            LowStockCount = await _context.Products
                .CountAsync(p => p.StockQuantity < 10); // Assuming low stock is < 10
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error calculating statistics: {ex.Message}");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}