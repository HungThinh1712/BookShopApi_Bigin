using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace BookShopApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryID { get; set; }

        [BsonIgnore]
        public Category Category { get; set; }
        public string Author { get; set; }
        public int Amount { get; set; } 
        public int Code { get; set; }
        public bool isDeleted { get; set; }

    }
}