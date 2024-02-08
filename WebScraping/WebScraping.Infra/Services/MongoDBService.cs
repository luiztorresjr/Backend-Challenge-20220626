using HtmlAgilityPack;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WebScraping.Infra.Models;

namespace WebScraping.Infra.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoCollection<ProductMongo> _products;

        public MongoDBService(IOptions<MongoDBSetttings> settings)
        {
            MongoClient client = new MongoClient(settings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(settings.Value.DatabaseName);
            _products = database.GetCollection<ProductMongo>(settings.Value.CollectionName);
        }

        public async Task AddOneProduct(ProductMongo product)
        {
            await _products.InsertOneAsync(product);
            return;
        }

        public async Task<List<ProductMongo>> GetAllProducts()
        {
            return await _products.Find(new BsonDocument()).ToListAsync<ProductMongo>();
        }

        public async Task UpdateProduct(string id, ProductMongo updateProduct)
        {
            FilterDefinition<ProductMongo> filter = Builders<ProductMongo>.Filter.Eq("Id", id);
            ProductMongo old = await _products.Find(filter).FirstAsync<ProductMongo>();

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
            return;
        }

        public async Task DeleteProduct(string id)
        {
            FilterDefinition<ProductMongo> filter = Builders<ProductMongo>.Filter.Eq("Id", id);
            await _products.DeleteOneAsync(filter);
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
                return null;
            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }

        public async Task<ProductMongo>? GetProductByCode(long Code)
        {
            FilterDefinition<ProductMongo> filter = Builders<ProductMongo>
                .Filter.Eq("Code", Code);
            return await _products.Find(filter).FirstAsync<ProductMongo>();
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
