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
        private IScrapingService _scrapingService;


        public ProductController(IProductService service, IScrapingService scrapingService)
        {
            _service = service;
            _scrapingService = scrapingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _service.AddProduct(product);
            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
            
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10)
        {
            var products = await _service.GetAllProducts();
            if (products != null)
            {
                var total = products.Count;
                var totalPage = (int)Math.Ceiling((decimal)total / pageSize);
                var producsPerPage = products
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                return Ok(producsPerPage);
                
            }
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            var products = await _service.GetProductById(id);
            if (products != null)
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

        [HttpGet("GetByWebScraping")]
        public async Task<IActionResult> GetProductsByScraping()
        {
            var products = await _scrapingService.GetProductsAsync();
            if (products != null)
                return Ok(products);
            else
                return NoContent();
        }
        [HttpGet]
        public IActionResult Get()
        {

            return Ok("Fullstack Challenge 20201026");
        }
    }
}
