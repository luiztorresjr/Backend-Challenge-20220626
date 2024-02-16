using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Infra.Models;

namespace WebScraping.Infra.Services
{
    public interface IProductMongoService 
    {
        Result<ProductModel> Get(int page,  int pageSize);
        Task<ProductModel> GetById(string id);
        Task<bool> GetByCodeExistsAsync(long  code);
        Task<ProductModel> Create(ProductModel product);
        Task<ProductModel> Create(ProductEntity product);
        Task<List<ProductEntity>> CreateMany(List<ProductEntity> products);
        Task Update(string id, ProductModel product);
        Task Remove(string id);
    }
}
