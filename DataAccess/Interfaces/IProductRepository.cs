using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<Product?> GetAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync(int? limit);
    Task<IEnumerable<Product>> GetAllByCollectionAsync(string collectionName);
}
