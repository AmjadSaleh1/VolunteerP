using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;
using VolunteerP.Utilities;
using VolunteerP.ServerApi.Services;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private UserService _userService;
        private string userImageUrl;
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
            try
            {
                if (!AreAllFieldsFilled())
                {
                    DisplayFieldErrors();
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                if (await _userService.EmailExistsAsync(emailTextBox.Text))
                {
                    MessageBox.Show("This email is already registered.");
                    return;
                }

                if (!IsValidEmail(emailTextBox.Text))
                {
                    emailErrorIcon.Visibility = Visibility.Visible; // Show the error icon
                    MessageBox.Show("Please enter a valid email address.");
                    return;
                }

                User newUser = new User
                {
                    Name = nameTextBox.Text,
                    Password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Password),
                    Email = emailTextBox.Text,
                    DateOfBirth = dateOfBirthPicker.SelectedDate,
                    PhoneNumber = Phone.Text,
                    Location = location.Text,
                    Gender = genderstring,
                    ImageUrl= userImageUrl
                };

                await _userService.AddUserAsync(newUser);
                MessageBox.Show("User registered successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when trying to register user: {ex.Message}");
            }
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*png";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() == true)
            {
                userImageUrl = openDialog.FileName;
                this.UserProfileImage.ImageSource = new BitmapImage(new Uri(userImageUrl));
            }
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
                passwordErrorIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextPassword.Visibility = Visibility.Visible;
            }
        }

        private bool AreAllFieldsFilled()
        {
            return !string.IsNullOrWhiteSpace(nameTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(txtPassword.Password) &&
                   !string.IsNullOrWhiteSpace(emailTextBox.Text);// Ensure all required fields are not empty
        }

        private void DisplayFieldErrors()
        {
            nameErrorIcon.Visibility = Visibility.Collapsed;
            passwordErrorIcon.Visibility = Visibility.Collapsed;
            emailErrorIcon.Visibility = Visibility.Collapsed;
            

            // Display error icon if fields are empty
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
                nameErrorIcon.Visibility = Visibility.Visible;

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
                passwordErrorIcon.Visibility = Visibility.Visible;

            if (string.IsNullOrWhiteSpace(emailTextBox.Text))
                emailErrorIcon.Visibility = Visibility.Visible;

        }


        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Hide the error icon when the user starts typing in the email field
            emailErrorIcon.Visibility = Visibility.Collapsed;
        }

        private void NameTextBox_TextChanged(Object sender, TextChangedEventArgs e)
        {
            nameErrorIcon.Visibility = Visibility.Collapsed;
        }

        //check if the email have a valid

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|hotmail\.com)$";
            return Regex.IsMatch(email, pattern);
        }

        private void Cancel_click(object sender, EventArgs e)
        {
            var appWindow = new View.Log_Sign_in();
            appWindow.Show();
            this.Close();

        }
        private void Back_click(object sender, EventArgs e)
        {
            var appWindow = new View.Log_Sign_in();
            appWindow.Show();
            this.Close();
        }

    }
}
