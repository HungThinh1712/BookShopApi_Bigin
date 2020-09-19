using BookShopApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync() =>
           await _users.Find(user => user.isDeleted == false).ToListAsync();
        public async Task<User> GetAsync(string id) =>
           await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();

        public async Task<User> GetUserAsync(string username) =>
         await _users.Find<User>(user => user.UserName == username).FirstOrDefaultAsync();

        public async Task<User> GetUserAsync(string username, string password) =>
         await _users.Find<User>(user => user.UserName == username && user.PassWord == password &&user.isDeleted==false).FirstOrDefaultAsync();

        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateAsync(string id, User userIn) =>
           await _users.ReplaceOneAsync(user => user.Id == id, userIn);

        public async Task RemoveAsync(string id) =>
            await _users.DeleteOneAsync(user => user.Id == id);

       
    }
}
