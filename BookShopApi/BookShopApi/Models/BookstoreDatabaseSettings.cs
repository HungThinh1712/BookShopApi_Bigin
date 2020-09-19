using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Models
{
    public class BookstoreDatabaseSettings : IBookstoreDatabaseSettings
    {
        public string BooksCollectionName { get; set; }
        public string CategoriesCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string ShoppingCartsCollectionName { get; set; }
        public string PurchaseHistoriesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBookstoreDatabaseSettings
    {
        string BooksCollectionName { get; set; }
        string CategoriesCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string ShoppingCartsCollectionName { get; set; }
        string PurchaseHistoriesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
