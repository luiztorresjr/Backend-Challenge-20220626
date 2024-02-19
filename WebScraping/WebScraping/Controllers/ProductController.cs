using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WebScraping.Infra.Models;
using WebScraping.Model;
using WebScraping.Services;

namespace WebScraping.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IProductService _service;
        private IScrapingService _scrapingService;


        public ProductController(IProductService service, IScrapingService scrapingService)
        {
            _service = service;
            _scrapingService = scrapingService;
        }

        [HttpGet]
        public IActionResult Get()
        {

            return Ok("Fullstack Challenge 20201026");
        }

        [HttpGet("products")]
        public async Task<ActionResult<Result<Product>>> GetProductsAsync(int page = 1, int pageSize = 10)
        {
            var products = await _service.GetAllProducts(page, pageSize);            
            if(products != null)
                return Ok(products);                
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
            var products = await _service.GetProductById(id);
            if (products.Id != null)
                return Ok(products);
            else
                return NoContent();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

#if DEBUG

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

        [HttpGet("GetByWebScraping")]
        public async Task<ActionResult<Result<Product>>> GetProductsByScraping(int page = 1, int pageSize = 10)
        {
            var products = await _scrapingService.GetProductsAsync(page, pageSize);
            if (products != null)
                return Ok(products);
            else
                return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                var products = await _service.AddProduct(product);
                if (products.Id != null)
                    return Created();
                else
                    return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }

        }
#endif
    }
}
