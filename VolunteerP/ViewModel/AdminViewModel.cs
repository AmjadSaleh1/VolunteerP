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
            Task.Run(() => LoadPostsForAdmin());

            LockCommand = new RelayCommand(LockUser);
            HideCommand = new RelayCommand(HidePost);
        }

        public async Task InitializeAsync()
        {
            await LoadPostsForAdmin();
            // Any additional setup can go here
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


        private async Task LoadPostsForAdmin()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();  // Fetches all posts
                Posts = new ObservableCollection<Post>(posts);  // Sets to display all posts in admin view
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
                // Toggle the visibility
                post.IsVisible = !post.IsVisible;

                // Update the post in the database
                await _postService.UpdatePost(post);

                // Show a message box indicating the new visibility status
                MessageBox.Show(post.IsVisible ? "The post is now visible." : "The post is now hidden.", "Visibility Changed");

                // Optionally refresh the posts list if it's displayed elsewhere
                await LoadPostsForAdmin();
            }
        }



    }
}
