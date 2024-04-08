using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;
using VolunteerP.Utilities;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private UserService _userService;
        public int SelectedGender { get; set; }
        private string genderstring;
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
                    Password = txtPassword.Password,
                    Email = emailTextBox.textbox.Text,
                    DateOfBirth = Dob.textbox.Text,
                    PhoneNumber = Phone.textbox.Text,
                    Location = location.textbox.Text,
                    Gender = genderstring
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

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ContainsDigits(nameTextBox.textbox.Text))
            {
                nameErrorTextBlock.Text = "Name cannot contain numbers.";
            }
            else
            {
                nameErrorTextBlock.Text = "";  // Clear the error message when the input is valid
            }
        }

        private bool ContainsDigits(string input)
        {
            return input.Any(char.IsDigit);
        }

        private void GenderOption_Click(object sender, RoutedEventArgs e)
        {
            
            // This method assumes that the IsSelected property is toggled within each UserOption's own logic
            UserOption selectedOption = sender as UserOption;
            if (selectedOption != null)
            {
                // Set the gender based on the option text
                genderstring = selectedOption.Text == "Male" ? "Male" : "Female";
                int gender = selectedOption.Text == "Male" ? 1 : 0;
                
                // Assuming you have a User model or similar to update
                // UpdateUserGender(gender);  // Implement this method to handle your user data update

                // Optionally, you can use another approach to find and update only when saving if you prefer to bind a property
                this.SelectedGender = gender;  // Store selected gender in a property for later use
            }
        }

        private void textPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password))
            {
                TextPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextPassword.Visibility = Visibility.Visible;
            }
        }


    }
}
