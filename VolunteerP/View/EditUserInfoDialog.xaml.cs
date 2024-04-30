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
using VolunteerP.ViewModel;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for EditUserInfoDialog.xaml
    /// </summary>
    public partial class EditUserInfoDialog : Window
    {
        public EditUserInfoDialog(User user, UserService userService)
        {
            InitializeComponent();
            DataContext = new EditUserInfoViewModel(user, userService, Close);
        }
    }
}
