using System.Configuration;
using System.Data;
using System.Windows;
using VolunteerP.ServerApi.Data;

namespace VolunteerP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dbContext = new MongoDbContext();
            
        }
    }

}
