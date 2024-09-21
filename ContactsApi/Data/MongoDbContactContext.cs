using ContactsApi.Models;
using MongoDB.Driver;

namespace ContactsApi.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            
            var client = new MongoClient("mongodb+srv://23matthewangel:Cobeykiki12345@backend-new.ruace.mongodb.net/phoneApp?retryWrites=true&w=majority&appName=backend-new");
            _database = client.GetDatabase("phoneApp");
        }

        public IMongoCollection<Contact> Contacts => _database.GetCollection<Contact>("contacts");
    }
}