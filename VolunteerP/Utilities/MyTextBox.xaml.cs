using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace VolunteerP.Utilities
{
    public partial class MyTextBox : UserControl
    {
        public MyTextBox()
        {
            InitializeComponent();
            // Ensure textbox's Text property binds to this UserControl's Text property
            textbox.DataContext = this;
            textbox.SetBinding(TextBox.TextProperty, new Binding("Text")
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
            "Hint", typeof(string), typeof(MyTextBox));

        // Text DependencyProperty
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(MyTextBox), new FrameworkPropertyMetadata(string.Empty));

        // Expose TextBox's TextChanged event
        public event TextChangedEventHandler TextChanged
        {
            add { textbox.TextChanged += value; }
            remove { textbox.TextChanged -= value; }
        }
    }
}
