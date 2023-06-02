using System;
using System.Collections.Generic;
using System.Windows;
using VsProject.ViewModels;

namespace VsProject.Services
{
    public static class NavService
    {
        public static event Action<object> Navigated;
        public static Dictionary<Type, ViewModelBase> loadedViewModels = new();

        public static void Navigate(Type viewModelType)
        {
            var viewType = VMVMappings.GetViewType(viewModelType);
            if (!loadedViewModels.TryGetValue(viewModelType, out var viewModel))
            {
                viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
                loadedViewModels[viewModelType] = viewModel;
            }

            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            Navigated?.Invoke(view);
        }
        public static void Navigate<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            var viewType = VMVMappings.GetViewType(viewModel.GetType());
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            Navigated?.Invoke(view);
        }

        public static void Unload(Type viewModelType)
        {
            loadedViewModels.Remove(viewModelType);
        }
    }
}
