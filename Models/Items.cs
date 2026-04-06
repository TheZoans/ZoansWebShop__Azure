using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Globalization;
namespace Zoans.Models
{
    public class Items
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal  Price { get; set; }
        public string Seller { get; set; } = "";
        public string ImageURL { get; set; } = "";
        public string Category { get; set; } = "";
        public string Condition { get; set; } = "New";

        public string? BatteryLife { get; set; }
        public int? Age { get; set; }
        public string? Size { get; set; }
        public string? Material { get; set; }

        public double AverageRating { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public double RatingSum { get; set; } = 0;

        public Dictionary<string, int> UserRatings { get; set; } = new();

        public List<Review> Reviews { get; set; } = new();

    }

    public class Review
    {
        public string UserId { get; set; } = "";
        public string Username { get; set; } = "";
        public string Content { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
