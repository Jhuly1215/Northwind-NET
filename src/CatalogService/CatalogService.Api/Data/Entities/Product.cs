namespace CatalogService.Api.Data.Entities;

public class Product
{
    public int Id { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public decimal StandardCost { get; set; }
    public decimal ListPrice { get; set; }
    public int? ReorderLevel { get; set; }
    public int? TargetLevel { get; set; }
    public string? QuantityPerUnit { get; set; }
    public bool Discontinued { get; set; }
    public int? MinimumReorderQuantity { get; set; }
    public string? Category { get; set; }
}
