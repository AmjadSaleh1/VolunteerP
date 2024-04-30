using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Services;
using VolunteerP.Utilities;

namespace VolunteerP.ViewModel
{
    public class EditUserInfoViewModel : ViewModelBase
    {
        public User User { get; }
        public ICommand SaveCommand { get; }
        public Action CloseAction { get; set; }
        private UserService _userService;

        public EditUserInfoViewModel(User user, UserService userService, Action closeAction)
        {
            User = user;
            _userService = userService;
            CloseAction = closeAction;
            SaveCommand = new RelayCommand(obj => SaveChanges());
            _userService = userService;
        }

        private async void SaveChanges()
        {
            try
            {
                await _userService.UpdateUserinfo(User);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., logging or message to user
                Console.WriteLine("Error updating user: " + ex.Message);
                // Optionally keep the dialog open or notify the user of the failure
            }
        }
    }
}
