using System;
using System.Linq;
using System.Windows;
using VsProject.Resources.Controls;
using VsProject.Services;
using VsProject.ViewModels;

namespace VsProject.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            NavService.Navigated += OnNavigated;
        }
        private void OnNavigated(object view)
        {
            if (view is FrameworkElement frameworkElement)
            {
                navFrame.Content = frameworkElement;

                Type viewModelType = frameworkElement.DataContext.GetType();
                var selectedItem = sidebar.Items.OfType<NavListBoxItem>().FirstOrDefault(item => item.ViewModel == viewModelType);

                if (selectedItem != null)
                {
                    sidebar.SelectedItem = selectedItem;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavService.Navigate(new CalendarViewModel());
        }


    }
}
