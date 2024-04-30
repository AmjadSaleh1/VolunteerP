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

            // Initialize the database context
            var dbContext = new MongoDbContext();

            // Initialize services with the database instance
            var userService = new UserService(dbContext.Database);
            var postService = new PostService(dbContext.Database);

            // Set the DataContext
            DataContext = new AdminViewModel(userService, postService);
        }
    }
}
