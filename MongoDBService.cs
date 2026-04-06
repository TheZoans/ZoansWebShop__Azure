using MongoDB.Driver;
using Zoans.Models;

namespace Zoans.Services
{
    public class MongoDbService
    {
        private IMongoDatabase database;

        public MongoDbService(IConfiguration config)
        {
            string connectionString = config["MongoDB:ConnectionString"];
            string databaseName = config["MongoDB:DatabaseName"];

            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Items> GetItems()
        {
            return database.GetCollection<Items>("items");
        }

        public IMongoCollection<User> GetUsers()
        {
            return database.GetCollection<User>("users");
        }
    }
}