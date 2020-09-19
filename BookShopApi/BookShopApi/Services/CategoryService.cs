
using BookShopApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _categories = database.GetCollection<Category>(settings.CategoriesCollectionName);
        }

        public async Task<List<Category>> GetAsync() =>
           await _categories.Find(category => category.isDeleted==false).ToListAsync();
        public async Task<Category> GetAsync(string id) =>
           await _categories.Find<Category>(category => category.Id == id).FirstOrDefaultAsync();

        public async Task<Category> CreateAsync(Category category)
        {
            await _categories.InsertOneAsync(category);
            return category;
        }

        public async Task UpdateAsync(string id, Category categoryIn) =>
           await _categories.ReplaceOneAsync(category => category.Id == id, categoryIn);

        public async Task RemoveAsync(string id) =>
            await _categories.DeleteOneAsync(category => category.Id == id);
    }
}