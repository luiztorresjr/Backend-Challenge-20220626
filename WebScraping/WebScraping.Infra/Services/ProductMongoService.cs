using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Drawing.Printing;
using WebScraping.Infra.Cache;
using WebScraping.Infra.Models;
using WebScraping.Infra.Repository;

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

        public ProductModel Create(ProductModel product)
        {
            var entity = _mapper.Map<ProductEntity>(product);
            _products.Create(entity);
            var cacheKey = $"{keyCache}/{product.Barcode}";
            _cache.Set(cacheKey, product);
            return Get(entity.Id);
        }

        public Task<ProductModel>? Create(ProductEntity product)
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

        public Result<ProductModel> Get(int page, int pageSize)
        {
            var cacheKey = $"{keyCache}/{page}/{pageSize}";
            var products = _cache.Get<Result<ProductModel>>(cacheKey);
            if(products == null)
            {
                products = _mapper.Map<Result<ProductModel>>(_products.GetAsync(page, pageSize));
            }
            return products;
        }

        public Task<List<ProductEntity>> CreateMany(List<ProductEntity> products)
        {
            foreach (var item in products)
            {
                var product = _mapper.Map<ProductModel>(item);
                Create(product);
            }
            return Task.FromResult(products);
        }

        public ProductModel Get(string id)
        {
            var cacheKey = $"{keyCache}/{id}";

            var product = _cache.Get<ProductModel>(cacheKey);

            if (product is null)
            {
                product = _mapper.Map<ProductModel>(_products.Get(id));
                _cache.Set(cacheKey, product);
            }

            return product;
        }


        public bool GetByCodeExistsAsync(long code)
        {
            var cacheKey = $"{keyCache}/{code}";
            var product = _cache.Get<ProductModel>(cacheKey);
            if (product is null)
            {
                product = _mapper.Map<ProductModel>(_products.Get(code));
                _cache.Set(cacheKey, product);
            }
            if (product is null)
                return false;
            return true;
        }

        public Task<ProductModel?> GetById(string id)
        {
            var cacheKey = $"{keyCache}/{id}";
            var product = _cache.Get<ProductModel>(cacheKey);
            if (product is null)
            {
                product = _mapper.Map<ProductModel>(_products.GetAsync(id));
                _cache.Set(cacheKey, product);
            }
            return Task.FromResult(result: product);
        }

        public Task Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Task Update(string id, ProductModel product)
        {
            throw new NotImplementedException();
        }

        Task<ProductModel> IProductMongoService.Create(ProductModel product)
        {
            throw new NotImplementedException();
        }

        Task<bool> IProductMongoService.GetByCodeExistsAsync(long code)
        {
            var cacheKey = $"{keyCache}/{code}";
            var product = _cache.Get<ProductModel>(cacheKey);
            if(product is null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }
}
