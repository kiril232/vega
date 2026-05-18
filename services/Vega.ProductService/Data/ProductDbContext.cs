using Microsoft.EntityFrameworkCore;
using Vega.ProductService.Domain;

namespace Vega.ProductService.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        var p = mb.Entity<Product>();
        p.ToTable("products");
        p.HasKey(x => x.Id);
        p.Property(x => x.Name).IsRequired().HasMaxLength(256);
        p.Property(x => x.Description).HasMaxLength(4000);
        p.Property(x => x.Category).IsRequired().HasMaxLength(128);
        p.HasIndex(x => x.Category);
        p.Property(x => x.Price).HasPrecision(10, 2);
        p.Property(x => x.ImageUrl).HasMaxLength(1024);
    }
}
