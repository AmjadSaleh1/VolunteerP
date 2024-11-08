﻿using Microsoft.Win32;
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
using VolunteerP.ViewModel;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for Voulnteer.xaml
    /// </summary>
    public partial class Voulnteer : Window
    {
        public Voulnteer(string email)
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
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*png";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() == true)
            {
                var url = openDialog.FileName;
                this.UserProfileImage.ImageSource = new BitmapImage(new Uri(url));
            }
        }
    }

}
