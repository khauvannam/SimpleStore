using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService, BlobService blobService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] int? limit = 4)
    {
        var categories = await categoryService.GetAllAsync(limit);
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromForm] CategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string? imageUrl = null;

        if (categoryDto.Image is not null)
        {
            try
            {
                var fileName =
                    Path.GetRandomFileName() + Path.GetExtension(categoryDto.Image.FileName);
                await using var fileStream = categoryDto.Image.OpenReadStream();
                imageUrl = await blobService.UploadFileAsync(fileStream, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error uploading file: {ex.Message}" });
            }
        }

        var category = new Category(
            categoryDto.Name,
            imageUrl ?? string.Empty,
            categoryDto.Description
        );

        var createdCategory = await categoryService.CreateAsync(category);

        return CreatedAtAction(
            nameof(GetCategoryById),
            new { id = createdCategory.Id },
            createdCategory
        );
    }

    // Update Category
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCategory = await categoryService.GetAsync(id);

        if (existingCategory is null)
        {
            return NotFound(new { message = "Category not found." });
        }

        var imageUrl = existingCategory.Image;

        if (categoryDto.Image is not null)
        {
            try
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    blobService.DeleteFile(Path.GetFileName(imageUrl));
                }

                var fileName =
                    Path.GetRandomFileName() + Path.GetExtension(categoryDto.Image.FileName);
                await using var fileStream = categoryDto.Image.OpenReadStream();
                imageUrl = await blobService.UploadFileAsync(fileStream, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error uploading file: {ex.Message}" });
            }
        }

        existingCategory.Update(
            categoryDto.Name,
            imageUrl,
            categoryDto.Description,
            existingCategory.Products
        );

        var updatedCategory = await categoryService.UpdateAsync(existingCategory);

        return Ok(updatedCategory);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await categoryService.GetAsync(id);
        if (category is null)
        {
            return NotFound(new { message = "Category not found." });
        }

        // Delete the image if it exists
        if (!string.IsNullOrEmpty(category.Image))
        {
            try
            {
                blobService.DeleteFile(Path.GetFileName(category.Image));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error deleting file: {ex.Message}" });
            }
        }

        await categoryService.DeleteAsync(category);
        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await categoryService.GetAsync(id);
        if (category is null)
        {
            return NotFound();
        }

        return Ok(category);
    }
}
