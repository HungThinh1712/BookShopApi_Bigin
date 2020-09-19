using BookShopApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public class ShoppingCartService
    {

        private readonly IMongoCollection<ShoppingCart> _shoppingcarts;

        public ShoppingCartService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _shoppingcarts = database.GetCollection<ShoppingCart>(settings.ShoppingCartsCollectionName);
        }
        public async Task<ShoppingCart> GetAsync(string userid) =>
            await _shoppingcarts.Find<ShoppingCart>(shoppingcart => shoppingcart.UserId == userid).FirstOrDefaultAsync();
        public async Task<ShoppingCart> CreateAsync(ShoppingCart shoppingcart)
        {
            await _shoppingcarts.InsertOneAsync(shoppingcart);
            return shoppingcart;
        }

        public async Task UpdateAsync(string id, ShoppingCart shoppingcartIn) =>
           await _shoppingcarts.ReplaceOneAsync(shoppingcart => shoppingcart.Id == id, shoppingcartIn);
    }
}
