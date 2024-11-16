using DataAccess.Models;

namespace BusinessLogic.Interfaces;

public interface IProductService
{
    Task<Product> CreateAsync(Product productDto);
    Task<Product> UpdateAsync(Product productDto);
    Task DeleteAsync(Product productDto);
    Task<Product?> GetAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync(int? limit);
    Task<IEnumerable<Product>> GetAllByCollectionAsync(string collectionName);
}
