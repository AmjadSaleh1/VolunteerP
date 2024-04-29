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
using VolunteerP.ServerApi.Services;

namespace VolunteerP.Utilities
{
    /// <summary>
    /// Interaction logic for EditPostDialog.xaml
    /// </summary>
    public partial class EditPostDialog : Window
    {
        private PostService _postService;
        private Post _post;

        public string PostContent
        {
            get { return PostContentTextBox.Text; }
            set { PostContentTextBox.Text = value; }
        }

        public string ImagePath
        {
            get { return _post.ImagePath; }
            set { _post.ImagePath = value; UpdateImageDisplay(); }
        }

        public EditPostDialog(Post post, PostService postService)
        {
            InitializeComponent();
            _postService = postService;
            _post = post;
            PostContentTextBox.Text = post.UserPost;
            PostContentTextBox.Text = post.UserPost;
            UpdateImageDisplay();
        }

        private void UpdateImageDisplay()
        {
            PostImage.Source = string.IsNullOrEmpty(_post.ImagePath) ? null : new BitmapImage(new Uri(_post.ImagePath));
        }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp",
                Title = "Select a Photo"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
            }
        }

        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            ImagePath = null; // Clears the image path
        }



        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _post.UserPost = PostContentTextBox.Text;
            _postService.UpdatePostAsync(_post.Id, _post.UserPost); // Update text
            _postService.UpdatePostImagePath(_post.Id, _post.ImagePath);  // This should handle `null` as a "delete" operation
            this.DialogResult = true;
        }
    }
}
