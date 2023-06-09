using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Services;

namespace VsProject.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {

        private bool _isChanged = false;
        public bool IsChanged 
        {
            get => _isChanged;
            set 
            {
                _isChanged = value;
                CommandManager.InvalidateRequerySuggested();
                OnPropertyChanged(nameof(IsChanged));
            }
        }


        private ObservableCollection<AppointmentViewModel> _oldAppointments;
        private DateOnly _startDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateOnly _endDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
        public DateOnly EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        private TimeOnly _startTime { get; set; } = new TimeOnly(8, 0);
        public TimeOnly StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }
        private int _hourCount { get; set; } = 9;
        public int HourCount
        {
            get => _hourCount;
            set
            {
                if (value > 0)
                    _hourCount = value;
                else
                    throw new ArgumentOutOfRangeException("HourCount", "HourCount must be a positive value.");

                OnPropertyChanged(nameof(HourCount));
            }
        }


        private ObservableCollection<AppointmentViewModel> _appointments = new ObservableCollection<AppointmentViewModel>();


        public ObservableCollection<AppointmentViewModel> Appointments
        {
            get => _appointments;
            set
            {
                _appointments = value;
                OnPropertyChanged(nameof(Appointments));
            }
        }


        public ICommand AppointmentEditCommand { get; }
        public ICommand AppointmentRemoveCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }

        public CalendarViewModel()
        {
            AppointmentEditCommand = new ViewModelCommand(ExecuteAppointmentEdit);
            AppointmentRemoveCommand = new ViewModelCommand(ExecuteAppointmentRemove);
            SaveEditCommand = new ViewModelCommand(ExecuteSaveEdit, CanExecuteSaveEdit);
            CancelEditCommand = new ViewModelCommand(ExecuteCancelEdit);
            List<AppointmentModel> appointments = (List<AppointmentModel>)UserPrincipal.AppointmentRepository.GetAll();
            foreach (AppointmentModel appointment in appointments)
            {
                var appointmentViewModel = new AppointmentViewModel(appointment);
                Appointments.Add(appointmentViewModel);
                appointmentViewModel.PropertyChanged += Appointment_PropertyChanged;

            }
            _oldAppointments = new ObservableCollection<AppointmentViewModel>(Appointments.Select(a => new AppointmentViewModel(a)));


        }

        private void Appointment_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(!IsChanged)
            IsChanged = true;
        }


        private void ExecuteAppointmentEdit(object obj)
        {
            if(obj is AppointmentViewModel appointment) 
            {
                var old = new AppointmentViewModel(appointment);
                var result = DialogService.Show(new AppointmentEditViewModel(appointment, StartTime.Hour + HourCount, StartTime.Hour)) ;
                if (result == false)
                {
                    appointment.Subject = old.Subject;
                    appointment.Date = old.Date;
                    appointment.StartTime = old.StartTime;
                    appointment.EndTime = old.EndTime;
                }
                else if (result == true)
                {
                    UserPrincipal.AppointmentRepository.Edit(appointment.Appointment);
                }
            }

        }
        private void ExecuteAppointmentRemove(object obj)
        {
            if(obj is AppointmentViewModel appointment) 
            {
                if (DialogService.ShowYesNoDialog() == true)
                {
                    Appointments.Remove(appointment);
                    UserPrincipal.AppointmentRepository.Remove(appointment.Appointment);
                }
            }

        }
        private void ExecuteSaveEdit(object obj)
        {
            if (DialogService.ShowYesNoDialog() == true)
            {
                for (int i = 0; i < Appointments.Count; i++)
                {
                    if (_oldAppointments[i] != Appointments[i])
                        UserPrincipal.AppointmentRepository.Edit(Appointments[i].Appointment);
                }
                IsChanged = false;
                _oldAppointments = new ObservableCollection<AppointmentViewModel>(Appointments.Select(a => new AppointmentViewModel(a)));
            }
        }
        private bool CanExecuteSaveEdit(object obj)
        {
            return IsChanged;
        }
            
        private void ExecuteCancelEdit(object obj)
        {
            foreach (AppointmentViewModel appointment in Appointments)
            {
                appointment.PropertyChanged -= Appointment_PropertyChanged;
            }
            Appointments.Clear();
            List<AppointmentModel> appointments = (List<AppointmentModel>)UserPrincipal.AppointmentRepository.GetAll();
            foreach (AppointmentModel appointment in appointments)
            {
                var appointmentViewModel = new AppointmentViewModel(appointment);
                Appointments.Add(appointmentViewModel);
                appointmentViewModel.PropertyChanged += Appointment_PropertyChanged;

            }
            IsChanged=false;
            _oldAppointments = new ObservableCollection<AppointmentViewModel>(Appointments.Select(a => new AppointmentViewModel(a)));
            NavService.Navigate(this);
        }

    }





}

