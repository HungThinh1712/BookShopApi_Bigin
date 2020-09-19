using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookShopApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool isDeleted { get; set; }
        public bool isAdmin { get; set; } 
    }
}
