using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Models;

namespace VolunteerP.ServerApi.Services
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _postCollection;
        public PostService(IMongoDatabase database) 
        {
            _postCollection = database.GetCollection<Post>("Posts");
        }



        public async Task AddPostAsync(Post post)
        {
            await _postCollection.InsertOneAsync(post);
        }
    }
}
