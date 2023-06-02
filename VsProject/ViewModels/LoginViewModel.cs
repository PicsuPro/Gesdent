using System.Net;
using System.Windows.Input;
using VsProject.Models;

namespace VsProject.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Fields
        private string? _username;
        private string? _password;
        private string? _errorMessage;


        public string? Username
        {
            get => _username; set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string? Password
        {
            get => _password; set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string? ErrorMessage
        {
            get => _errorMessage; set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        //-> Commands
        public ICommand LoginCommand { get; }
        public ICommand? RememberPasswordCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private void ExecuteLoginCommand(object obj)
        {
            if (UserPrincipal.SetUser(new NetworkCredential(Username, Password)))
            {
                End();
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }
        private bool CanExecuteLoginCommand(object obj)
        {
            return !(string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3);
        }


    }
}
