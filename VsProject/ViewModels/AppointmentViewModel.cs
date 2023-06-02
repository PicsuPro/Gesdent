using System;
using VsProject.Models;

namespace VsProject.ViewModels
{
    public class AppointmentViewModel : ViewModelBase
    {
        private readonly AppointmentModel _appointment;

        public string? Subject
        {
            get => _appointment.Subject;
            set
            {
                _appointment.Subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        public DateTime StartDateTime
        {
            get => _appointment.StartDateTime;
            set
            {
                _appointment.StartDateTime = value;
                OnPropertyChanged(nameof(StartDateTime));
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public TimeSpan Duration
        {
            get => _appointment.Duration;
            set
            {
                _appointment.Duration = value;
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public TimeOnly StartTime => TimeOnly.FromDateTime(_appointment.StartDateTime);

        public TimeOnly EndTime => StartTime.Add(_appointment.Duration);

        public AppointmentViewModel(AppointmentModel appointment)
        {
            _appointment = appointment;
        }

    }
}
