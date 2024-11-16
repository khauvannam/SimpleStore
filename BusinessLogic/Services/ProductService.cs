using BusinessLogic.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace BusinessLogic.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public async Task<Product> CreateAsync(Product product)
    {
        return await repository.CreateAsync(product);
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        return await repository.UpdateAsync(product);
    }

    public async Task DeleteAsync(Product product)
    {
        await repository.DeleteAsync(product);
    }

    public async Task<Product?> GetAsync(int id)
    {
        return await repository.GetAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(int? limit)
    {
        return await repository.GetAllAsync(limit);
    }

    public async Task<IEnumerable<Product>> GetAllByCollectionAsync(string collectionName)
    {
        return await repository.GetAllByCollectionAsync(collectionName);
    }
}
