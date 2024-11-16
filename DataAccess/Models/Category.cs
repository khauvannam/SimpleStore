using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class Category(string name, string image, string description)
{
    public int Id { get; set; }

    [MaxLength(2000)]
    public string Name { get; set; } = name;

    [MaxLength(2000)]
    public string Image { get; set; } = image;

    [MaxLength(2000)]
    public string Description { get; set; } = description;

    public List<Product> Products { get; init; } = [];

    public Category Update(string name, string image, string description, List<Product> products)
    {
        Name = name;
        Image = image;
        Description = description;
        Products.AddRange(products);
        return this;
    }
}
