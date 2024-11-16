using BusinessLogic.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace BusinessLogic.Services;

public class CategoryService(ICategoryRepository repository) : ICategoryService
{
    public async Task<Category> CreateAsync(Category categoryDto)
    {
        return await repository.CreateAsync(categoryDto);
    }

    public async Task<Category> UpdateAsync(Category categoryDto)
    {
        return await repository.UpdateAsync(categoryDto);
    }

    public async Task DeleteAsync(Category categoryDto)
    {
        await repository.DeleteAsync(categoryDto);
    }

    public async Task<Category?> GetAsync(int id)
    {
        return await repository.GetAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(int? limit)
    {
        return await repository.GetAllAsync(limit);
    }
}
