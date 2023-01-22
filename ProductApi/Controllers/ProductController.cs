using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductApi.Models;
using ProductApi.Services;
using ProductApi.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductApi.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productService;

        public ProductController(IMapper mapper, ILogger<ProductController> logger, IProductRepository productService)
        {
            _mapper = mapper;
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("Product/{id}")]
        [SwaggerResponse(200, type: typeof(Product))]
        public async Task<IActionResult> GetProduct(int id) 
        {
            Product product;

            try
            {
                product = await _productService.GetByIdAsync(id);

                if (product == null)
                {
                    _logger.LogError($"Product with id - {id} not found");
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product by id");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(product);
        }

        [HttpGet("GetProductsByName/{name}")]
        [SwaggerResponse(200, type: typeof(List<Product>))]
        public async Task<IActionResult> GetProductByName(string name)
        {
            List<Product> products;

            try
            {
                products = await _productService.GetByNameAsync(name);

                if (products == null)
                {
                    _logger.LogError($"Product matching name - {name} not found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product by name");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(products);
        }

        [HttpGet("GetProductsByPriceRange/{from}/{to}")]
        [SwaggerResponse(200, type: typeof(List<Product>))]
        public async Task<IActionResult> GetProductByPriceRange(decimal from, decimal to)
        {
            List<Product> products;

            if((from < 0 || to < 0) || (from > to))
            {
                return BadRequest("Invalid price range");
            }

            try
            {
                products = await _productService.GetByPriceRange(from, to);

                if (products == null)
                {
                    _logger.LogError($"No product with price between - {from} and {to} - found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product by price");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(products);
        }

        [HttpPost("Product")]
        [SwaggerResponse(200)]
        public async Task<IActionResult> CreateProduct(ProductVM product)
        {
            _logger.LogInformation($"Add Request:{JsonConvert.SerializeObject(product)}");

            try
            {
                await _productService.AddAsync(_mapper.Map<Product>(product));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [HttpPut("Product")]
        [SwaggerResponse(200)]
        public async Task<IActionResult> UpdateProduct(int id, ProductVM product)
        {
            _logger.LogInformation($"Update Request - id: {id}, payload: {JsonConvert.SerializeObject(product)}");

            try
            {
                var productToUpdate = await _productService.GetByIdAsync(id);
                if(productToUpdate != null)
                {
                    productToUpdate.Name= product.Name;
                    productToUpdate.Description= product.Description;
                    productToUpdate.Price= product.Price;

                    await _productService.UpdateAsync(productToUpdate);
                }
                else
                {
                    _logger.LogError($"Product to update with id - {id} - not found");
                    return BadRequest("Invalid product id");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [HttpDelete("Product")]
        [SwaggerResponse(200)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"Delete product with id: {id}");

            try
            {
                var productToDelete = await _productService.GetByIdAsync(id);
                if (productToDelete != null)
                {
                    await _productService.DeleteAsync(productToDelete);
                }
                else
                {
                    _logger.LogError($"Product to delete with id - {id} - not found");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

    }
}