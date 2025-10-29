using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models;

public class SaleItem
{
    public int Id { get; set; }
    
    [Required]
    public int SaleId { get; set; }
    public Sale Sale { get; set; } = null!;
    
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public decimal UnitPrice { get; set; }
    
    [Required]
    public decimal TotalPrice { get; set; }
}