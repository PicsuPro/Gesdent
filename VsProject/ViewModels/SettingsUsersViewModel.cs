using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Services;

namespace VsProject.ViewModels
{

    public class SettingsUsersViewModel : ViewModelBase
    {

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

            Users = new ObservableCollection<UserModel>(UserPrincipal.UserRepository.GetAll());

            AddUserCommand = new ViewModelCommand(ExecuteAddUserCommand);
            EditUserCommand = new ViewModelCommand((user) => ExecuteEditUserCommand((UserModel)user));
            RemoveUserCommand = new ViewModelCommand((user) => ExecuteRemoveUserCommand((UserModel)user), (user) => CanExecuteRemoveUserCommand((UserModel)user));
        }



        private void ExecuteAddUserCommand(object obj)
        {
            var newUser = new UserModel();

            if (DialogService.Show(new UserEditViewModel(newUser)) == true)
            {
                UserPrincipal.UserRepository.Add(newUser);
                Users.Add(UserPrincipal.UserRepository.GetByUsername(newUser.UserName));
            }


        }

        private void ExecuteEditUserCommand(UserModel user)
        {
            if (DialogService.Show(new UserEditViewModel(user)) == true)
            {
                UserPrincipal.UserRepository.Edit(user);
                var index = Users.IndexOf(user);
                Users.Remove(user);
                Users.Insert(index, UserPrincipal.UserRepository.GetById((Guid)user.Id));
                if (user.Id == UserPrincipal.Current?.Id)
                {
                    UserPrincipal.SetUser(user);
                }
            }

        }


        private void ExecuteRemoveUserCommand(UserModel user)
        {
            UserPrincipal.UserRepository.Remove(user);
            Users.Remove(user);
        }

        private static bool CanExecuteRemoveUserCommand(UserModel user)
        {
            return user?.Id != UserPrincipal.Current?.Id;
        }
    }
}

