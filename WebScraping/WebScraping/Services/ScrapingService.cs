using AutoMapper;
using WebScraping.Infra.Scraping;
using WebScraping.Model;

namespace WebScraping.Services
{
    public class ScrapingService : IScrapingService
    {
        private readonly IMapper _mapper;
        private readonly IWebScrapingService _service;

        public ScrapingService(IWebScrapingService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var result = await _service.GetProductUsingScraping();
                var response = new List<Product>();
                if (result != null)
                {
                    response = _mapper.Map<List<Product>>(result);
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return [];
            }
        }
    }
}
