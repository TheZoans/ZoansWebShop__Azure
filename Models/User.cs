using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Zoans.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "user";

        
        public Dictionary<string, int> RatingsGiven { get; set; } = new Dictionary<string, int>();
    }
}
