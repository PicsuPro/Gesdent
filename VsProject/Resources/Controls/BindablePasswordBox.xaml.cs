using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

namespace VsProject.Controls
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
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = txtBoxPassword.Text;
            txtBoxPassword.MaxWidth = 0;
            txtPassword.MaxWidth = double.PositiveInfinity;
        }


    }
}
