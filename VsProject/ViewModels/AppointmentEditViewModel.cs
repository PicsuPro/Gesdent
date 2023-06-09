using System;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Repositories;

namespace VsProject.ViewModels
{
    public class AppointmentEditViewModel : ViewModelBase
    {
        private AppointmentViewModel _appointment = new AppointmentViewModel(new AppointmentModel() { Subject = "" ,Date = DateOnly.FromDateTime(DateTime.Now), StartTime = new TimeOnly( 12, 0, 0), EndTime = new TimeOnly(12, 30, 0) });
        private DateOnly _date;
        private int? _maxHour;
        private int _minHour = 0;
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

        public int? MaxHour
        {
            get => _maxHour; 
            set
            {
                _maxHour = value;
                OnPropertyChanged(nameof(MaxHour));
            }
        }
        public int MinHour
        {
            get => _minHour; 
            set
            {
                _minHour = value;
                OnPropertyChanged(nameof(MinHour));
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
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            IsNewUser = true;
            IsEditingPassword = IsNewUser;
        }
        public AppointmentEditViewModel(AppointmentViewModel appointment)
        {
            Appointment = appointment;
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            IsNewUser = appointment == null;
            IsEditingPassword = IsNewUser;
        }


      public AppointmentEditViewModel(AppointmentViewModel appointment, int maxhour, int minhour)
        {
            Appointment = appointment;
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            IsNewUser = appointment == null;
            IsEditingPassword = IsNewUser;

            MaxHour = maxhour;
            MinHour = minhour;
        }


        private void ExecuteSaveEdit(object obj)
        {
            End();
        }

        private bool CanExecuteSaveEdit(object obj)
        {
            return !(string.IsNullOrWhiteSpace(Appointment.Subject));
        }

    }
}

