using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs;

public record ProductDto(
    [MaxLength(2000)] string Name,
    [MaxLength(2000)] string Description,
    IFormFile? Image,
    [MaxLength(2000)] string Ingredient,
    decimal Price,
    int Stock,
    int CategoryId
);
