using System;
using System.Windows;
using VsProject.Models;

namespace VsProject.ViewModels
{
    public class AppointmentViewModel : ViewModelBase
    {
        private readonly AppointmentModel _appointment;
        private readonly PatientModel _patient;
        private readonly string _patientName;
        public event Action<AppointmentViewModel, DateOnly>? DateChanged;
        public AppointmentModel Appointment => _appointment; 
        public string? Subject
        {
            get => _appointment.Subject;
            set
            {
                _appointment.Subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        public string PatientName => _patientName;
      
        
        public DateOnly Date
        {
            get => _appointment.Date;
            set
            {
                var old = _appointment.Date;
                _appointment.Date = value;
                DateChanged?.Invoke(this, old);
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(StartDateTime));
            }
        }
        public TimeOnly StartTime
        {
            get => _appointment.StartTime;
            set
            {
                var max = EndTime;
                _appointment.StartTime = (value > max) ? max : value;
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(StartDateTime));
            }
        }

        public TimeOnly EndTime
        {
            get => _appointment.EndTime;
            set
            {
                var min = StartTime;
                _appointment.EndTime = (value < min) ? min : value;
                OnPropertyChanged(nameof(EndTime));
                OnPropertyChanged(nameof(Duration));
            }
        }

        public DateTime StartDateTime
        {
            get => new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
            set
            {
                Date = DateOnly.FromDateTime(value);
                StartTime = TimeOnly.FromDateTime(value);
                OnPropertyChanged(nameof(StartDateTime));
            }
        }


        public TimeSpan Duration
        {
            get => EndTime - StartTime;
            set
            {
                EndTime = StartTime.Add(value);
                OnPropertyChanged(nameof(Duration));
            }
        }





        public AppointmentViewModel(AppointmentModel appointment)
        {
            _appointment = appointment;
            _patient = UserPrincipal.PatientRepository.GetById(appointment.PatientId);
            _patientName = _patient.LastName + " " + _patient.FirstName;
        }



        public AppointmentViewModel(AppointmentViewModel appointmentvm)
        {
            _appointment = new AppointmentModel() { PatientId = appointmentvm._appointment.PatientId, Subject = appointmentvm.Subject, Date = appointmentvm.Date, StartTime = appointmentvm.StartTime, EndTime = appointmentvm.EndTime };
            _patient = appointmentvm._patient;
            _patientName = appointmentvm.PatientName;
        }

    }
}
