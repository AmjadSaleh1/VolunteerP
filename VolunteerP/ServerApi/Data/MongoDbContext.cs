using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Configuration;

namespace VolunteerP.ServerApi.Data
{
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; private set; }

        public MongoDbContext()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
                string dbName = "Volunteer";  // This should also come from a config file
                var client = new MongoClient(connectionString);
                Database = client.GetDatabase(dbName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to MongoDB: " + ex.Message);
                throw;  // Rethrow if you cannot recover from this error
            }
        }

        // Test method to check the connection and fetch data, returns a string message
        public string TestDatabaseConnection()
        {
            try
            {
                var collection = Database.GetCollection<BsonDocument>("testCollection");
                var documentCount = collection.Find(new BsonDocument()).Limit(1).CountDocuments();
                return $"Connection successful, document count in 'testCollection': {documentCount}";
            }
            catch (Exception ex)
            {
                return "Failed to connect to MongoDB or retrieve data: " + ex.Message;
            }
        }
    }
}
