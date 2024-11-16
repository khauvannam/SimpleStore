using DataAccess.Models;

namespace BusinessLogic.Interfaces;

public interface ICategoryService
{
    Task<Category> CreateAsync(Category categoryDto);
    Task<Category> UpdateAsync(Category categoryDto);
    Task DeleteAsync(Category categoryDto);
    Task<Category?> GetAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync(int? limit);
}
