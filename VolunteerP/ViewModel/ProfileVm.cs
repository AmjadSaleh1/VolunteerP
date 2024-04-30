using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VolunteerP.ServerApi.Data;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;
using VolunteerP.View;

namespace VolunteerP.ViewModel
{
    public class ProfileVm : ViewModelBase
    {
        public ICommand UpdateInfoCommand { get; }
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
            UpdateInfoCommand = new RelayCommand(OpenUpdateInfoDialog);
        }

        private void OpenUpdateInfoDialog(object obj)
        {
            var userService = new UserService(new MongoDbContext().Database); // Correctly get an instance of UserService
            var dialog = new EditUserInfoDialog(User, userService);
            dialog.ShowDialog();
        }
    }
}
