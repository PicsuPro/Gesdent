using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VsProject.Models;

namespace VsProject.ViewModels
{
    public class AppointmentEditViewModel : ViewModelBase
    {
        //Fields
        private AppointmentViewModel _appointment = new AppointmentViewModel(new AppointmentModel() { Subject = "We NEed To Talk About thhis" , StartDateTime= new DateTime(2023,5,20,12,0,0), Duration = new TimeSpan(1,0,0)});
        private string _errorMessage = "";
        private bool _isNewUser = false;
        private bool _isEditPassword = true;

        public AppointmentViewModel Appointment
        {
            get => _appointment;
            set
            {
                _appointment = value;
                OnPropertyChanged(nameof(Appointment));
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
        public AppointmentEditViewModel()
        {

        }
        public AppointmentEditViewModel(AppointmentViewModel appointment)
        {
            Appointment = appointment;
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            IsNewUser = appointment == null;
            IsEditingPassword = IsNewUser;
        }


        private void ExecuteSaveEdit(object obj)
        {
            End();
        }

        private bool CanExecuteSaveEdit(object obj)
        {
            return true;/*!(string.IsNullOrWhiteSpace(User.UserName) || User.UserName.Length < 3 || (IsEditingPassword && (string.IsNullOrWhiteSpace(User.Hash) || User.Hash.Length < 3)));*/
        }

    }
}

