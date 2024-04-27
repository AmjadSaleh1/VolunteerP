using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;

namespace VolunteerP.ViewModel
{
    public class PostVm : ViewModelBase
    {
        private PostService _postservice;
        public ObservableCollection<Post> Posts { get; private set; }

        public PostVm(PostService postService)
        {
            _postservice = postService ?? throw new ArgumentNullException(nameof(postService));
            Posts = new ObservableCollection<Post>();
            InitializePostsAsync();
        }

        private async Task InitializePostsAsync()
        {
            try
            {
                var postsFromDb = await _postservice.GetAllPostsAsync();
                foreach (var post in postsFromDb)
                {
                    Posts.Add(post);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load posts: " + ex.Message);
            }
        }


        // Method to add a new post to the collection and the database
        public async Task AddPost(Post newPost)
        {
            if (newPost == null)
                throw new ArgumentNullException(nameof(newPost));

            try
            {
                await _postservice.AddPostAsync(newPost);
                Posts.Add(newPost);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add post: {ex.Message}");
            }
        }
    }
}
