using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface ICategoryRepository
{
    Task<Category> CreateAsync(Category product);
    Task<Category> UpdateAsync(Category product);
    Task DeleteAsync(Category product);
    Task<Category?> GetAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync(int? limit);
}
