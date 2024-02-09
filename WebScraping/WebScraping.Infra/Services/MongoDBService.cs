using HtmlAgilityPack;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebScraping.Infra.Models;

namespace WebScraping.Infra.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoCollection<ProductMongo> _products;
        private readonly ILogger<MongoDBService> _logger;

        public MongoDBService(IOptions<MongoDBSetttings> settings, ILogger<MongoDBService> logger)
        {
            MongoClient client = new MongoClient(settings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(settings.Value.DatabaseName);
            _products = database.GetCollection<ProductMongo>(settings.Value.CollectionName);
            _logger = logger;
        }

        public async Task AddOneProduct(ProductMongo product)
        {
            try
            {
                await _products.InsertOneAsync(product);
                _logger.LogInformation("inserido com sucesso id:" + product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"problema ao inserir produto: {product.Code} - {AddOneProduct} {DateTime.UtcNow} - erro ex: {ex.Message}");
            }
            return;
        }

        public async Task<List<ProductMongo>> GetAllProducts()
        {
            try
            {
                var products = await _products.Find(new BsonDocument()).ToListAsync<ProductMongo>();
                if (products != null && products.Count() > 0)
                {
                    return products;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"problema ao buscar produtos - {GetAllProducts} {DateTime.UtcNow} - erro ex: {ex.Message}");
            }
            return new List<ProductMongo>();

        }

        public async Task UpdateProduct(string id, ProductMongo updateProduct)
        {
            try
            {
                FilterDefinition<ProductMongo> filter = Builders<ProductMongo>.Filter.Eq("Id", id);
                var products = GetAllProducts();
                var old = new ProductMongo();
                if (products == null || products.Result.Count() == 0)
                {
                    return;
                }
                else
                {
                    old = products.Result.FirstOrDefault(i => i.Id == id);
                }
                if (old.Id != null)
                {
                    UpdateDefinition<ProductMongo> update =
                        Builders<ProductMongo>.Update
                        .Set(product => product.Code, updateProduct.Code < 1 ? old.Code : updateProduct.Code)
                        .Set(product => product.Barcode, String.IsNullOrEmpty(updateProduct.Barcode) ? old.Barcode : updateProduct.Barcode)
                        .Set(product => product.Status, String.IsNullOrEmpty(updateProduct.Status) ? old.Status : updateProduct.Status)
                        .Set(product => product.Imported, updateProduct.Imported == DateTime.MinValue ? old.Imported : updateProduct.Imported)
                        .Set(product => product.Url, String.IsNullOrEmpty(updateProduct.Url) ? old.Url : updateProduct.Url)
                        .Set(product => product.ProductName, String.IsNullOrEmpty(updateProduct.ProductName) ? old.ProductName : updateProduct.ProductName)
                        .Set(product => product.Quantity, String.IsNullOrEmpty(updateProduct.Quantity) ? old.Quantity : updateProduct.Quantity)
                        .Set(product => product.Categories, String.IsNullOrEmpty(updateProduct.Categories) ? old.Categories : updateProduct.Categories)
                        .Set(product => product.Packaging, String.IsNullOrEmpty(updateProduct.Packaging) ? old.Packaging : updateProduct.Packaging)
                        .Set(product => product.Brands, String.IsNullOrEmpty(updateProduct.Brands) ? old.Brands : updateProduct.Brands)
                        .Set(product => product.ImageUrl, String.IsNullOrEmpty(updateProduct.ImageUrl) ? old.ImageUrl : updateProduct.ImageUrl);
                    await _products.UpdateOneAsync(filter, update);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"problema ao atualizar o produtos - {UpdateProduct} {DateTime.UtcNow} - erro ex: {ex.Message}");
            }
            return;
        }

        public async Task DeleteProduct(string id)
        {
            try
            {
                FilterDefinition<ProductMongo> filter = Builders<ProductMongo>.Filter.Eq("Id", id);
                var products = GetAllProducts();
                var old = new ProductMongo();
                if (products == null || products.Result.Count() == 0)
                {
                    return;
                }
                else
                {
                    old = products.Result.FirstOrDefault(i => i.Id == id);
                    if (old.Id != null)
                    {
                        await _products.DeleteOneAsync(filter);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"problema ao deletar o produtos - {DeleteProduct} {DateTime.UtcNow} - erro ex: {ex.Message}");
            }

            return;
        }

        public async Task<ProductMongo> GetProductById(string id)
        {
            var products = await _products.Find(new BsonDocument()).ToListAsync<ProductMongo>();
            try
            {
                var product = products.FirstOrDefault(i => i.Id == id);
                if (product != null && product.Id == id)
                {
                    return product;
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError($"problema ao deletar o produtos - {GetProductById} {DateTime.UtcNow} - erro ex: {ex.Message}");
            }
            return new ProductMongo();
        }

        public async Task<ProductMongo>? GetProductByCode(long Code)
        {
            var products = await _products.Find(new BsonDocument()).ToListAsync<ProductMongo>();
            try
            {
                var product = products.FirstOrDefault(i => i.Code == Code);
                if (product != null && product.Code == Code)
                {
                    return product;
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"problema ao deletar o produtos - {GetProductById} {DateTime.UtcNow} - erro ex: {ex.Message}");
            }
            return new ProductMongo();
        }

        public async Task AddOrUpdateMany(List<ProductMongo> products)
        {
            foreach (ProductMongo product in products)
            {
                var exists = GetProductByCode(product.Code).Result;
                if (exists != null)
                {
                    FilterDefinition<ProductMongo> filter = Builders<ProductMongo>.Filter.Eq("Id", exists.Id);
                    ProductMongo old = await _products.Find(filter).FirstAsync<ProductMongo>();

                    UpdateDefinition<ProductMongo> update =
                        Builders<ProductMongo>.Update
                        .Set(product => product.Code, exists.Code < 1 ? old.Code : product.Code)
                        .Set(product => product.Barcode, String.IsNullOrEmpty(product.Barcode) ? old.Barcode : product.Barcode)
                        .Set(product => product.Status, String.IsNullOrEmpty(product.Status) ? old.Status : product.Status)
                        .Set(product => product.Imported, product.Imported == DateTime.MinValue ? old.Imported : product.Imported)
                        .Set(product => product.Url, String.IsNullOrEmpty(product.Url) ? old.Url : product.Url)
                        .Set(product => product.ProductName, String.IsNullOrEmpty(product.ProductName) ? old.ProductName : product.ProductName)
                        .Set(product => product.Quantity, String.IsNullOrEmpty(product.Quantity) ? old.Quantity : product.Quantity)
                        .Set(product => product.Categories, String.IsNullOrEmpty(product.Categories) ? old.Categories : product.Categories)
                        .Set(product => product.Packaging, String.IsNullOrEmpty(product.Packaging) ? old.Packaging : product.Packaging)
                        .Set(product => product.Brands, String.IsNullOrEmpty(product.Brands) ? old.Brands : product.Brands)
                        .Set(product => product.ImageUrl, String.IsNullOrEmpty(product.ImageUrl) ? old.ImageUrl : product.ImageUrl);
                    await _products.UpdateOneAsync(filter, update);
                }
                else
                {
                   await _products.InsertOneAsync(product);
                }
            }
            return;
        }

        public async Task AddOrUpdateMany(ProductMongo product)
        {
            var exists = GetProductByCode(product.Code).Result;
            if (exists != null)
            {
                _ = UpdateProduct(exists.Id, product);
            }
            else
            {
                _ = AddOneProduct(product);
            }

            return;
        }
        public async Task<bool> GetProductByCodeExistsAsync(long Code)
        {
            var products = await _products.Find(new BsonDocument()).ToListAsync<ProductMongo>();
            try
            {
                var product = products.FirstOrDefault(i => i.Code == Code);
                if (product != null && product.Code == Code)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        
    }
}
