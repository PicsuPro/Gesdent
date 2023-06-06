using System.Windows;

namespace VsProject.Views
{
    /// <summary>
    /// Interaction logic for AppointmentEditView.xaml
    /// </summary>
    public partial class YesNoDialog : Window
    {
        public YesNoDialog()
        {
            InitializeComponent();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
