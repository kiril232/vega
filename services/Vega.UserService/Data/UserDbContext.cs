using Microsoft.EntityFrameworkCore;
using Vega.UserService.Domain;

namespace Vega.UserService.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        var u = mb.Entity<User>();
        u.ToTable("users");
        u.HasKey(x => x.Id);
        u.Property(x => x.Email).IsRequired().HasMaxLength(256);
        u.HasIndex(x => x.Email).IsUnique();
        u.Property(x => x.PasswordHash).IsRequired();
        u.Property(x => x.FullName).HasMaxLength(256);
        u.Property(x => x.CreatedAt).IsRequired();
    }
}
