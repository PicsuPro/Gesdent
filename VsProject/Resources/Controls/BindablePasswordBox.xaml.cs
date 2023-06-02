using System.Windows;
using System.Windows.Controls;

namespace VsProject.Resources.Controls
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {


        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }


        public BindablePasswordBox()
        {
            InitializeComponent();
            txtPassword.PasswordChanged += OnPasswordChanged;
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Closing += (sender, e) => { DataContext = null; };
            }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = txtPassword.Password;
        }

        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtBoxPassword.Text = txtPassword.Password;
            txtPassword.MaxWidth = 0;
            txtBoxPassword.MaxWidth = double.PositiveInfinity;
            txtBoxPassword.Focusable = true;
            txtPassword.Focusable = false;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = txtBoxPassword.Text;
            txtBoxPassword.MaxWidth = 0;
            txtPassword.MaxWidth = double.PositiveInfinity;
            txtPassword.Focusable = true;
            txtBoxPassword.Focusable = false;
        }

    }
}
