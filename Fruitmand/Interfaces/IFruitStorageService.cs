using Fruitmand.Models;

namespace Fruitmand.Interfaces
{
    public interface IFruitStorageService
    {
        Task AddFruitAsync(string soortFruit, FruitItem item);
        Task<FruitItem?> GetOldestFruitAsync(string soortFruit);
        Task DeleteOldestFruitAsync(string soortFruit);
        Task<List<FruitItem>> GetAllFruitsAsync();
        Task DeleteAllFruitsAsync();
    }
}
