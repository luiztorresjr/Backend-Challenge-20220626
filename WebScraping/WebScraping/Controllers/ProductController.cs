using Microsoft.AspNetCore.Mvc;
using WebScraping.Model;
using WebScraping.Services;

namespace WebScraping.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _service.AddProduct(product);
            return CreatedAtAction(nameof(GetProducts), new {id = product.Id}, product);
            
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _service.GetAllProducts();
            if (products != null)
                return Ok(products);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            var products = await _service.GetProductById(id);
            if(products != null)
                return Ok(products);
            else
                return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(string id, [FromBody] Product product)
        {
            _service.UpdateProduct(id, product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _service.DeleteProduct(id);
            return NoContent();
        }
    }
}
