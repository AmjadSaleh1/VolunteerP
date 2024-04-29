using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;

namespace VolunteerP.ViewModel
{
    public class PostVm : ViewModelBase
    {
        private PostService _postService;
        public ICommand ContactCommand { get; private set; }
        public ICommand AddPhotoCommand { get; private set; }

        private Post _newPost;
        public Post NewPost
        {
            get => _newPost;
            set
            {
                _newPost = value;
                OnPropertyChanged(nameof(NewPost));
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail;
            set
            {
                _userEmail = value;
                OnPropertyChanged(nameof(UserEmail));
            }
        }

        private string _userPhone;
        public string UserPhone
        {
            get => _userPhone;
            set
            {
                _userPhone = value;
                OnPropertyChanged(nameof(UserPhone));
            }
        }

        private PostService _postservice;
        public ObservableCollection<Post> Posts { get; private set; }

        public PostVm(PostService postService)
        {
            _postservice = postService ?? throw new ArgumentNullException(nameof(postService));
            Posts = new ObservableCollection<Post>();
            InitializePostsAsync();
            ContactCommand = new RelayCommand(DisplayContactInfo);
            AddPhotoCommand = new RelayCommand(AddPhotoExecute);
            NewPost = new Post();
            _postService = postService;
        }



        private async void AddPhotoExecute(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp",
                Title = "Select a photo"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;  // Update the ViewModel's ImagePath
                MessageBox.Show("Image path set to: " + ImagePath);  // Debugging message
            }
        }

        private void DisplayContactInfo(object parameter)
        {
            var post = parameter as Post;
            if (post != null)
            {
                string message = $"Name: {post.PostName}\nEmail: {post.UserEmail}\nPhone: {post.UserPhone}";
                MessageBox.Show(message, "Contact Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
