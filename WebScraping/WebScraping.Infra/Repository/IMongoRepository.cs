using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Infra.Bases;
using WebScraping.Infra.Models;
using MongoDB.Bson;

namespace WebScraping.Infra.Repository
{
    public interface IMongoRepository<T> where T : BaseModel
    {
        Result<T> GetAsync(int page, int pageSize);
        Task<T> GetAsync(string id);
        T Get(string id);
        T Create(T product);

        T? GetByCode(long code);
        void Update(string id,  T product);
        void Remove(string id);
        T Get(long code);
        Task<Result<T>> GetAllAsync(int page, int pageSize);

        List<T> CreateMany(List<T> product);

        bool GetByCodeExistsAsync(long code);

    }
}
