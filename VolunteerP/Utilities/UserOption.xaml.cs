using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VolunteerP.Utilities
{
    public partial class UserOption : UserControl
    {
        // Foreground color when not selected
        private static readonly Brush DefaultForeground = Brushes.White;

        // Foreground color when selected
        private static readonly Brush SelectedForeground = Brushes.Green;

        public UserOption()
        {
            InitializeComponent();
            // Set the default icon color
            IconColor = DefaultForeground;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(UserOption));

        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(UserOption));

        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register(
            "IconColor", typeof(Brush), typeof(UserOption), new PropertyMetadata(DefaultForeground));

        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Assuming 'UserOption' controls are direct children of the same parent panel
            var parent = this.Parent as Panel;
            if (parent != null)
            {
                // Deselect all other UserOption controls
                foreach (UserOption sibling in parent.Children.OfType<UserOption>())
                {
                    // Reset the icon color to default for all siblings
                    sibling.IconColor = Brushes.White; // or your default color
                }
            }

            // Select this UserOption control by setting its icon color to green
            this.IconColor = Brushes.Green;
        }

        private static IEnumerable<T> FindSiblings<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null) return Enumerable.Empty<T>();

            return Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(parent))
                             .Select(i => VisualTreeHelper.GetChild(parent, i))
                             .OfType<T>()
                             .Where(control => control != child);
        }
    }
}
