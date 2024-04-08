using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Models;

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
}