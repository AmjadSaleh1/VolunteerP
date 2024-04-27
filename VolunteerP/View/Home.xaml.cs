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
            LoadPosts();
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

            // Proceed if the post content is not empty.
            var newPost = new Post
            {
                UserPost = postTextBox.Text,
                PostName = "YourUserName", // This should be retrieved from the logged-in user's data.
                PostTime = DateTime.Now,
                ImagePath = "PathOrURLToImage", // This should be set after the user uploads an image.
                UserEmail = "THE USER EMAIL" // This should be the email of the logged-in user.
            };

            try
            {
                // Add the post to the database.
                await _postservice.AddPostAsync(newPost);

                // Add the post to the ObservableCollection bound to the ListView.
                ((PostVm)this.DataContext).Posts.Add(newPost);

                // Clear the text box after the post has been successfully added.
                postTextBox.Clear();

                MessageBox.Show("Post added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        public async void LoadPosts()
        {
            try
            {
                var posts = await _postservice.GetAllPostsAsync();
                feedListView.ItemsSource = posts; // Correctly set ItemsSource
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load posts: " + ex.Message);
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
