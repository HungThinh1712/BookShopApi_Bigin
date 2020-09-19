using BookShopApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public class PurchaseHistoryService
    {
        private readonly IMongoCollection<PurchaseHistory> _purchasehistories;

        public PurchaseHistoryService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _purchasehistories = database.GetCollection<PurchaseHistory>(settings.CategoriesCollectionName);
        }

        public async Task<List<PurchaseHistory>> GetAsync() =>
           await _purchasehistories.Find(PurchaseHistory => true).ToListAsync();
        public async Task<PurchaseHistory> GetAsync(string id) =>
           await _purchasehistories.Find<PurchaseHistory>(ph =>ph.Id == id).FirstOrDefaultAsync();

        public async Task<PurchaseHistory> CreateAsync(PurchaseHistory ph)
        {
            await _purchasehistories.InsertOneAsync(ph);
            return ph;
        }

        public async Task UpdateAsync(string id, PurchaseHistory phIn) =>
           await _purchasehistories.ReplaceOneAsync(ph => ph.Id == id, phIn);

        public async Task RemoveAsync(string id) =>
            await _purchasehistories.DeleteOneAsync(ph => ph.Id == id);
    }
}
