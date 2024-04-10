using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Models;
using BCrypt.Net;



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
                return true;  // User found and password matches
            }
            return false;  // No user found or password does not match
        }



    }
}