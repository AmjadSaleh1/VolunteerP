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
            if (_postservice == null)
            {
                MessageBox.Show("Post service is not initialized.");
                return;  // Stop further execution to prevent the null reference exception.
            }

            var newPost = new Post
            {
                UserPost = postTextBox.Text,
                PostName = "YourUserName",  // Ensure this is correctly assigned and meaningful.
                PostTime = DateTime.Now,
                ImagePath = "PathOrURLToImage"  // Validate this path or handle it dynamically.
            };

            try
            {
                await _postservice.AddPostAsync(newPost);
                MessageBox.Show("Post added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
