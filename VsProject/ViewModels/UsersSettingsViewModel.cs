using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Repositories;

namespace VsProject.ViewModels
{

    public class UsersSettingsViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;

        private UserModel _selectedUser;
        private ObservableCollection<UserModel> _users;

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand RemoveUserCommand { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public UsersSettingsViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _userRepository = new UserRepository();

            Users = new ObservableCollection<UserModel>(_userRepository.GetAll());

            AddUserCommand = new ViewModelCommand(ExecuteAddUserCommand);
            EditUserCommand = new ViewModelCommand(ExecuteEditUserCommand);
            RemoveUserCommand = new ViewModelCommand(ExecuteRemoveUserCommand, CanExecuteRemoveUserCommand);
        }

        private void ExecuteAddUserCommand(object obj)
        {
            var newUser = new UserModel();
            //var userEditViewModel = new UserEditViewModel(newUser);

            //if (userEditViewModel.ShowDialog() == true)
            //{
            //  _userRepository.Add(newUser);
            //  Users.Add(newUser);
            //}
        }

        private void ExecuteEditUserCommand(object obj)
        {
            //var userEditViewModel = new UserEditViewModel(_selectedUser);

            //if (userEditViewModel.ShowDialog() == true)
            //{
            //    _userRepository.Edit(_selectedUser);
            //}
        }   

        private void ExecuteRemoveUserCommand(object obj)
        {
            _userRepository.Remove(_selectedUser);
            Users.Remove(_selectedUser);
        }

        private bool CanExecuteRemoveUserCommand(object obj)
        {
            return _selectedUser != null;
        }
    }
}

