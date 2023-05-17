using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
using VsProject.Models;
using VsProject.Resources.Controls;
using VsProject.Services;
using VsProject.ViewModels;

namespace VsProject.Views
{
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
      
        public static readonly DependencyProperty HourCountProperty =
        DependencyProperty.Register(
            "HourCount",
            typeof(int),
            typeof(CalendarView),
            new PropertyMetadata(14, OnHourCountPropertyChanged));
        public static void OnHourCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (CalendarView)d;
            calendarView.HourCount = (int)e.NewValue;
        }
        public int HourCount
        {
            get { return (int)GetValue(HourCountProperty); }
            set 
            { 
                SetValue(HourCountProperty, value);
                HourList = Enumerable.Range(0, HourCount)
                .Select(i => StartHour.AddHours(i).ToString(DateTimeFormatInfo.CurrentInfo.ShortTimePattern).Replace(":00", ""))
                .ToList();
            }
        }


        public static readonly DependencyProperty StartHourProperty =
        DependencyProperty.Register(
            "StartHour",
            typeof(TimeOnly),
            typeof(CalendarView),
            new PropertyMetadata(new TimeOnly(7, 0), OnStartHourPropertyChanged));
        public static void OnStartHourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (CalendarView)d;
            calendarView.StartHour = (TimeOnly)e.NewValue;

        }

        public TimeOnly StartHour
        {
            get { return (TimeOnly)GetValue(StartHourProperty); }
            set 
            { 
                SetValue(StartHourProperty, value);
                HourList = Enumerable.Range(0, HourCount)
                .Select(i => StartHour.AddHours(i).ToString(DateTimeFormatInfo.CurrentInfo.ShortTimePattern).Replace(":00", ""))
                .ToList();
            }
        }

        public static readonly DependencyProperty StartDateProperty =
        DependencyProperty.Register(
            "StartDate",
            typeof(DateOnly),
            typeof(CalendarView), new PropertyMetadata(new DateOnly(), OnStartDatePropertyChanged));
        public static void OnStartDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (CalendarView)d;
            calendarView.StartDate = (DateOnly)e.NewValue;

        }
        public DateOnly StartDate
        {
            get { return (DateOnly)GetValue(StartDateProperty); }
            set 
            { 
                SetValue(StartDateProperty, value);
                if(Appointments != null)
                DayAppointments = new DayAppointmentsCollection(Appointments, value, EndDate);
            }
        }
        
        public static readonly DependencyProperty EndDateProperty =
        DependencyProperty.Register(
            "EndDate",
            typeof(DateOnly),
            typeof(CalendarView), new PropertyMetadata(new DateOnly(), OnEndDatePropertyChanged));
        public static void OnEndDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (CalendarView)d;
            calendarView.EndDate = (DateOnly)e.NewValue;

        }
        public DateOnly EndDate
        {
            get { return (DateOnly)GetValue(EndDateProperty); }
            set 
            { 
                SetValue(EndDateProperty, value);
                if (Appointments != null)
                    DayAppointments = new DayAppointmentsCollection(Appointments,StartDate , value);

            }
        }
        
        public static readonly DependencyProperty AppointmentsProperty =
        DependencyProperty.Register(
            "Appointments",
            typeof(ObservableCollection<AppointmentViewModel>),
            typeof(CalendarView), new PropertyMetadata(null, OnAppointmentsPropertyChanged));
        public static void OnAppointmentsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (CalendarView)d;
            calendarView.Appointments = (ObservableCollection<AppointmentViewModel>)e.NewValue;

        }
        public ObservableCollection<AppointmentViewModel> Appointments
        {
            get { return (ObservableCollection<AppointmentViewModel>)GetValue(AppointmentsProperty); }
            set 
            { 
                SetValue(AppointmentsProperty, value);
                DayAppointments = new DayAppointmentsCollection(value, StartDate, EndDate);
            }
        }


        private DayAppointmentsCollection? _dayAppointments;
        public DayAppointmentsCollection? DayAppointments 
        { 
            get => _dayAppointments;
            set 
            {
                _dayAppointments = value;
                
            } 
        }

        public List<string> HourList { get; set; }

        public CalendarView()
        {
            HourList = Enumerable.Range(0, HourCount)
                .Select(i => StartHour.AddHours(i).ToString(DateTimeFormatInfo.CurrentInfo.ShortTimePattern).Replace(":00", ""))
                .ToList();
            InitializeComponent();
        }



        private Point dragStartPoint;
        private FrameworkElement? draggedItem;
        private Canvas? draggingCanvas;
        AppointmentViewModel? appointment;

        private void Appointment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                draggedItem = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
                dragStartPoint = e.GetPosition(draggedItem);
                draggingCanvas = VisualTreeHelper.GetParent(draggedItem) as Canvas;
                appointment = draggedItem?.DataContext as AppointmentViewModel;
                DragDrop.DoDragDrop(draggedItem, appointment, DragDropEffects.Move);
            }

        }


        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            (TimeOnly startTime, TimeSpan duration) = GetStartTimeAndDurationFromPosition(e.GetPosition(draggingCanvas));
            appointment.StartDateTime = new DateTime(appointment.StartDateTime.Year, appointment.StartDateTime.Month, appointment.StartDateTime.Day, startTime.Hour, startTime.Minute, startTime.Second);
            appointment.Duration = duration;
        }


        private void Canvas_Drop(object sender, DragEventArgs e)
        {

            Canvas canvas = (Canvas)sender;
            if (canvas != draggingCanvas)
            {
                DayAppointmentsCollection dayAppointments = (DayAppointmentsCollection)CustomDataGrid.ItemsSource;
                dayAppointments.AppointmentList.Remove(appointment);

                (TimeOnly startTime,TimeSpan duration) = GetStartTimeAndDurationFromPosition(e.GetPosition(draggingCanvas)); 
                DateOnly newDay = ((KeyValuePair<DateOnly, ObservableCollection<AppointmentViewModel>>)canvas.DataContext).Key;

                appointment.StartDateTime = new DateTime(newDay.Year, newDay.Month, newDay.Day, startTime.Hour, startTime.Minute, startTime.Second);
                appointment.Duration = duration;
                dayAppointments.AppointmentList.Add(appointment);
            }
            draggedItem = null;
            draggingCanvas = null;
            appointment = null;
            dragStartPoint = default;
           

        }



        private (TimeOnly, TimeSpan) GetStartTimeAndDurationFromPosition(Point position)
        {
            double canvasHeight = draggingCanvas.ActualHeight;
            double top = Math.Clamp(position.Y - dragStartPoint.Y, 0, canvasHeight - draggedItem.ActualHeight);
            var columnHeight = canvasHeight / (HourCount);
            var hours = (top / columnHeight) + StartHour.Hour;
            var minutes = (hours * 60) % 60;
            var startTime = new TimeOnly((int)hours, (int)minutes, 0);

            var totalMinutes = draggedItem.ActualHeight / columnHeight * 60.0;
            var duration = TimeSpan.FromMinutes(totalMinutes);

            return (startTime, duration);
        }
    }

}
