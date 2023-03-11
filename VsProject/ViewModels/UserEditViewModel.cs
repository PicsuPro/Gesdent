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
using VsProject.Models.Repositories;
using System.Reflection.Metadata;
using System.Windows;
using System.Diagnostics;

namespace VsProject.ViewModels
{
    public class UserEditViewModel : ViewModelBase
    {
        //Fields
        private UserModel _user;
        private string _username = "";
        private string _password = "";
        private string _email = "";
        private string _errorMessage = "";
        private bool _isEditPassword = false;

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
                OnPropertyChanged(nameof(CanSaveEdit));
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                User.UserName = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(CanSaveEdit));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                User.Hash = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanSaveEdit));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                User.Email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanSaveEdit));
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

        public bool CanSaveEdit => !(string.IsNullOrWhiteSpace(User.UserName) || User.UserName.Length < 3 || (IsEditPassword && (string.IsNullOrWhiteSpace(User.Hash) || User.Hash.Length < 3)));

        public bool IsEditPassword
        {
            get => _isEditPassword; set 
            {
                if (!value) { Password = ""; }
                _isEditPassword = value;
                OnPropertyChanged(nameof(IsEditPassword));
                OnPropertyChanged(nameof(CanSaveEdit));
            }
        }


        //-> Commands

        public UserEditViewModel()
        {
            User = new UserModel();
        }
        public UserEditViewModel(UserModel user)
        {
            User = user;
            Username = User.UserName;
            Email = User.Email;
        }
    }
}


