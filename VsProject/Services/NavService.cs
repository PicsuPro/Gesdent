using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VsProject.Resources.Controls;
using VsProject.ViewModels;

namespace VsProject.Services
{
    public static class NavService
    {
        public static event Action<FrameworkElement>? Navigated;
        private static Dictionary<Type, ViewModelBase> loadedViewModels = new();

        public static void Navigate(Type viewModelType)
        {
            (Type viewType, Type? parentType) = VMVMappings.GetViewAndParentTypes(viewModelType);
            if (!loadedViewModels.TryGetValue(viewModelType, out var viewModel))
            {
                viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
                loadedViewModels[viewModelType] = viewModel;
            }
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;

            if (parentType != null)
            {
                var parent = (FrameworkElement)Activator.CreateInstance(parentType);

                int childCount = VisualTreeHelper.GetChildrenCount(parent);
                var tabControl = parent.FindLogicalChild<TabControl>();
                var tabItem = tabControl.Items.OfType<NavTabItem>().FirstOrDefault(item => item.ViewModel == viewModelType);
                if(tabItem != null)
                {
                    tabItem.Content = view;
                    tabControl.SelectedItem = tabItem;
                }
                Navigated?.Invoke(parent);
            }
            else
            {
                Navigated?.Invoke(view);
            }
        }
        public static void Navigate<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            (Type viewType, Type? parentType) = VMVMappings.GetViewAndParentTypes(viewModel.GetType());
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            Navigated?.Invoke(view);

            if (parentType != null)
            {
                var parent = (FrameworkElement)Activator.CreateInstance(parentType);

                int childCount = VisualTreeHelper.GetChildrenCount(parent);
                var tabControl = parent.FindLogicalChild<TabControl>();
                var tabItem = tabControl.Items.OfType<NavTabItem>().FirstOrDefault(item => item.ViewModel == viewModel.GetType());
                if (tabItem != null)
                {
                    tabItem.Content = view;
                    tabControl.SelectedItem = tabItem;
                }
                Navigated?.Invoke(parent);
            }
            else
            {
                Navigated?.Invoke(view);
            }
        }

        public static void Unload(Type viewModelType)
        {
            loadedViewModels.Remove(viewModelType);
        }

        public static Type GetParentTypeFromViewModelType(Type viewModelType)
        {
            var parentType = VMVMappings.GetParentType(viewModelType);
            if (parentType == null)
            {
                return VMVMappings.GetViewType(viewModelType);
            }
            return parentType;
        }

    }
}
