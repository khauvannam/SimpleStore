using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs;

public record CategoryDto(
    [MaxLength(2000)] string Name,
    IFormFile? Image,
    [MaxLength(2000)] string Description
);
