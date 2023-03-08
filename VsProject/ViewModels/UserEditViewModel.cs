using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Repositories;
using System.Reflection.Metadata;
using System.Windows;
using System.Diagnostics;

namespace VsProject.ViewModels
{
    public class UserEditViewModel : ViewModelBase
    {
        //Fields
        private UserModel _user;
        private string _username;
        private string _password;
        private string _email;
        private string _errorMessage;

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public string Username
        {
            get => _username; set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get => _password; set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage; set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public string Email
        {
            get => _email; set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }


        public event EventHandler EditFinished;

        //-> Commands
        public ICommand SaveEditCommand { get; }
       

        public UserEditViewModel()
        {
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEditCommand, CanExecuteSaveEditCommand);
        }
        public UserEditViewModel(UserModel user)
        {
            User = user;
            Username = user.UserName;
            Email = user.Email;
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEditCommand, CanExecuteSaveEditCommand);
        }



        private void ExecuteSaveEditCommand(object obj)
        {
            User.Hash = Password;
            User.UserName = Username;
            User.Email = Email;
            EditFinished?.Invoke(this, EventArgs.Empty);
        }
        private bool CanExecuteSaveEditCommand(object obj)
        {
            return !(string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3);
        }
    
    }
}


