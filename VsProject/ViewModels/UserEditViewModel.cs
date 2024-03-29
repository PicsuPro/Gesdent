﻿using System.Windows.Input;
using VsProject.Models;

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
        private bool _isNewUser = false;
        private bool _isEditPassword = true;

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
            get => _username;
            set
            {
                _username = value;
                User.UserName = value;
                OnPropertyChanged(nameof(Username));
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


        public bool IsEditingPassword
        {
            get => _isEditPassword; set
            {

                _isEditPassword = IsNewUser || value;

                OnPropertyChanged(nameof(IsEditingPassword));
            }
        }

        public bool IsNewUser
        {
            get => _isNewUser; set
            {
                _isNewUser = value;
                OnPropertyChanged(nameof(IsNewUser));
                OnPropertyChanged(nameof(IsNotNewUser));
            }
        }
        public bool IsNotNewUser => !_isNewUser;



        //-> Commands
        public ICommand SaveEditCommand { get; }
        public UserEditViewModel()
        {

        }
        public UserEditViewModel(UserModel user)
        {
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            IsNewUser = user.Id == null;
            IsEditingPassword = IsNewUser;
            User = user;
            Username = User.UserName;
            Email = User.Email;
        }


        private void ExecuteSaveEdit(object obj)
        {
            End();
        }

        private bool CanExecuteSaveEdit(object obj)
        {
            return !(string.IsNullOrWhiteSpace(User.UserName) || User.UserName.Length < 3 || (IsEditingPassword && (string.IsNullOrWhiteSpace(User.Hash) || User.Hash.Length < 3)));
        }

    }
}


