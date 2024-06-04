using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var product = GetProductById(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            products.Add(product);
            productCache[product.Id] = product;
            return CreatedAtAction("Get", new { id = product.Id }, product);
        }

        [HttpPut("{ in}")]
        public ActionResult Put(int id, [FromBody] Product updatedProduct)
        {
            var product = GetProductById(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            productCache[product.Id] = product;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = GetProductById(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            products.Remove(product);
            productCache.Remove(product.Id);
            return Ok();
        }

        private Product GetProductById(int id)
        {
            if (!productCache.TryGetValue(id, out Product product))
            {
                product = products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    productCache[id] = product;
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