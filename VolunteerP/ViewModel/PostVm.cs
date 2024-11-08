﻿using Microsoft.Win32;
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
        public ICommand CommentsCommand {  get; private set; }

        private ObservableCollection<Post> _filteredPosts;
        private string _searchText;


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
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            private set
            {
                _posts = value;
                OnPropertyChanged(nameof(Posts));
            }
        }

        public PostVm(PostService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
            Posts = new ObservableCollection<Post>();
            FilteredPosts = new ObservableCollection<Post>();
            InitializePostsAsync();
            ContactCommand = new RelayCommand(DisplayContactInfo);
            AddPhotoCommand = new RelayCommand(AddPhotoExecute);
            CommentsCommand = new RelayCommand(OpenCommentsDialog);
            NewPost = new Post();
            _postService = postService;
            EventSystem.OnPostUpdated += HandlePostUpdated;
            LoadPosts();
        }

        private void HandlePostUpdated(Post updatedPost)
        {
            // Handle the updated post information
            InitializePostsAsync(); // Reload all posts, or optimize by updating only affected post
        }

        public void Cleanup()
        {
            EventSystem.OnPostUpdated -= HandlePostUpdated; // Unsubscribe from the event
        }

        private ObservableCollection<Post> _posts;

        public ObservableCollection<Post> FilteredPosts
        {
            get => _filteredPosts;
            set
            {
                _filteredPosts = value;
                OnPropertyChanged(nameof(FilteredPosts));
            }
        }


        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));  // This should trigger property change notification
                    FilterPosts(_searchText);  // This should filter the posts based on the new search text
                }
            }
        }

        private void OpenCommentsDialog(object parameter)
        {
            if (parameter is Post selectedPost)
            {
                var dialog = new CommentDialog(selectedPost);

                if (dialog.ShowDialog() == true)
                {
                    // Logic to handle comment addition if needed
                    // Example:
                    // Check if new comment was not empty and then add to the post comments
                    if (!string.IsNullOrWhiteSpace(dialog.NewCommentText))
                    {
                        Comment newComment = new Comment
                        {
                            CommentText = dialog.NewCommentText,
                            IsAnonymous = dialog.IsAnonymous
                            // Set other necessary properties
                        };
                        selectedPost.Comments.Add(newComment);
                        _postservice.AddCommentToPost(selectedPost.Id, newComment);
                        // Make sure to implement AddCommentToPost in your PostService
                    }
                }
            }
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

        private async void InitializePostsAsync()
        {
            try
            {
                var postsFromDb = await _postService.GetAllPostsAsync();
                Posts = new ObservableCollection<Post>(postsFromDb);
                FilteredPosts = new ObservableCollection<Post>(postsFromDb.Where(p => p.IsVisible));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load posts: " + ex.Message);
            }
        }

        private void FilterPosts(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredPosts = new ObservableCollection<Post>(Posts);
            }
            else
            {
                var filtered = Posts.Where(post =>
                    (post.UserPost?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (post.PostName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
                FilteredPosts = new ObservableCollection<Post>(filtered);
            }
        }



        private async void LoadPosts()
        {
            try
            {
                var postsFromDb = await _postService.GetAllPostsAsync();
                var visiblePosts = postsFromDb.Where(p => p.IsVisible).ToList();
                Posts = new ObservableCollection<Post>(visiblePosts);
                FilteredPosts = new ObservableCollection<Post>(visiblePosts);
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
