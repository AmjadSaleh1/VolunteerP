using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;
using VolunteerP.View;

namespace VolunteerP.ViewModel
{
    public class UserProfileViewModel : ViewModelBase
    {

        private readonly UserService _userService;
        private PostService _postService;
        private User _user;
        private object _currentView;
        public User User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }


        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand PostCommand { get; set; }
        public ICommand UserPostCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public UserProfileViewModel()
        {
            _postService = ServiceLocator.PostService;
            var mongoDBContext = new MongoDbContext();
            _userService = new UserService(mongoDBContext.Database);
            PostCommand = new RelayCommand(Post);
            UserPostCommand = new RelayCommand(UserPost);
            ProfileCommand = new RelayCommand(Profile);
            CurrentView = new PostVm(_postService);
            


        }

       

        public async Task InitializeAsync(string userEmail)
        {
            await LoadUserData(userEmail);
        }

        private async Task LoadUserData(string userEmail)
        {
            try
            {
                User = await _userService.GetUserByEmailAsync(userEmail);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load user data: {ex.Message}");
            }
        }

       

        private void Post(object obj)
        {
            CurrentView = new PostVm(_postService);
            OnPropertyChanged(nameof(CurrentView));
        }

        private void UserPost(object obj)
        {
            try
            {
                CurrentView = new UserPostsVm();
                OnPropertyChanged(nameof(CurrentView));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to load posts: {ex.Message}");
            }
        }

        private void Profile(object obj)
        {
            CurrentView = new ProfileVm(User);
        }

        public async void UpdateUserProfileImage()
        {
            // Assuming _userService is available in the ViewModel
            await _userService.UpdateUserProfileImage(User);
        }

    }
}
