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
using VolunteerP.ServerApi.Services;
using VolunteerP.ViewModel;
using VolunteerP.ServerApi.Models;
using VolunteerP.ServerApi.Data;
using VolunteerP.Utilities;

namespace VolunteerP.View
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
            var dbContext = new MongoDbContext();
            var userService = new UserService(dbContext.Database);
            var postService = new PostService(dbContext.Database);
            DataContext = new AdminViewModel(userService, postService);
        }

        private void Back_to_app(object sender, EventArgs e)
        {
            var Window = new View.Needy(UserHelper.CurrentUser.Email);
            Window.Show();
            this.Close();

        }
    }
}
