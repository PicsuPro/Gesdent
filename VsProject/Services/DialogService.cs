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
           // ViewMappings.LoadMappingsFromFile("ViewMappings.xml");
        }

        public static bool? Show<TViewModel>(TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            var viewType = 0;//= ViewMappings.GetMapping(viewModel.GetType());
            if (viewType == null)
            {
                throw new ArgumentException($"No view mapping defined for view model type '{viewModel.GetType().FullName}'");
            }

            var view = new Window();//(Window)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;

            return view.ShowDialog();
        }
    }

}
