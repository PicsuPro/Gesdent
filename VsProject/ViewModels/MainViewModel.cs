using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VsProject.Services;

namespace VsProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand SettingsCommand { get; }

        public MainViewModel()
        {
            SettingsCommand = new ViewModelCommand(ExecuteSettingsCommand);
        }

        private void ExecuteSettingsCommand(object obj)
        {
            DialogService.Show(new SettingsViewModel());
        }
    }
}
