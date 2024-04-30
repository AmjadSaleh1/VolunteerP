using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Models;
using BCrypt.Net;
using MongoDB.Driver.Core.Configuration;



namespace VolunteerP.ServerApi.Services
{ 

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(IMongoDatabase database)
    {
        _usersCollection = database.GetCollection<User>("users");
    }

   

        public async Task AddUserAsync(User user)
    {
        await _usersCollection.InsertOneAsync(user);
    }

    public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _usersCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }
        

        public async Task<bool> ValidateUserLogin(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                if (user.IsLocked)
                {
                    throw new Exception("This account has been locked. Please try again later.");
                }

                else
                {
                    return true;
                }
            }
            return false;  // No user found or password does not match
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _usersCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
            return user != null;
        }

      

        public async Task<string> GetUserGenderByEmail(string email)
        {
            var user = await GetUserByEmailAsync(email);
            if (user != null)
            {
                return user.Gender ?? "Unknown"; // Return gender or "Unknown" if null
            }
            return "Unknown"; // Return "Unknown" if no user is found
        }


        public async Task<int> GetUserCountAsync()
        {
            return (int)await _usersCollection.CountDocumentsAsync(new BsonDocument());
        }

        public async Task<List<User>> GetUsersAsync()
        {
            // This line fetches all documents from the 'users' collection and converts them to a List of User objects.
            return await _usersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update.Set(u => u.IsLocked, user.IsLocked);
            await _usersCollection.UpdateOneAsync(filter, update);

            try
            {
                await _usersCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., logging
                Console.WriteLine("Error updating user: " + ex.Message);
                throw;
            }
        }


        public async Task UpdateUserinfo(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update
                .Set(u => u.Name, user.Name)
                .Set(u => u.Email, user.Email)
                .Set(u => u.PhoneNumber, user.PhoneNumber)
                .Set(u => u.UserName, user.UserName)
                // Add other fields as necessary
                ;
            await _usersCollection.UpdateOneAsync(filter, update);
        }


        public async Task UpdateUserProfileImage(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update.Set(u => u.ImageUrl, user.ImageUrl);
            await _usersCollection.UpdateOneAsync(filter, update);
        }


    }
}