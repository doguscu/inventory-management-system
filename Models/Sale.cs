using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models;

public class Sale
{
    public int Id { get; set; }
    
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public decimal TotalAmount { get; set; }
    
    [StringLength(100)]
    public string? CustomerName { get; set; }
    
    [StringLength(200)]
    public string? Notes { get; set; }
    
    // Navigation property
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}