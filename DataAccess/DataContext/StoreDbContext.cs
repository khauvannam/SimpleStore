using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataContext;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configuration.ProductConfiguration());

        modelBuilder.ApplyConfiguration(new Configuration.CategoryConfiguration());
    }
}
