
using WebScraping.Infra.Models;
using WebScraping.Model;

namespace WebScraping.Infra.Scraping
{
    public interface IWebScrapingService
    {
        Task<Result<Product>> GetProductUsingScraping(int page=1, int pageSize=100);
    }
}
