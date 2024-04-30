using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Models;

namespace VolunteerP.Utilities
{
    public static class EventSystem
    {
        public delegate void PostUpdatedHandler(Post post);
        public static event PostUpdatedHandler OnPostUpdated;

        public static void RaisePostUpdated(Post post)
        {
            OnPostUpdated?.Invoke(post);
        }
    }
}
