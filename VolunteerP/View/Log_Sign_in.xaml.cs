using MongoDB.Bson;
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
using System.Windows.Shapes;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Services;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for Log_Sign_in.xaml
    /// </summary>
    public partial class Log_Sign_in : Window
    {
        private UserService _userService;
        public Log_Sign_in()
        {
            InitializeComponent();
            InitializeUserService();
        }

        private void InitializeUserService()
        {
            var dbContext = new MongoDbContext();  // Assuming MongoDbContext is correctly set up
            _userService = new UserService(dbContext.Database);
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var appWindow=new View.SignUp();
            appWindow.Show();
            this.Close();
        }

        private void textEmail_MouseDown(object sender, MouseEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length  >  0)
            {
                TextEmail.Visibility=Visibility.Collapsed;
            }
            else
            {
                TextEmail.Visibility = Visibility.Visible;
            }
        }

        private void textPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                TextPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextPassword.Visibility = Visibility.Visible;
            }

        }

        private async void SignIn_click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtPassword.Password))
            {
                try
                {
                    bool isValidUser = await _userService.ValidateUserLogin(txtEmail.Text, txtPassword.Password); // Await is used correctly here
                    if (isValidUser)
                    {
                        MessageBox.Show("Login Successful!");
                        string gender = await _userService.GetUserGenderByEmail(txtEmail.Text);
                        Window appWindo;
                        if(gender.ToLower()=="male")
                        {
                            appWindo = new View.Needy();
                            appWindo.Show();
                        }
                        else
                        {
                            appWindo = new View.Voulnteer();
                            appWindo.Show();
                        }
                        
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Login Failed. Please check your email and password.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter both email and password.");
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
