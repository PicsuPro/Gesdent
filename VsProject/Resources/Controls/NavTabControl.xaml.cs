using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VsProject.Services;
using VsProject.ViewModels;

namespace VsProject.Resources.Controls
{
    /// <summary>
    /// Interaction logic for NavListBoxItem.xaml
    /// </summary>
    public partial class NavTabControl : TabControl
    {
        private bool navSelection = false;
        public NavTabControl()
        {
            NavService.NavigatedToChild += OnNavigated;
            InitializeComponent();
        }

        private void OnNavigated(FrameworkElement view)
        {
            var viewModelType = view.DataContext.GetType();
            var tabItem = Items.OfType<NavTabItem>().FirstOrDefault(item => item.ViewModel == viewModelType);
            if (tabItem != null)
            {
                tabItem.Content = view;
                navSelection = true;
                SelectedItem = tabItem;
                navSelection = false;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (!navSelection)
            {
                var viewModelType = e.AddedItems.OfType<NavTabItem>().FirstOrDefault()?.ViewModel;
                var viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
                if (viewModel != null)
                {
                    NavService.Navigate(viewModel);
                }
            }
        }
    }
}
