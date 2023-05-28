namespace BaseMongoDb.Services
{
    using BaseMongoDb.Models;
    using BaseMongoDb.Services.Interfaces;
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoService<T>
        : IMongoService<T>
        where T : BaseMongoEntity
    {
        private readonly IMongoCollection<T> _baseCollection;

        public MongoService(
            IOptions<MongoDbDatabaseSettings> entityDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                entityDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                entityDatabaseSettings.Value.DatabaseName);

            _baseCollection = mongoDatabase.GetCollection<T>(
                typeof(T).Name);
        }

        public async Task<List<T>> GetAsync() =>
            await _baseCollection.Find(_ => true).ToListAsync();

        public async Task<T?> GetAsync(string id) =>
            await _baseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(T newEntity)
        {
            newEntity.Id = ObjectId.GenerateNewId().ToString();
            newEntity.Created = DateTime.Now;
            newEntity.Updated = null;
            await _baseCollection.InsertOneAsync(newEntity);
        }

        public async Task UpdateAsync(string id, T newEntity)
        {
            newEntity.Updated = DateTime.Now;
            await _baseCollection.ReplaceOneAsync(x => x.Id == id, newEntity);
        }

        public async Task RemoveAsync(string id) =>
            await _baseCollection.DeleteOneAsync(x => x.Id == id);
    }
}
