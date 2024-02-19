using AutoMapper;
using IdentityModel.OidcClient;
using WebScraping.Infra.Models;
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

        public async Task<Result<Product>> GetProductsAsync(int page, int pageSize)
        {
            try
            {
                var result = await _service.GetProductUsingScraping();
                var response = new Result<Product>();
                if (result != null)
                {
                    response = _mapper.Map<Result<Product>>(result);
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return default;
            }
        }
    }
}
