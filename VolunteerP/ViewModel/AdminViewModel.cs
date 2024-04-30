using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;
using VolunteerP.ServerApi.Data;


namespace VolunteerP.ViewModel
{
    public class AdminViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Post> Posts { get; set; }

        public ICommand LockCommand { get; private set; }
        public ICommand HideCommand { get; private set; }

        private readonly UserService _userService;  // Service to manage user actions
        private readonly PostService _postService;  // Service to manage post actions

        public AdminViewModel(UserService userService, PostService postService)
        {
            _userService = userService;
            _postService = postService;

            Users = new ObservableCollection<User>();  // Load users from database
            Posts = new ObservableCollection<Post>();  // Load posts from database

            LoadUsers();
            LoadPosts();

            LockCommand = new RelayCommand(LockUser);
            HideCommand = new RelayCommand(HidePost);
        }

        private void LoadUsers()
        {
            // Assume GetUsersAsync returns a List<User>
            var users = _userService.GetUsersAsync().Result;  // Replace with proper async handling
            Users = new ObservableCollection<User>(users);
        }

        private void LoadPosts()
        {
            // Assume GetPostsAsync returns a List<Post>
            var posts = _postService.GetAllPostsAsync().Result;  // Replace with proper async handling
            Posts = new ObservableCollection<Post>(posts);
        }

        private async void LockUser(object userObject)
        {
            var user = userObject as User;
            if (user != null)
            {
                user.IsLocked = !user.IsLocked;  // Toggle lock status
                await _userService.UpdateUser(user);
            }
        }

        private async void HidePost(object postObject)
        {
            var post = postObject as Post;
            if (post != null)
            {
                post.IsVisible = !post.IsVisible;  // Toggle visibility
                await _postService.UpdatePost(post);
            }
        }

    }
}
