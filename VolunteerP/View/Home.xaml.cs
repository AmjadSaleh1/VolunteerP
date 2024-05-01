using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;
using VolunteerP.ViewModel;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {

        private PostService _postservice;


        public Home()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            var dbContext = new MongoDbContext();
            var postService = new PostService(dbContext.Database);
            this.DataContext = new PostVm(postService);
        }


        private void InitializeDatabaseConnection()
        {
            try
            {
                var dbContext = new MongoDbContext(); // This assumes MongoDbContext is properly set up to use the config file
                _postservice = new PostService(dbContext.Database);  // Initialize _userService here
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to database: " + ex.Message);
            }
        }

        private async void Post_Click(object sender, RoutedEventArgs e)
        {
            // Check for empty post content first.
            if (string.IsNullOrWhiteSpace(postTextBox.Text))
            {
                MessageBox.Show("Post cannot be empty");
                return;
            }

            // Initialize ViewModel from DataContext
            if (!(this.DataContext is PostVm viewModel))
            {
                MessageBox.Show("Data context is not set properly.");
                return;
            }

            // Create the new post object.
            var newPost = new Post
            {
                UserPost = postTextBox.Text,
                PostName = UserHelper.CurrentUser.Name,
                PostTime = DateTime.Now,
                ImagePath = viewModel.ImagePath, // Temporarily set image path
                UserEmail = UserHelper.CurrentUser.Email,
                UserPhone = UserHelper.CurrentUser.PhoneNumber,
                IsVisible = true
                
            };

            try
            {
                // Add the post to the database. Make sure this sets the Id of newPost.
                await _postservice.AddPostAsync(newPost);

                // Update the image path in the database.


                // Add the post to the ObservableCollection bound to the ListView.
                viewModel.Posts.Add(newPost);

                // Clear the text box after the post has been successfully added.
                postTextBox.Clear();

                MessageBox.Show("Post added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void PostAnonymously_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(postTextBox.Text))
            {
                MessageBox.Show("Post cannot be empty");
                return;
            }

            if (!(this.DataContext is PostVm viewModel))
            {
                MessageBox.Show("Data context is not set properly.");
                return;
            }

            var newPost = new Post
            {
                UserPost = postTextBox.Text,
                PostName = UserHelper.CurrentUser.UserName, // Use a generic name
                PostTime = DateTime.Now,
                UserEmail = UserHelper.CurrentUser.Email,
                ImagePath = viewModel.ImagePath, // Can still include an image if desired
                IsAnonymous = true, // This should be a new boolean property in your Post model
                IsVisible=true
            };

            try
            {
                await _postservice.AddPostAsync(newPost);
                viewModel.Posts.Add(newPost);
                postTextBox.Clear();
                MessageBox.Show("Anonymous post added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }



        private void PostTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "whats in ur mind")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(Colors.Black); // Set text color to black (or any other color for user input)
            }
        }
    }
}
