﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VsProject.Models;
using VsProject.Services;

namespace VsProject.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {

        private DateOnly _startDate { get; set; } = new DateOnly(2023, 06, 04);
        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateOnly _endDate { get; set; } = new DateOnly(2023, 06, 11);
        public DateOnly EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        private TimeOnly _startHour { get; set; } = new TimeOnly(6, 0);
        public TimeOnly StartHour
        {
            get => _startHour;
            set
            {
                _startHour = value;
                OnPropertyChanged(nameof(StartHour));
            }
        }
        private int _hourCount { get; set; } = 14;
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


        private ObservableCollection<AppointmentViewModel> _appointments =
            new ObservableCollection<AppointmentViewModel>
                {
                    new AppointmentViewModel(new AppointmentModel{ Date  = new DateOnly(2023, 06, 04), StartTime = new TimeOnly(08, 0, 0), EndTime = new TimeOnly(08, 05, 00), Subject = "Meeting with MF" }),
                    new AppointmentViewModel(new AppointmentModel{ Date  = new DateOnly(2023, 06, 05), StartTime = new TimeOnly(13, 0, 0), EndTime = new TimeOnly(14, 30, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ Date  = new DateOnly(2023, 06, 06), StartTime = new TimeOnly(14, 0, 0), EndTime = new TimeOnly(16, 30, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ Date  = new DateOnly(2023, 06, 07), StartTime = new TimeOnly(15, 0, 0), EndTime = new TimeOnly(16, 00, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ Date  = new DateOnly(2023, 06, 08), StartTime = new TimeOnly(08, 0, 0), EndTime = new TimeOnly(09, 00, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ Date  = new DateOnly(2023, 06, 09), StartTime = new TimeOnly(09, 0, 0), EndTime = new TimeOnly(10, 30, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ Date  = new DateOnly(2023, 06, 10), StartTime = new TimeOnly(09, 0, 0), EndTime = new TimeOnly(12, 00, 00), Subject = "Ya Don't say" }),
                    new AppointmentViewModel(new AppointmentModel{ Date  = new DateOnly(2023, 06, 11), StartTime = new TimeOnly(10, 0, 0), EndTime = new TimeOnly(12, 50, 00), Subject = "Do thing" }),
                };


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

        public CalendarViewModel()
        {
            AppointmentEditCommand = new ViewModelCommand(ExecuteAppointmentEdit);
        }

        private void ExecuteAppointmentEdit(object obj)
        {
            if(obj is AppointmentViewModel appointment) 
            {
                var old = new AppointmentViewModel(appointment);
                if (DialogService.Show(new AppointmentEditViewModel(appointment)) == false)
                {
                    appointment.Subject = old.Subject;
                    appointment.Date = old.Date;
                    appointment.StartTime = old.StartTime;
                    appointment.EndTime = old.EndTime;
                }
            }

        }




    }





}

