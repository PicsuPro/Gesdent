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
            UserEditViewModel userEditViewModel = new UserEditViewModel(newUser);

            if (DialogService.Show(userEditViewModel) == true)
            {
                _userRepository.Add(userEditViewModel.User);
            }
            

        }

        private void ExecuteEditUserCommand(UserModel user)
        {
            UserEditViewModel userEditViewModel = new UserEditViewModel(user);
            
            if (DialogService.Show(userEditViewModel) == true)
            {
                _userRepository.Edit(userEditViewModel.User);
            }
        }


        private void ExecuteRemoveUserCommand(UserModel user)
        {
            Trace.WriteLine("REMOVE");
            _userRepository.Remove(user);
            Users.Remove(user);
        }

        private bool CanExecuteRemoveUserCommand(object obj)
        {
            return true;
        }
    }
}

