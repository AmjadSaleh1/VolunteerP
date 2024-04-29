using MongoDB.Bson;
using System;
using System.Collections.Generic;
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
    }
}
