using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VolunteerP.ServerApi.Models
{
   

    public class Post
    {
        public ObjectId Id { get; set; }
        public string UserPost {  get; set; }
        public string PostName { get; set; }
        public DateTime PostTime { get; set; }
        public string ImagePath { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public bool IsEditable { get; internal set; }
        public bool IsAnonymous { get; set; }
        public bool IsVisible { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }
        public Post()
        {
            Comments = new ObservableCollection<Comment>();
        }
    }

    public class Comment
    {
        public string CommentText { get; set; }
        public bool IsAnonymous { get; set; }
        public string UserName { get; set; }
    }
}
