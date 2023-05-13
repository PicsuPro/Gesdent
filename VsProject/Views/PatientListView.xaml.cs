using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VsProject.Views
{
    /// <summary>
    /// Interaction logic for PatientListView.xaml
    /// </summary>
    public partial class PatientListView : UserControl
    {
        public PatientListView()
        {
            InitializeComponent();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PatientFrame.Navigate(new Uri("/Views/AddPatientView.xaml", UriKind.Relative));
            btnAdd.Visibility=Visibility.Hidden;
            btnRemove.Visibility = Visibility.Hidden;
            txtAdd.Visibility=Visibility.Visible;
            returnn.Visibility=Visibility.Visible;

        }

        private void NavListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
