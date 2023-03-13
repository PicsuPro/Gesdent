using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VsProject.Resources.Controls
{
    public partial class ControlBar : UserControl
    {
        private Window _window;
        public static readonly DependencyProperty MinimizeProperty = DependencyProperty.Register(
        "Minimize",
        typeof(Visibility),
        typeof(ControlBar),
        new PropertyMetadata(Visibility.Collapsed));

        public Visibility Minimize
        {
            get { return (Visibility)GetValue(MinimizeProperty); }
            set { SetValue(MinimizeProperty, value); }
        }

        public ControlBar()
        {
            _window = Window.GetWindow(this);
            InitializeComponent();
            Loaded += (sender, e) =>
            {
                _window = Window.GetWindow(this);
                Loaded -= (s, ev) => { }; 
            };
        }


        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            _window.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (_window.Owner != null) 

                _window.DialogResult = false;
            else
                _window.Close();
        }

        private void ControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _window.DragMove();
        }
    }
}
