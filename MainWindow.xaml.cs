using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InventoryApp.ViewModels;
using InventoryApp.Models;
using System.Windows.Controls.Primitives;

namespace InventoryApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel _viewModel;
    public User? CurrentUser { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        
        _viewModel = new MainViewModel();
        DataContext = _viewModel;
        
        // Initialize async data after window loads
        Loaded += MainWindow_Loaded;
    }
    
    public MainWindow(User currentUser) : this()
    {
        CurrentUser = currentUser;
        SetUserInfo();
    }
    
    private void SetUserInfo()
    {
        if (CurrentUser != null)
        {
            UserFullNameText.Text = CurrentUser.FullName;
            UserEmailText.Text = CurrentUser.Email;
        }
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await _viewModel.InitializeAsync();
            
            // Anasayfa'yı varsayılan olarak seç
            UpdateMenuSelection("Anasayfa");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Application initialization failed: {ex.Message}", 
                          "Critical Error", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Error);
        }
    }
    
    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string menuItem)
        {
            UpdateMenuSelection(menuItem);
            NavigateToPage(menuItem);
        }
    }
    
    private void UpdateMenuSelection(string selectedMenu)
    {
        // Tüm menü butonlarını normal renge döndür
        AnasayfaButton.Background = Brushes.Transparent;
        StokButton.Background = Brushes.Transparent;
        SatislarButton.Background = Brushes.Transparent;
        IstatistiklerButton.Background = Brushes.Transparent;
        AyarlarButton.Background = Brushes.Transparent;
        
        // Seçili menüyü vurgula
        var selectedBrush = new SolidColorBrush(Color.FromRgb(52, 152, 219)); // #3498DB
        
        switch (selectedMenu)
        {
            case "Anasayfa":
                AnasayfaButton.Background = selectedBrush;
                break;
            case "Stok":
                StokButton.Background = selectedBrush;
                break;
            case "Satislar":
                SatislarButton.Background = selectedBrush;
                break;
            case "Istatistikler":
                IstatistiklerButton.Background = selectedBrush;
                break;
            case "Ayarlar":
                AyarlarButton.Background = selectedBrush;
                break;
        }
    }
    
    private void NavigateToPage(string menuItem)
    {
        // Şimdilik sadece title'ı güncelle, ileride farklı sayfalar ekleyeceğiz
        switch (menuItem)
        {
            case "Anasayfa":
                _viewModel.Title = "Envanter Yönetim Sistemi - Anasayfa";
                break;
            case "Stok":
                _viewModel.Title = "Envanter Yönetim Sistemi - Stok Yönetimi";
                break;
            case "Satislar":
                _viewModel.Title = "Envanter Yönetim Sistemi - Satışlar";
                break;
            case "Istatistikler":
                _viewModel.Title = "Envanter Yönetim Sistemi - İstatistikler";
                break;
            case "Ayarlar":
                _viewModel.Title = "Envanter Yönetim Sistemi - Ayarlar";
                break;
        }
    }
    
    private void UserProfileButton_Click(object sender, RoutedEventArgs e)
    {
        // User menu popup'ını aç/kapat
        UserMenuPopup.IsOpen = !UserMenuPopup.IsOpen;
    }
    
    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        // Popup'ı kapat
        UserMenuPopup.IsOpen = false;
        
        // Çıkış onayı iste
        var result = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", 
                                   "Çıkış Onayı", 
                                   MessageBoxButton.YesNo, 
                                   MessageBoxImage.Question);
                                   
        if (result == MessageBoxResult.Yes)
        {
            // Login window'u aç
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            
            // Bu window'u kapat
            this.Close();
        }
    }
}