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
using VsProject.Repositories;
using VsProject.Views;

namespace VsProject.ViewModels
{

    public class UsersSettingsViewModel : ViewModelBase
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

        public UsersSettingsViewModel()
        {
            _userRepository = new UserRepository();

            Users = new ObservableCollection<UserModel>(_userRepository.GetAll());

            AddUserCommand = new ViewModelCommand(ExecuteAddUserCommand);
            EditUserCommand = new ViewModelCommand((user) => ExecuteEditUserCommand((UserModel)user));
            RemoveUserCommand = new ViewModelCommand((user) => ExecuteRemoveUserCommand((UserModel)user) , CanExecuteRemoveUserCommand);
        }

        private void ExecuteAddUserCommand(object obj)
        {
            var newUser = new UserModel();
           // var userEditView = new UserEditView(newUser);

       
        }

        private void ExecuteEditUserCommand(UserModel user)
        {
            UserEditViewModel userEditViewModel = new UserEditViewModel(user);
            UserEditView userEditView = new UserEditView { DataContext = userEditViewModel };
            if (userEditView.ShowDialog() == true)
            {
                _userRepository.Edit(userEditViewModel.User);
            }
        }


        private void ExecuteRemoveUserCommand(UserModel user)
        {
            Trace.WriteLine("REMOVE");
            //_userRepository.Remove(_selectedUser);
            //Users.Remove(_selectedUser);
        }

        private bool CanExecuteRemoveUserCommand(object obj)
        {
            return true;
        }
    }
}

