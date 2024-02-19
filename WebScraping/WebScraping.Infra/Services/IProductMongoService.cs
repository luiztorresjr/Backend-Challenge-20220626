using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Infra.Models;
using WebScraping.Model;

namespace WebScraping.Infra.Services
{
    public interface IProductMongoService 
    {
        Result<Product> Get(int page,  int pageSize);
        Task<Product> GetById(string id);
        Task<bool> GetByCodeExistsAsync(long  code);
        Task<Product> Create(Product product);
        Task<Product> Create(ProductEntity product);
        Task<List<ProductEntity>> CreateMany(List<ProductEntity> products);
        Task Update(string id, Product product);
        Task Remove(string id);
    }
}
