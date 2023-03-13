using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using VsProject.ViewModels;

namespace VsProject.Views
{
    /// <summary>
    /// Interaction logic for SettingsUsersView.xaml
    /// </summary>
    public partial class SettingsUsersView : UserControl
    {
        public SettingsUsersView()
        {
            InitializeComponent();
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Closing += (sender, e) => { DataContext = null; };
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
