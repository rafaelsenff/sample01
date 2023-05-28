using BaseMongoDb.Models;

namespace BaseMongoDb.Services.Interfaces
{
    public interface IMongoService<T> where T : BaseMongoEntity
    {
        Task<List<T>> GetAsync();

        Task<T?> GetAsync(string id);

        Task CreateAsync(T newEntity);

        Task UpdateAsync(string id, T newEntity);

        Task RemoveAsync(string id);
    }
}
