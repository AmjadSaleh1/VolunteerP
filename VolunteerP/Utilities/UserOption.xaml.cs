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
            btk.Click += OnInternalButtonClick;
        }

        // Expose an event that external controls can subscribe to
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserOption));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        private void OnInternalButtonClick(object sender, RoutedEventArgs e)
        {
            // Raise the external Click event to signal that this control was clicked
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ClickEvent, this);
            RaiseEvent(newEventArgs);

            // Handle visual and state updates internally
            UpdateSelectionState();
        }

        private void UpdateSelectionState()
        {
            var parent = this.Parent as Panel;
            if (parent != null)
            {
                foreach (UserOption sibling in parent.Children.OfType<UserOption>())
                {
                    sibling.IconColor = DefaultForeground; // Reset all siblings
                    sibling.IsSelected = false;            // Deselect all siblings
                }
            }

            this.IconColor = SelectedForeground; // Highlight this as selected
            this.IsSelected = true;
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(UserOption), new PropertyMetadata(false));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(UserOption));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(UserOption));

        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register(
            "IconColor", typeof(Brush), typeof(UserOption), new PropertyMetadata(DefaultForeground));

        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }
    }
}
