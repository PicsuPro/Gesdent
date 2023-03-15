using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
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

            Users = new ObservableCollection<UserModel>(UserPrincipal.Repository.GetAll());

            AddUserCommand = new ViewModelCommand(ExecuteAddUserCommand);
            EditUserCommand = new ViewModelCommand((user) => ExecuteEditUserCommand((UserModel)user));
            RemoveUserCommand = new ViewModelCommand((user) => ExecuteRemoveUserCommand((UserModel)user), (user) => CanExecuteRemoveUserCommand((UserModel)user));
        }



        private void ExecuteAddUserCommand(object obj)
        {
            var newUser = new UserModel();

            if (DialogService.Show(new UserEditViewModel(newUser)) == true)
            {
                    UserPrincipal.Repository.Add(newUser);
                    Users.Add(UserPrincipal.Repository.GetByUsername(newUser.UserName));
            }


        }

        private void ExecuteEditUserCommand(UserModel user)
        {
            if (DialogService.Show(new UserEditViewModel(user)) == true)
            {
                UserPrincipal.Repository.Edit(user);
                var index = Users.IndexOf(user);
                Users.Remove(user);
                Users.Insert(index, UserPrincipal.Repository.GetById((Guid)user.Id));
                if (user.Id == UserPrincipal.Current?.Id)
                {
                    UserPrincipal.Set(user);
                }
            }

        }


        private void ExecuteRemoveUserCommand(UserModel user)
        {
            UserPrincipal.Repository.Remove(user);
            Users.Remove(user);
        }

        private static bool CanExecuteRemoveUserCommand(UserModel user)
        {
            return user?.Id != UserPrincipal.Current?.Id;
        }
    }
}

