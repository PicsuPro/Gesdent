using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VsProject.Views
{
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
        public ObservableCollection<Appointment> Appointments { get; set; }
        public CalendarView()
        {
            InitializeComponent();
        


                    // Initialize the Appointments property with test data
            Appointments = new ObservableCollection<Appointment>
            {
                new Appointment { StartTime = new DateTime(2023, 5, 8, 9, 0, 0), EndTime = new DateTime(2023, 5, 8, 10, 0, 0), Subject = "Meeting with John" },
                new Appointment { StartTime = new DateTime(2023, 5, 8, 11, 0, 0), EndTime = new DateTime(2023, 5, 8, 12, 0, 0), Subject = "Lunch with Sarah" },
                new Appointment { StartTime = new DateTime(2023, 5, 8, 13, 0, 0), EndTime = new DateTime(2023, 5, 8, 14, 0, 0), Subject = "Phone call with Alex" },
            };
        }

            // Define the Appointments property and any other properties you need
    }

    public class Appointment
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Subject { get; set; }
    }

    public class AppointmentControl : UserControl
    {
        // Define the appearance and behavior of the AppointmentControl here
    }
    }
