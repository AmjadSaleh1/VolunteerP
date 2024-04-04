using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolunteerP.Utilities;
using System.Windows.Input;
using System.Transactions;
using VolunteerP.View;

namespace VolunteerP.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand ProfileCommand { get; set; } 
        public ICommand LogoutCommand { get; set; }
  
     

        private void Home(object obj) => CurrentView = new HomeVm();
        private void Product(object obj) => CurrentView = new ProductVm();
        private void Profile(object obj) => CurrentView = new ProfileVm();
        private void LogOut(object obj) => CurrentView = new LogOutVm();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            ProductsCommand = new RelayCommand(Product);
            ProfileCommand = new RelayCommand(Profile);
            LogoutCommand = new RelayCommand(LogOut);

            // Startup Page
            CurrentView = new HomeVm();
        }
    }
}
