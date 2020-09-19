
using BookShopApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync() =>
            await _books.Find(book => book.isDeleted ==false && book.Amount > 0).ToListAsync();

        public async Task<Book> GetAsync(string id) =>
           await _books.Find<Book>(book => book.Id == id).FirstOrDefaultAsync();
        public async Task<Book> GetAsync(int code) =>
          await _books.Find<Book>(book => book.Code == code).FirstOrDefaultAsync();
        public async Task<Book> CreateAsync(Book book)
        {
            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task UpdateAsync(string id, Book bookIn) =>
           await _books.ReplaceOneAsync(book => book.Id == id, bookIn);


        public async Task RemoveAsync(string id) =>
           await _books.DeleteOneAsync(book => book.Id == id);

        public async Task<List<Book>> SearchAsync(string searchstring) =>
            await _books.Find(book => book.isDeleted == false && book.Name.Contains(searchstring)).ToListAsync();

    }
}