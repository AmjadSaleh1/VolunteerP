using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;

namespace VolunteerP.ViewModel
{
    public class PostVm : ViewModelBase
    {
        private PostService postService;
        private String _post;

        public String Post
        {
            get { return _post; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Post cannot be empty.");
                _post = value;
            }
        }
        public PostVm() 
        {
            var mongoDBContext = new MongoDbContext();
            postService = new PostService(mongoDBContext.Database);

        }
    }
}
