using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Models.Repositories;
using VsProject.Services;
using VsProject.Views;

namespace VsProject.ViewModels
{

    public class SettingsUsersViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;

        private ObservableCollection<UserModel> _users;

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

        public SettingsUsersViewModel()
        {
            _userRepository = new UserRepository();

            Users = new ObservableCollection<UserModel>(_userRepository.GetAll());

            AddUserCommand = new ViewModelCommand(ExecuteAddUserCommand);
            EditUserCommand = new ViewModelCommand((user) => ExecuteEditUserCommand((UserModel)user));
            RemoveUserCommand = new ViewModelCommand((user) => ExecuteRemoveUserCommand((UserModel)user));
        }

        private void ExecuteAddUserCommand(object obj)
        {
            var newUser = new UserModel();

            if (DialogService.Show(new UserEditViewModel(newUser)) == true)
            {
                _userRepository.Add(newUser);
                Users.Add(_userRepository.GetByUsername(newUser.UserName));

            }


        }

        private void ExecuteEditUserCommand(UserModel user)
        {
            if (DialogService.Show(new UserEditViewModel(user)) == true)
            {
                _userRepository.Edit(user);
                var index = Users.IndexOf(user);
                Users.Remove(user);
                Users.Insert(index, _userRepository.GetById((Guid)user.Id));
            }
        }


        private void ExecuteRemoveUserCommand(UserModel user)
        {
            _userRepository.Remove(user);
            Users.Remove(user);
        }


    }
}

