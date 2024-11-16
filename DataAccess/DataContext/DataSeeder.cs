using Bogus;
using DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.DataContext;

public static class DataSeeder
{
    public static void SeedCategory(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
        if (dbContext.Categories.Count() > 10)
        {
            return;
        }

        var faker = new Faker();
        List<Category> categories = [];
        List<string> categoryNames =
        [
            "Dog Food",
            "Cat Food",
            "Pet Toys",
            "Aquarium Supplies",
            "Bird Accessories",
            "Reptile Supplies",
            "Grooming Tools",
            "Pet Health",
            "Pet Clothing",
            "Pet Beds",
        ];
        categories.AddRange(
            categoryNames.Select(s => new Category(
                s,
                faker.Image.PicsumUrl(),
                faker.Commerce.ProductDescription()
            ))
        );
        dbContext.Categories.AddRange(categories);
        dbContext.SaveChanges();
    }

    public static void SeedProduct(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
        if (dbContext.Products.Count() > 10)
        {
            return;
        }

        var faker = new Faker();
        List<Product> products = [];
        var categoryIds = dbContext.Categories.Select(c => c.Id).ToList();
        for (var i = 0; i < 10; i++)
        {
            var product = new Product(
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                faker.Image.PicsumUrl(),
                faker.Commerce.ProductDescription(),
                1000M,
                1000,
                faker.PickRandom(categoryIds)
            );
            products.Add(product);
        }

        dbContext.Products.AddRange(products);
        dbContext.SaveChanges();
    }
}
