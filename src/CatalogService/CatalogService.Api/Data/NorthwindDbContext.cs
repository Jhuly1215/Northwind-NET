using Microsoft.EntityFrameworkCore;
using CatalogService.Api.Data.Entities;

namespace CatalogService.Api.Data;

public class NorthwindDbContext : DbContext
{
    public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("products"); // ojo: tu tabla se llama `products`
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.ProductName).HasColumnName("product_name").HasMaxLength(50);
            e.Property(x => x.ProductCode).HasColumnName("product_code").HasMaxLength(25);
            e.Property(x => x.ListPrice).HasColumnName("list_price").HasPrecision(19, 4);
            e.Property(x => x.StandardCost).HasColumnName("standard_cost").HasPrecision(19, 4);
            e.Property(x => x.Category).HasColumnName("category").HasMaxLength(50);
            e.Property(x => x.Discontinued).HasColumnName("discontinued");
            e.Property(x => x.QuantityPerUnit).HasColumnName("quantity_per_unit").HasMaxLength(50);
            e.Property(x => x.ReorderLevel).HasColumnName("reorder_level");
        });
    }
}
