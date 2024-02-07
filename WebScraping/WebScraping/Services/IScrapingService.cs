using WebScraping.Model;

namespace WebScraping.Services
{
    public interface IScrapingService
    {
        Task<List<Product>> GetProductsAsync();
    }
}
