using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WebScraping.Infra.Bases;
using WebScraping.Infra.Models;

namespace WebScraping.Infra.Repository
{
    public partial class MongoRepository<T> : IMongoRepository<T> where T : BaseModel
    {
        /// <summary>
        /// Gets the collection
        /// </summary>
        /// 
        private ILogger<MongoRepository<T>> _logger; 
        protected IMongoCollection<T> _collection;
        public IMongoCollection<T> Collection
        {
            get
            {
                return _collection;
            }
        }

        /// <summary>
        /// Mongo Database
        /// </summary>
        protected IMongoDatabase _database;
        public IMongoDatabase Database
        {
            get
            {
                return _database;
            }
        }


        /// <summary>
        /// Ctor
        /// </summary>        
        public MongoRepository(IMongoDBSettings dataProvider, ILogger<MongoRepository<T>> logger)
        {
            _logger = logger;
            if (!string.IsNullOrEmpty(dataProvider.ConnectionURI))
            {
                var client = new MongoClient(dataProvider.ConnectionURI);
                _database = client.GetDatabase(dataProvider.DatabaseName);
                _collection = _database.GetCollection<T>(typeof(T).Name);

                try
                {
                    var result = client.GetDatabase(dataProvider.DatabaseName).RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                    _logger.LogInformation("Pinged your deployment. You successfully connected to MongoDB!");
                }
                catch (Exception ex) { 
                    Console.WriteLine(ex);
                    _logger.LogError(ex.Message);
                }

            }
        }
        public MongoRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var databaseName = new MongoUrl(connectionString).DatabaseName;
            _database = client.GetDatabase(databaseName);
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }


        public Result<T> GetAsync(int page, int pageSize)
        {
            var result = new Result<T>();
            result.Page = page;
            result.Quantity = pageSize;
            
            result.Data = _collection.Find(_=> true)
                .Skip((page - 1) * pageSize).Limit(pageSize).ToList();

            result.Total = _collection.CountDocuments(_=> true);
            result.TotalPages = result.Total / pageSize;

            return result;

        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(string id)
        {
            return _collection.Find(e => e.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get entity by identifier async
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual Task<T> GetByIdAsync(string id)
        {
            return _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual T Insert(T entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        /// <summary>
        /// Async Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<T> InsertAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        /// <summary>
        /// Async Insert many entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task InsertManyAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<T> entities)
        {
            _collection.InsertMany(entities);
        }

        /// <summary>
        /// Async Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<T>> InsertAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
            return entities;
        }


        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual T Update(T entity)
        {
            _collection.ReplaceOne(x => x.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false });
            return entity;

        }

        /// <summary>
        /// Async Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<T> UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false });
            return entity;
        }


        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Update(entity);
            }
        }

        /// <summary>
        /// Async Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await UpdateAsync(entity);
            }
            return entities;
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity)
        {
            _collection.FindOneAndDelete(e => e.Id == entity.Id);
        }

        /// <summary>
        /// Async Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<T> DeleteAsync(T entity)
        {
            await _collection.DeleteOneAsync(e => e.Id == entity.Id);
            return entity;
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _collection.FindOneAndDeleteAsync(e => e.Id == entity.Id);
            }
        }

        /// <summary>
        /// Async Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await DeleteAsync(entity);
            }
            return entities;
        }

       


        public T Create(T entity)
        {
            _collection.InsertOne(entity);
            return entity;
        }

        public T Get(string id)
        {
           return _collection.Find<T>(product => product.Id == id).FirstOrDefault();
        }

        public void Update(string id, T product)
        {
            _collection.ReplaceOne(product => product.Id == id, product);
        }

        public void Remove(string id)
        {
            _collection.DeleteOne(product => product.Id == id);
        }

        Result<T>  IMongoRepository<T>.GetAsync(int page, int pageSize)
        {
            var result = new Result<T>();
            result.Page = page;
            result.Quantity = pageSize;

            result.Data = _collection.Find(_ => true)
                .Skip((page - 1) * pageSize).Limit(pageSize).ToList();
            if (result.Data.Count > 0)
            {
                result.Total = _collection.CountDocuments(_ => true);
                result.TotalPages = result.Total / pageSize;

                return result;
            }
            return default;
        }

        public T? Get(long code)
        {
            var products = _collection.Find(_ => true).FirstOrDefault();
            return products == null ? default(T) : products;
        }

        public async Task<T> GetAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(entity => entity.Id, id);

            var product = await _collection.Find(filter).FirstOrDefaultAsync();
            if(product != null)
            {
                return product;
            }
            return default;
        }

        public async Task<Result<T>> GetAllAsync(int page, int pageSize)
        {
            var result = new Result<T>();
            result.Page = page;
            result.Quantity = pageSize;

            result.Data = await _collection.Find(_ => true)
                .Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();

            result.Total = _collection.CountDocuments(_ => true);
            result.TotalPages = result.Total / pageSize;

            return result;
        }

        public List<T> CreateMany(List<T> entities)
        {
            _collection.InsertManyAsync(entities);
            return entities;
        }

        public bool GetByCodeExistsAsync(long code)
        {

            var product = _collection.Find(_=>true).ToList();
           
            if (product != null && product.Where(i => i.Code == code).Count()>0)
            {
                return true;
            }
            return false;

        }

        public T? GetByCode(long code)
        {
            var products = _collection.Find(_=>true).ToList().Where(i => i.Code == code).FirstOrDefault();
            if (products is null)
            {
                return products;
            }
            return null;
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IMongoQueryable<T> Table
        {
            get { return _collection.AsQueryable(); }
        }
    }
}