using DataAccess.DataContext;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ProductRepository(StoreDbContext context) : IProductRepository
{
    public async Task<Product> CreateAsync(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(Product product)
    {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }

    public async Task<Product?> GetAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(int? limit)
    {
        if (limit.HasValue)
        {
            return await context.Products.Take(limit.Value).ToListAsync();
        }

        return await context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllByCollectionAsync(string collectionName)
    {
        var normalizedCollectionName = collectionName.ToLower().Replace("-", " ");

        var query = context.Products.AsQueryable();

        switch (normalizedCollectionName)
        {
            case "all":
                return await query.ToListAsync();

            case "best sellers":
                var bestSellers = await query
                    .Where(p => p.Stock > 100)
                    .OrderByDescending(p => p.Price)
                    .ToListAsync();
                return bestSellers;

            default:
                if (!string.IsNullOrEmpty(collectionName))
                {
                    query = query.Where(p =>
                        EF.Functions.Like(p.Category.Name.ToLower(), $"%{collectionName}%")
                    );
                }

                break;
        }

        return await query.ToListAsync();
    }
}
