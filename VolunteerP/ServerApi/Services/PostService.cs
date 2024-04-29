using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Bson;
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

        public async Task<List<Post>> GetAllPostsAsync()
        {
            try
            {
                return await _postCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception here to diagnose issues with the database query
                throw new Exception("Error retrieving posts: " + ex.Message, ex);
            }
        }

      public async Task<List<Post>> GetPostsByUserEmailAsync(string email)
        {
            return await _postCollection.Find(post => post.UserEmail == email).ToListAsync();
        }

        public async Task UpdatePostAsync(ObjectId postId, string newContent)
        {
            var filter = Builders<Post>.Filter.Eq(post => post.Id, postId);
            var update = Builders<Post>.Update.Set(post => post.UserPost, newContent);
            await _postCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeletePostAsync(ObjectId postId)
        {
            await _postCollection.DeleteOneAsync(post => post.Id == postId);
        }

        public async Task<bool> UpdatePostImagePath(ObjectId postId, string imagePath)
        {
            var filter = Builders<Post>.Filter.Eq(post => post.Id, postId);
            var update = Builders<Post>.Update.Set(post => post.ImagePath, imagePath);
            var result = await _postCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> AddCommentToPost(ObjectId postId, Comment comment)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
            var update = Builders<Post>.Update.Push(p => p.Comments, comment);

            try
            {
                var result = await _postCollection.UpdateOneAsync(filter, update);
                return result.ModifiedCount == 1;
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                // For example, log the exception and return false or rethrow it
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
