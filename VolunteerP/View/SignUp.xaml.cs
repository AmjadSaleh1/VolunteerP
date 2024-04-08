using System.Windows;
using System.Windows.Input;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private UserService _userService;
        public SignUp()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            try
            {
                var dbContext = new MongoDbContext(); // This assumes MongoDbContext is properly set up to use the config file
                _userService = new UserService(dbContext.Database);  // Initialize _userService here
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to database: " + ex.Message);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Example: Collecting data from user input and using _userService to interact with the database
            try
            {
                // Assume there are text fields for User data collection
                User newUser = new User
                {
                    Name = nameTextBox.textbox.Text,  // Replace these with actual data collection from your form
                    Email = emailTextBox.textbox.Text
                    // Add other fields as needed
                };

                await _userService.AddUserAsync(newUser);  // Use _userService to add the user
                MessageBox.Show("User registered successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to register user: {ex.Message}");
            }
        }


    }
}
