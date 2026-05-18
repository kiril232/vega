using Microsoft.EntityFrameworkCore;
using Vega.CartService.Domain;

namespace Vega.CartService.Data;

public class CartDbContext : DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }

    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        var c = mb.Entity<Cart>();
        c.ToTable("carts");
        c.HasKey(x => x.Id);
        c.Property(x => x.UserId).IsRequired();
        c.HasIndex(x => x.UserId).IsUnique();
        c.HasMany(x => x.Items).WithOne().HasForeignKey(i => i.CartId).OnDelete(DeleteBehavior.Cascade);
        c.Ignore(x => x.Total);

        var i = mb.Entity<CartItem>();
        i.ToTable("cart_items");
        i.HasKey(x => x.Id);
        i.Property(x => x.ProductName).IsRequired().HasMaxLength(256);
        i.Property(x => x.ImageUrl).HasMaxLength(1024);
        i.Property(x => x.PriceSnapshot).HasPrecision(10, 2);
        i.HasIndex(x => new { x.CartId, x.ProductId }).IsUnique();
    }
}
