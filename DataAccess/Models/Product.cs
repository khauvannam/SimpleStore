using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class Product(
    string name,
    string description,
    string image,
    string ingredient,
    decimal price,
    int stock,
    int categoryId
)
{
    public int Id { get; set; }

    [MaxLength(2000)]
    public string Name { get; set; } = name;

    [MaxLength(2000)]
    public string Description { get; set; } = description;

    [MaxLength(2000)]
    public string Image { get; set; } = image;

    [MaxLength(2000)]
    public string Ingredient { get; set; } = ingredient;

    public decimal Price { get; set; } = price;
    public int Stock { get; set; } = stock;
    public int CategoryId { get; set; } = categoryId;
    public Category Category { get; set; } = null!;

    public Product Update(
        string name,
        string description,
        string image,
        string ingredient,
        decimal price,
        int stock,
        int categoryId
    )
    {
        Name = name;
        Description = description;
        Image = image;
        Ingredient = ingredient;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        return this;
    }
}
