using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using YourProjectNamespace.Models;
using System;

namespace YourProjectNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static List<Product> products = new List<Product>()
        {
            new Product { Id = 1, Name = "Product 1", Description = "Description of Product 1", Price = 100 },
            new Product { Id = 2, Name = "Product 2", Description = "Description of Product 2", Price = 200 }
        };

        private static readonly Dictionary<int, Product> productCache = new Dictionary<int, Product>();

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            _logger.LogInformation("Fetching all products");
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            _logger.LogInformation($"Fetching product by ID: {id}");
            var product = GetProductById(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID: {id} not found.");
                return NotFound("Product not found");
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            _logger.LogInformation($"Attempting to create a product: {product.Name}");
            products.Add(product);
            productCache[product.Id] = product;
            _logger.LogInformation($"Product created with ID: {product.Id}");
            return CreatedAtAction("Get", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Product updatedProduct)
        {
            _logger.LogInformation($"Attempting to update product: {id}");
            var product = GetProductById(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID: {id} not found for update.");
                return NotFound("Product not found");
            }

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            productCache[product.Id] = product;
            _logger.LogInformation($"Product with ID: {id} updated");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"Attempting to delete product: {id}");
            var product = GetProductById(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID: {id} not found for deletion.");
                return NotFound("Product not found");
            }

            products.Remove(product);
            productCache.Remove(product.Id);
            _logger.LogInformation($"Product with ID: {id} deleted");
            return Ok();
        }

        private Product GetProductById(int id)
        {
            _logger.LogDebug($"Searching product with ID: {id} in cache");
            if (!productCache.TryGetValue(id, out Product product))
            {
                product = products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    _logger.LogDebug($"Product with ID: {id} found, adding to cache.");
                    productCache[id] = product;
                }
                else
                {
                    _logger.LogDebug($"Product with ID: {id} not found.");
                }
            }
            return product;
        }

        private string GetEnvironmentSetting()
        {
            var value = Environment.GetEnvironmentVariable("YOUR_ENV_VARIABLE");
            return value ?? "Default Value";
        }
    }
}