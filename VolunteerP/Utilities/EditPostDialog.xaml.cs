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

        public EditPostDialog(Post post, PostService postService)
        {
            InitializeComponent();
            _postService = postService;
            _post = post;
            PostContentTextBox.Text = post.UserPost;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _post.UserPost = PostContentTextBox.Text;
            _postService.UpdatePostAsync(_post.Id, _post.UserPost);
            this.DialogResult = true;
        }
    }
}
