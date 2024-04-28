using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;

namespace VolunteerP.ViewModel
{
    public class UserPostsVm : ViewModelBase
    {

        private ObservableCollection<Post> _posts;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged(nameof(Posts));
            }
        }

        public ICommand EditPostCommand { get; private set; }
        public ICommand DeletePostCommand { get; private set; }

        public readonly PostService _postService;

        public UserPostsVm()
        {

            _postService = ServiceLocator.PostService; // Make sure this service is correctly initialized with a database context
            InitializeCommands();
            if (UserHelper.CurrentUser != null)
            {
                LoadUserPosts(UserHelper.CurrentUser.Email);
            }
            else
            {
                MessageBox.Show("Current user is not set.");
            }

        }

        private void InitializeCommands()
        {
            EditPostCommand = new RelayCommand(EditPost);
            DeletePostCommand = new RelayCommand(DeletePost);
        }

        private void EditPost(object postObject)
        {
            var post = postObject as Post;
            if (post != null)
            {
                EditPostDialog dialog = new EditPostDialog(post, _postService)
                {
                    Owner = Application.Current.MainWindow, // Set the owner window for centering
                    PostContent = post.UserPost, // Pass the current content to the dialog
                    DataContext = post
                };

                if (dialog.ShowDialog() == true) // Show the dialog as modal
                {
                    post.UserPost = dialog.PostContent; // Update the post content with the dialog's result
                                                        // Call the service to update the database
                    _postService.UpdatePostAsync(post.Id, post.UserPost);
                }
            }
        }

        private async void DeletePost(object postObject)
        {
            var post = postObject as Post;
            if (post == null) return;
            await _postService.DeletePostAsync(post.Id);
            Posts.Remove(post);
        }

        public async void LoadUserPosts(string userEmail)
        {
            try
            {
                var posts = await _postService.GetPostsByUserEmailAsync(userEmail);
                if (posts != null)
                {
                    Posts = new ObservableCollection<Post>(posts);
                    Debug.WriteLine($"Loaded {Posts.Count} posts for {userEmail}");
                }
                else
                {
                    Posts = new ObservableCollection<Post>(); // Initialize with an empty collection if null is returned.
                    Debug.WriteLine("No posts found for the given user email.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load posts: {ex.Message}");
                Debug.WriteLine($"Failed to load posts: {ex}");
            }
        }

    }
}
