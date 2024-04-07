using System.Windows;
using System.Windows.Controls;


namespace VolunteerP.Utilities
{
    /// <summary>
    /// Interaction logic for MyTextBox.xaml
    /// </summary>
    public partial class MyTextBox : UserControl
    {
        public MyTextBox()
        {
            InitializeComponent();
        }

        public string SignHint
        {
            get { return (string) GetValue(Hintproperty); }
            set { SetValue(Hintproperty, value); }
        }

        public static readonly DependencyProperty Hintproperty = DependencyProperty.Register
            ("SignHint", typeof(string), typeof(MyTextBox));
    }
}
