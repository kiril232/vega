using Microsoft.EntityFrameworkCore;
using Vega.OrderService.Domain;

namespace Vega.OrderService.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        var o = mb.Entity<Order>();
        o.ToTable("orders");
        o.HasKey(x => x.Id);
        o.Property(x => x.Status).HasConversion<int>();
        o.Property(x => x.Total).HasPrecision(10, 2);
        o.Property(x => x.Currency).HasMaxLength(8);
        o.HasIndex(x => x.UserId);
        o.HasMany(x => x.Items).WithOne().HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);

        var i = mb.Entity<OrderItem>();
        i.ToTable("order_items");
        i.HasKey(x => x.Id);
        i.Property(x => x.ProductName).IsRequired().HasMaxLength(256);
        i.Property(x => x.UnitPrice).HasPrecision(10, 2);
        i.Ignore(x => x.LineTotal);
    }
}
