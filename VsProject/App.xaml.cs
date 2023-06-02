using System.Globalization;
using System.Net;
using System.Windows;
using VsProject.Models;
using VsProject.Services;
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
            //if ((bool)DialogService.Show(new LoginViewModel()))
            //{
                UserPrincipal.SetUser(new NetworkCredential("yes", "yes"));
                mainView.Show();
            //}
            //else
            //{
            //    Shutdown();
            //}
        }
    }
}
