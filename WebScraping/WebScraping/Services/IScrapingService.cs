using WebScraping.Infra.Models;
using WebScraping.Model;

namespace WebScraping.Services
{
    public interface IScrapingService
    {
        Task<Result<Product>> GetProductsAsync(int page, int pageSize);
    }
}
