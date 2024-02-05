using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Infra.Models;

namespace WebScraping.Infra.Services
{
    public interface IMongoDBService 
    {
        Task AddOneProduct(ProductMongo product);

        Task<List<ProductMongo>> GetAllProducts();

        Task<ProductMongo> GetProductById(string id);

        Task UpdateProduct(string id, ProductMongo product);

        Task DeleteProduct(string id);
    }
}
