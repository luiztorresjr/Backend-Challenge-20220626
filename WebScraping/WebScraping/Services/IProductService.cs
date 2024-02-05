using WebScraping.Model;

namespace WebScraping.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(string productId);
        Task<Product?> UpdateProduct(string id, Product produto);
        Task DeleteProduct(string id);
        Task<Product?> AddProduct(Product produto);
    }
}
