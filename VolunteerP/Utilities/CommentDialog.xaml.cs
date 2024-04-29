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
    /// Interaction logic for CommentDialog.xaml
    /// </summary>
    public partial class CommentDialog : Window
    {
        private Post _selectedPost;
        private PostService _postService;
        public List<Comment> ExistingComments { get; set; }
        public bool IsAnonymous { get; private set; }
        public string NewCommentText => NewCommentTextBox.Text;

        public CommentDialog(Post selectedPost)
        {
            InitializeComponent();
            _selectedPost = selectedPost ?? throw new ArgumentNullException(nameof(selectedPost));
            ExistingComments = _selectedPost.Comments.ToList();
            CommentsListView.ItemsSource = ExistingComments;
            _postService = ServiceLocator.PostService;
        }

        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            IsAnonymous = false;
            AddComment();
        }

        private void CommentAnonymously_Click(object sender, RoutedEventArgs e)
        {
            IsAnonymous = true;
            AddComment();
        }

        private void AddComment()
        {
            var newComment = new Comment
            {
                CommentText = NewCommentText,
                IsAnonymous = IsAnonymous,
                UserName = IsAnonymous ? UserHelper.CurrentUser.UserName : UserHelper.CurrentUser.Name
            };

            // Add the new comment to the local list and the ListView
            ExistingComments.Add(newComment);
            CommentsListView.Items.Refresh();

            // Add the new comment to the selected post's comments collection
            _selectedPost.Comments.Add(newComment);

            // Save the new comment to the database
            _postService.AddCommentToPost(_selectedPost.Id, newComment);

            // Clear the text box for new input
            NewCommentTextBox.Clear();
        }
    }
}
