using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Presentation.Extensions;

public static class Extension
{
    public static void AddPersistence(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
        serviceCollection.AddScoped<ICategoryService, CategoryService>();
        serviceCollection.AddScoped<IProductService, ProductService>();

        serviceCollection.AddScoped<BlobService>(provider =>
        {
            var rootPath = provider.GetRequiredService<IHostEnvironment>().ContentRootPath;
            return new BlobService(rootPath);
        });

        serviceCollection.AddControllers();
    }

    public static void ConfigCors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
            );
        });
    }

    public static void ConfigDatabase(this IServiceCollection serviceCollection)
    {
        var conn = ConnString.SqlServer();
        serviceCollection.AddDbContext<StoreDbContext>(opt => opt.UseSqlServer(conn));
    }

    public static void UseUploadStatic(this WebApplication webApplication)
    {
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload");
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath); // Ensure the directory exists
        }

        webApplication.UseStaticFiles(
            new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uploadPath),
                RequestPath =
                    "/upload" // URL prefix for accessing the files
                ,
            }
        );
    }

    public static void AddDataSeeder(this IServiceProvider serviceProvider)
    {
        DataSeeder.SeedCategory(serviceProvider);
        DataSeeder.SeedProduct(serviceProvider);
    }
}
