using Microsoft.Win32;
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
using VolunteerP.ServerApi.Models;
using VolunteerP.ViewModel;
using System.IO;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for Needy.xaml
    /// </summary>
    public partial class Needy : Window
    {
       

        public Needy(string email)
        {
            
            InitializeComponent();
            var viewModel = new UserProfileViewModel();
            DataContext = viewModel;
            viewModel.InitializeAsync(email);
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LogOut_click(object sender, RoutedEventArgs e)
        {
            var appWindow = new View.Log_Sign_in();
            appWindow.Show();
            this.Close();
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Image files|*.bmp;*.jpg;*.png",
                FilterIndex = 1
            };
            if (openDialog.ShowDialog() == true)
            {
                string filePath = openDialog.FileName;
                string destinationPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), System.IO.Path.GetFileName(filePath));
                File.Copy(filePath, destinationPath, true);
                UserSignProfileImage.ImageSource = new BitmapImage(new Uri(destinationPath, UriKind.Absolute));
                var viewModel = DataContext as UserProfileViewModel;
                if (viewModel != null)
                {
                    viewModel.User.ImageUrl = destinationPath;  // This should trigger the UI update
                    viewModel.UpdateUserProfileImage();  // Assuming this updates the database
                }
            }
        }
    }
}
