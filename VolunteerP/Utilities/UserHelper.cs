using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Models;

namespace VolunteerP.Utilities
{
    public static class UserHelper
    {
        public static User CurrentUser { get; set; }

        public static void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }
    }
}
