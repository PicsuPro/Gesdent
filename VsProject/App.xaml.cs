using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using VsProject.Models;
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
            CultureInfo.CurrentCulture = new CultureInfo("fr-FR");
            var mainView = new MainView();
            if ((bool)DialogService.Show(new LoginViewModel()))
            {
                //UserPrincipal.SetUser(new NetworkCredential("yes", "yes"));
                mainView.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}
