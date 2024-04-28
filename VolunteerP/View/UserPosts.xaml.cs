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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VolunteerP.ServerApi.Data;
using VolunteerP.ViewModel;
using static VolunteerP.View.Home;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for UserPosts.xaml
    /// </summary>
    public partial class UserPosts : UserControl
    {
        public UserPosts()
        {
            InitializeComponent();
            this.DataContext = new UserPostsVm();
        }
    }
}
