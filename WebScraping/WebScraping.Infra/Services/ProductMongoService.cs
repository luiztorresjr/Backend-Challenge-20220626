using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using WebScraping.Infra.Models;
using WebScraping.Infra.Repository;
using WebScraping.Model;

namespace WebScraping.Infra.Services
{
    public class ProductMongoService : IProductMongoService
    {
        private readonly ILogger<ProductMongoService> _logger;
        private readonly IMapper _mapper;
        private readonly ICacheRedisService _cache;
        private readonly IMongoRepository<ProductEntity> _products;
        private readonly string keyCache = "product";
        public ProductMongoService(IMapper mapper, IMongoRepository<ProductEntity> products, ILogger<ProductMongoService> logger, ICacheRedisService cache)
        {
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
            _products = products;
        }

        public Product Create(Product product)
        {
            var entity = _mapper.Map<ProductEntity>(product);
            _products.Create(entity);
            var cacheKey = $"{keyCache}/{product.Barcode}";
            _cache.Set(cacheKey, entity);
            return Get(entity.Id);
        }

        public Task<Product>? Create(ProductEntity product)
        {
            try
            {
                var reponse = _products.Create(product);
                var cacheKey = $"{keyCache}/{product.Barcode}";
                _cache.Set(cacheKey, product);
                return Task.FromResult(Get(reponse.Id));
            }catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default;
            }
           
        }

        public Result<Product> Get(int page, int pageSize)
        {
            var cacheKey = $"{keyCache}/{page}/{pageSize}";
            var products = _cache.Get<Result<ProductEntity>>(cacheKey);
            if(products == null)
            {
                products = _mapper.Map<Result<ProductEntity>>(_products.GetAsync(page, pageSize));
            }
            var response = _mapper.Map<Result<Product>>(products);
            return response;
        }

        public Task<List<ProductEntity>> CreateMany(List<ProductEntity> products)
        {
            foreach (var item in products)
            {
                var product = _mapper.Map<Product>(item);
                Create(product);
            }
            return Task.FromResult(products);
        }

        public Product Get(string id)
        {
            var cacheKey = $"{keyCache}/{id}";

            var product = _cache.Get<ProductEntity>(cacheKey);

            if (product is null)
            {
                product = _mapper.Map<ProductEntity>(_products.Get(id));
                _cache.Set(cacheKey, product);
            }
            var response = _mapper.Map<Product>(product);
            return response;
        }


        public bool GetByCodeExistsAsync(long code)
        {
            var cacheKey = $"{keyCache}/{code}";
            var product = _cache.Get<ProductEntity>(cacheKey);
            if (product is null)
            {
                product = _mapper.Map<ProductEntity>(_products.Get(code));
                _cache.Set(cacheKey, product);
            }
            if (product is null)
                return false;
            return true;
        }

        public Task<Product> GetById(string id)
        {
            var cacheKey = $"{keyCache}/{id}";
            var product = _cache.Get<ProductEntity>(cacheKey);
            if (product is null)
            {
                product = _mapper.Map<ProductEntity>(_products.GetAsync(id));
                _cache.Set(cacheKey, product);
            }
            var response = _mapper.Map<Product>(product);
            return Task.FromResult(result: response);
        }

        public Task Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Task Update(string id, Product product)
        {
            throw new NotImplementedException();
        }

        Task<Product> IProductMongoService.Create(Product product)
        {
            try
            {
                var request = _mapper.Map<ProductEntity>(product);
                var response = _products.Create(request);
                var reponse = _mapper.Map<Product>(response);
                var cacheKey = $"{keyCache}/{product.Barcode}";
                _cache.Set(cacheKey, response);

                return Task.FromResult(reponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default;
            }
        }

        Task<bool> IProductMongoService.GetByCodeExistsAsync(long code)
        {
            var cacheKey = $"{keyCache}/{code}";
            var product = _cache.Get<ProductEntity>(cacheKey);
            if(product is null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        
    }
}
