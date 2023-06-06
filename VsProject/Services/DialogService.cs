using System;
using System.Windows;
using VsProject.ViewModels;
using VsProject.Views;

namespace VsProject.Services
{
    public static class DialogService
    {
        public static bool? Show<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            var viewType = VMVMappings.GetViewType(viewModel.GetType());
            if (viewType == null)
            {
                throw new ArgumentException($"No view mapping defined for view model type '{viewModel.GetType().FullName}'");
            }

            var view = (Window)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;
            viewModel.Ending += (sender, args) =>
            {
                view.DialogResult = true;
                viewModel.Ending -= (sender, args) => { };
            };

            var result = view.ShowDialog();
            view.Close();

            return result;
        }
        public static bool? ShowYesNoDialog()
        {
            var view = new YesNoDialog();
            var result = view.ShowDialog();
            view.Close();
            return result;
        }
    }

}
