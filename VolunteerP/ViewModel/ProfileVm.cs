using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerP.ServerApi.Models;
using VolunteerP.Utilities;

namespace VolunteerP.ViewModel
{
    public class ProfileVm : ViewModelBase
    {
        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public ProfileVm(User user)
        {
            User = user;
        }
    }
}
