using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VsProject.ViewModels;

namespace VsProject.Services
{
    public static class DialogService
    {
        static DialogService()
        {
        }
        public static void Void() { }

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
            bool? result = view.ShowDialog();
            view.Close();
            return result;
        }
    }

}
