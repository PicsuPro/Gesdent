using System.Windows;
using System.Windows.Controls;

namespace VsProject.Views
{
    /// <summary>
    /// Interaction logic for StaffEditView.xaml
    /// </summary>

    public partial class PatientEditView : UserControl
    {
        private int _currentPanelIndex = 0; // Initialize the counter variable

        public int CurrentPanelIndex 
        { 
            get => _currentPanelIndex;
            set 
            {

                if (value >= 0 && value < MainContainer.Children.Count)
                {
                    _currentPanelIndex = value;
                    NavigateToPanel(value);
                }
                if (value == MainContainer.Children.Count -1)
                {
                    NextPageButton.IsEnabled = false;
                }
                else
                {
                    NextPageButton.IsEnabled = true;
                }
                if(value == 0)
                {
                    PreviousPageButton.IsEnabled = false;
                }
                else
                {
                    PreviousPageButton.IsEnabled = true;
                }


            }
        }


        public PatientEditView()
        {
            InitializeComponent();
            CurrentPanelIndex = 0;
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPanelIndex++;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPanelIndex--;
        }

        private void NavigateToPanel(int index)
        {
            for (int i = 0; i < MainContainer.Children.Count; i++)
            {
                UIElement child = MainContainer.Children[i];
                if (i != index)
                {
                    child.Visibility = Visibility.Collapsed;
                }
                else
                {
                    child.Visibility= Visibility.Visible;
                }

            }
        }


    }
}
