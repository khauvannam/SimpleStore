using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Models;
using Presentation.DTOs;

namespace Presentation.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService, BlobService blobService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string? imageUrl = null;

        if (productDto.Image is not null)
        {
            try
            {
                var fileName =
                    Path.GetRandomFileName() + Path.GetExtension(productDto.Image.FileName);

                await using var fileStream = productDto.Image.OpenReadStream();

                imageUrl = await blobService.UploadFileAsync(fileStream, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error uploading file: {ex.Message}" });
            }
        }

        var product = new Product(
            productDto.Name,
            productDto.Description,
            imageUrl!,
            productDto.Ingredient,
            productDto.Price,
            productDto.Stock,
            productDto.CategoryId
        );

        var createdProduct = await productService.CreateAsync(product);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = createdProduct.Id },
            createdProduct
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingProduct = await productService.GetAsync(id);
        if (existingProduct == null)
        {
            return NotFound(new { message = "Product not found." });
        }

        var imageUrl = existingProduct.Image;

        if (productDto.Image != null)
        {
            try
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    blobService.DeleteFile(Path.GetFileName(imageUrl));
                }

                var fileName =
                    Path.GetRandomFileName() + Path.GetExtension(productDto.Image.FileName);
                await using var fileStream = productDto.Image.OpenReadStream();

                imageUrl = await blobService.UploadFileAsync(fileStream, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error uploading file: {ex.Message}" });
            }
        }

        existingProduct.Update(
            productDto.Name,
            productDto.Description,
            imageUrl,
            productDto.Ingredient,
            productDto.Price,
            productDto.Stock,
            productDto.CategoryId
        );

        var updatedProduct = await productService.UpdateAsync(existingProduct);

        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await productService.GetAsync(id);
        if (product == null)
        {
            return NotFound(new { message = "Product not found." });
        }

        if (!string.IsNullOrEmpty(product.Image))
        {
            try
            {
                blobService.DeleteFile(Path.GetFileName(product.Image));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error deleting file: {ex.Message}" });
            }
        }

        await productService.DeleteAsync(product);
        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await productService.GetAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] int? limit = 4)
    {
        var products = await productService.GetAllAsync(limit);
        return Ok(products);
    }
}
