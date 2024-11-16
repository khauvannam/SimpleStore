using DataAccess.DataContext;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class CategoryRepository(StoreDbContext context) : ICategoryRepository
{
    public async Task<Category> CreateAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        var existingCategory = await context.Categories.FirstOrDefaultAsync(c =>
            c.Id == category.Id
        );

        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {category.Id} not found.");
        }

        existingCategory.Name = category.Name;
        await context.SaveChangesAsync();

        return existingCategory;
    }

    public async Task DeleteAsync(Category category)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
    }

    public async Task<Category?> GetAsync(int id)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(int? limit)
    {
        if (limit.HasValue)
        {
            return await context.Categories.Take(limit.Value).ToListAsync();
        }

        return await context.Categories.ToListAsync();
    }
}
