using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Services;

namespace VolunteerP.Utilities
{
    public static class ServiceLocator
    {
        private static IMongoDatabase _database;
        private static PostService _postService;

        static ServiceLocator()
        {
            // Initialize the MongoDB client and get the database.
            string connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("Volunteer");
        }

        public static PostService PostService => _postService ?? (_postService = new PostService(_database));
    }
}
