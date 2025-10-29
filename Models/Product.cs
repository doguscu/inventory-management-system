using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models;

public class Product
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Barcode { get; set; } = string.Empty;
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public int StockQuantity { get; set; }
    
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    
    // Navigation property
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}