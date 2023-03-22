using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VsProject.Services;
using VsProject.ViewModels;
using VsProject.Views;
namespace VsProject
{ 
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {

            VMVMappings.Initialize();
            var mainView = new MainView();
            if ((bool)DialogService.Show(new LoginViewModel()))
            {
                mainView.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}
