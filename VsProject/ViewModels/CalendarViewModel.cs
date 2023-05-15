using System.Collections.ObjectModel;
using System;
using VsProject.ViewModels;
using VsProject.Views;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Collections;

namespace VsProject.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {

        public Dictionary<int, ObservableCollection<Appointment>> WeekAppointments { get; set; } = new Dictionary<int, ObservableCollection<Appointment>>
        {
            { 0, new ObservableCollection<Appointment>
                {
                    new Appointment { StartTime = new DateTime(2023, 5, 7, 19, 0, 0), EndTime = new DateTime(2023, 5, 7, 21, 0, 0), Subject = "Meeting with MF" },
                    new Appointment { StartTime = new DateTime(2023, 5, 8, 13, 0, 0), EndTime = new DateTime(2023, 5, 8, 15, 30, 0), Subject = "Do thing" },
                }
            },
            { 1, new ObservableCollection<Appointment>
                {
                    new Appointment { StartTime = new DateTime(2023, 5, 9, 9, 0, 0), EndTime = new DateTime(2023, 5, 9, 10, 0, 0), Subject = "Meeting with John" },
                    new Appointment { StartTime = new DateTime(2023, 5, 9, 8, 0, 0), EndTime = new DateTime(2023, 5, 9, 12, 0, 0), Subject = "Lunch with Sarah" },
                    new Appointment { StartTime = new DateTime(2023, 5, 9, 13, 0, 0), EndTime = new DateTime(2023, 5, 9, 14, 0, 0), Subject = "Phone call with Alex" },
                }
            },
            { 2, new ObservableCollection<Appointment>
                {
                    new Appointment { StartTime = new DateTime(2023, 5, 10, 9, 0, 0), EndTime = new DateTime(2023, 5, 10, 10, 0, 0), Subject = "Meeting with John" },
                    new Appointment { StartTime = new DateTime(2023, 5, 10, 10, 0, 0), EndTime = new DateTime(2023, 5, 10, 12, 0, 0), Subject = "Lunch with Sarah" },
                    new Appointment { StartTime = new DateTime(2023, 5, 10, 13, 0, 0), EndTime = new DateTime(2023, 5, 10, 14, 0, 0), Subject = "Phone call with Alex" },
                }
            },
            { 3,new ObservableCollection<Appointment>
                {
                    new Appointment { StartTime = new DateTime(2023, 5, 8, 9, 0, 0), EndTime = new DateTime(2023, 5, 8, 10, 0, 0), Subject = "Meeting with John" },
                    new Appointment { StartTime = new DateTime(2023, 5, 8, 13, 0, 0), EndTime = new DateTime(2023, 5, 8, 15, 30, 0), Subject = "Phone call with BITCH" },
                }
            },
            { 4,new ObservableCollection<Appointment>
                {
                    new Appointment { StartTime = new DateTime(2023, 5, 8, 7, 0, 0), EndTime = new DateTime(2023, 5, 8, 8, 0, 0), Subject = "Meeting with MF" },
                }
            },
            {5, new ObservableCollection<Appointment>
                {
                    new Appointment { StartTime = new DateTime(2023, 5, 8, 9, 0, 0), EndTime = new DateTime(2023, 5, 8, 10, 0, 0), Subject = "Meeting with John" },
                }
            },
            {6, new ObservableCollection<Appointment>
                {
                    new Appointment { StartTime = new DateTime(2023, 5, 8, 7, 0, 0), EndTime = new DateTime(2023, 5, 8, 8, 0, 0), Subject = "Meeting with Last" },
                    new Appointment { StartTime = new DateTime(2023, 5, 8, 13, 0, 0), EndTime = new DateTime(2023, 5, 8, 15, 30, 0), Subject = "Do thing" },
                } 
            }
        };
       
    }

    public class ApointmentList<Appointment> : IEnumerable<Appointment>
    {
        public IEnumerator<Appointment> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class Appointment : ViewModelBase
        {
            public DateTime StartTime { get; set; }
            public TimeOnly StartTimeOnly 
            { 
                get => TimeOnly.FromDateTime(StartTime); set 
                {
                    StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, value.Hour, value.Minute, value.Second);
                    OnPropertyChanged(nameof(StartTime));
                    OnPropertyChanged(nameof(StartTimeOnly));
                }
            }
            public DateTime EndTime { get; set; }
            public TimeOnly EndTimeOnly
            {
                get => TimeOnly.FromDateTime(EndTime); set
                {
                    EndTime = new DateTime(EndTime.Year, EndTime.Month, EndTime.Day, value.Hour, value.Minute, value.Second);
                    OnPropertyChanged(nameof(EndTime));
                    OnPropertyChanged(nameof(EndTimeOnly));
                }
            }
            public TimeSpan Duration => EndTime - StartTime;
            public string? Subject { get; set; }
        }

       

}