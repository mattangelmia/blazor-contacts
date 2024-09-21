using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Threading.Tasks;
using ContactsApi.Controllers;
using ContactsApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BlazorContactsApp.Services
{
    public class ContactService
    {
        private readonly IMongoCollection<Contact> _contactsCollection;
        private readonly IMongoDatabase _database;

        private readonly HttpClient _httpClient;

        public ContactService(IConfiguration configuration, HttpClient httpClient)
        {
            var connectionString = configuration
                .GetSection("ContactDatabase:ConnectionString")
                .Value;
            var mongoUrl = new MongoUrl(connectionString);
            var client = new MongoClient(mongoUrl);
            _database = client.GetDatabase(mongoUrl.DatabaseName);
            _httpClient = httpClient;
        }

        public IMongoDatabase Database => _database;

        // public async Task<List<Contact>> GetContactsAsync()
        // {
        //     return await _contactsCollection.Find(contact => true).ToListAsync();
        // }

        public async Task<List<Contact>> GetContactsAsync()
        {
            var contacts = await _httpClient.GetFromJsonAsync<List<Contact>>(
                "http://localhost:5046/api/contacts"
            );
            return contacts ?? new List<Contact>();
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            await _httpClient.PostAsJsonAsync("http://localhost:5046/api/contacts", contact);
            return contact;
        }

        public async Task DeleteContactAsync(string id)
        {
            await _httpClient.DeleteAsync($"http://localhost:5046/api/contacts/{id}");
        }

        public async Task TestMongoDbConnectionAsync()
        {
            try
            {
                // Attempt to list collections to verify the connection
                var collections = await _database.ListCollectionsAsync();
                await collections.MoveNextAsync();
                Console.WriteLine("MongoDB connection successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MongoDB connection failed: {ex.Message}");
            }
        }
    }
}
