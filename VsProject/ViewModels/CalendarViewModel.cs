using System.Collections.ObjectModel;
using System;
using VsProject.ViewModels;
using VsProject.Views;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Xml.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using VsProject.Services;
using System.Windows;
using System.Runtime.InteropServices.JavaScript;
using VsProject.Models;
using System.Windows.Input;

namespace VsProject.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private DateTime _selectedDate = new DateTime(2023, 05, 22, 11, 34, 0);
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        private DateOnly _startDate { get; set; } = new DateOnly(2023, 05, 05);
        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateOnly _endDate { get; set; } = new DateOnly(2023, 05, 13);
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
                    new AppointmentViewModel(new AppointmentModel{ StartDateTime = new DateTime(2023, 5, 07, 08, 0, 0), Duration = new TimeSpan(00, 05, 00), Subject = "Meeting with MF" }),
                    new AppointmentViewModel(new AppointmentModel{ StartDateTime = new DateTime(2023, 5, 08, 13, 0, 0), Duration = new TimeSpan(01, 30, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ StartDateTime = new DateTime(2023, 5, 09, 14, 0, 0), Duration = new TimeSpan(02, 30, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ StartDateTime = new DateTime(2023, 5, 10, 15, 0, 0), Duration = new TimeSpan(02, 00, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ StartDateTime = new DateTime(2023, 5, 11, 08, 0, 0), Duration = new TimeSpan(01, 00, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ StartDateTime = new DateTime(2023, 5, 12, 09, 0, 0), Duration = new TimeSpan(01, 30, 00), Subject = "Do thing" }),
                    new AppointmentViewModel(new AppointmentModel{ StartDateTime = new DateTime(2023, 5, 13, 09, 0, 0), Duration = new TimeSpan(03, 00, 00), Subject = "Ya Don't say" }),
                    new AppointmentViewModel(new AppointmentModel{ StartDateTime = new DateTime(2023, 5, 14, 10, 0, 0), Duration = new TimeSpan(02, 50, 00), Subject = "Do thing" }),
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
            MessageBox.Show("WOOOW Tu as REUSIII, TUA CLICKEeee, BRAAAAVOOOO :\n" + ((AppointmentViewModel)obj).Subject);

        }




    }





}
    
