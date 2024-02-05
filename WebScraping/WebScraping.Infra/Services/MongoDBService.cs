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
            FilterDefinition<ProductMongo> filter = Builders<ProductMongo>.Filter.Eq("Id", id);
            return await _products.Find(filter).FirstAsync<ProductMongo>();
        }
    }
}
