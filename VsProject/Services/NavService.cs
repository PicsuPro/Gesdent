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
        public static event Action<FrameworkElement>? NavigatedToParent;
        public static event Action<FrameworkElement>? NavigatedToChild;
        private static Dictionary<Type, ViewModelBase> loadedViewModels = new();

        public static void Navigate(Type viewModelType)
        {
            (Type viewType, Type? parentType) = VMVMappings.GetViewAndParentTypes(viewModelType);
            if (!loadedViewModels.TryGetValue(viewModelType, out var viewModel))
            {
                viewModel = (ViewModelBase)Activator.CreateInstance(viewModelType);
                if (viewModel.KeepLoaded)
                loadedViewModels[viewModelType] = viewModel;
            }
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;

            if (parentType != null)
            {
                var parent = (FrameworkElement)Activator.CreateInstance(parentType);
                NavigatedToParent?.Invoke(parent);
                NavigatedToChild?.Invoke(view);
            }
            else
            {
                NavigatedToParent?.Invoke(view);
            }
        }
        public static void Navigate<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            (Type viewType, Type? parentType) = VMVMappings.GetViewAndParentTypes(viewModel.GetType());
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;

            if (parentType != null)
            {
                var parent = (FrameworkElement)Activator.CreateInstance(parentType);
                NavigatedToParent?.Invoke(parent);
                NavigatedToChild?.Invoke(view);
            }
            else
            {
                NavigatedToParent?.Invoke(view);
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
