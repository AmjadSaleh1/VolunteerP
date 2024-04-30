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
using System.Windows;


namespace VolunteerP.ViewModel
{
    public class AdminViewModel : ViewModelBase
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));  // Assuming OnPropertyChanged implements INotifyPropertyChanged
            }
        }

        private ObservableCollection<Post> _posts;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged(nameof(Posts));  // Same assumption as above
            }
        }


        public ICommand LockCommand { get; private set; }
        public ICommand HideCommand { get; private set; }

        private readonly UserService _userService;  // Service to manage user actions
        private readonly PostService _postService;  // Service to manage post actions

        public AdminViewModel(UserService userService, PostService postService)
        {
            _userService = userService;
            _postService = postService;

            LoadUsers();
            LoadPosts();

            LockCommand = new RelayCommand(LockUser);
            HideCommand = new RelayCommand(HidePost);
        }

        private async void LoadUsers()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                if (users != null && users.Any())
                {
                    Users = new ObservableCollection<User>(users);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load users: " + ex.Message);
            }
        }

       

        private async void LockUser(object parameter)
        {
            var user = parameter as User;
            if (user != null)
            {
                user.IsLocked = !user.IsLocked; // Toggle lock state
                await _userService.UpdateUser(user); // Update in database

                // Optionally show a message indicating the action's success
                MessageBox.Show($"User {(user.IsLocked ? "locked" : "unlocked")}.", "Info");
            }
        }

        private async Task LoadPosts()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();
                if (posts != null)
                {
                    Posts = new ObservableCollection<Post>(posts.Where(p => p.IsVisible)); // Only show visible posts
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load posts: " + ex.Message);
            }
        }

        private async void HidePost(object parameter)
        {
            var post = parameter as Post;
            if (post != null)
            {
                post.IsVisible = !post.IsVisible; // Toggle visibility
                await _postService.UpdatePost(post); // Update in database

                // Optionally show a message indicating the action's success
                MessageBox.Show($"Post {(post.IsVisible ? "visible" : "hidden")}.", "Info");

                // Refresh the list of posts
                await LoadPosts();
            }
        }



    }
}
