using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VsProject.Services;

namespace VsProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            if((bool)DialogService.Show(new LoginViewModel()))
            {
                // show the MainView
            }else
            {
                Application.Current.Shutdown();
            }
        }
    }
}
