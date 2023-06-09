using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VsProject.Models;
using VsProject.Services;
using VsProject.ViewModels;

namespace VsProject.Resources.Controls
{

    public partial class AppointmentScheduler : UserControl
    {

        public static readonly DependencyProperty AppointmenEditCommandProperty =
              DependencyProperty.Register(
                  "AppointmentEditCommand",
                  typeof(ICommand),
                  typeof(AppointmentScheduler),
                  new PropertyMetadata(null));

        public ICommand AppointmentEditCommand
        {
            get { return (ICommand)GetValue(AppointmenEditCommandProperty); }
            set { SetValue(AppointmenEditCommandProperty, value); }

        }


        public static readonly DependencyProperty AppointmentRemoveCommandProperty =
              DependencyProperty.Register(
                  "AppointmentRemoveCommand",
                  typeof(ICommand),
                  typeof(AppointmentScheduler),
                  new PropertyMetadata(null));

        public ICommand AppointmentRemoveCommand
        {
            get { return (ICommand)GetValue(AppointmentRemoveCommandProperty); }
            set { SetValue(AppointmentRemoveCommandProperty, value); }

        }

        public static readonly DependencyProperty HourCountProperty =
        DependencyProperty.Register(
            "HourCount",
            typeof(int),
            typeof(AppointmentScheduler),
            new PropertyMetadata(5, OnHourCountPropertyChanged));
        public static void OnHourCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var calendarView = (AppointmentScheduler)d;
            calendarView.HourCount = (int)e.NewValue;
        }
        public int HourCount
        {
            get { return (int)GetValue(HourCountProperty); }
            set
            {
                SetValue(HourCountProperty, value);
                UpdateHoursAndColumns();
            }
        }

        public static readonly DependencyProperty StartHourProperty =
        DependencyProperty.Register(
            "StartHour",
            typeof(TimeOnly),
            typeof(AppointmentScheduler),
            new PropertyMetadata(new TimeOnly(7, 0), OnStartHourPropertyChanged));
        public static void OnStartHourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (AppointmentScheduler)d;
            calendarView.StartHour = (TimeOnly)e.NewValue;

        }
        public TimeOnly StartHour
        {
            get { return (TimeOnly)GetValue(StartHourProperty); }
            set
            {
                SetValue(StartHourProperty, value);
                UpdateHoursAndColumns();
            }
        }

        public static readonly DependencyProperty StartDateProperty =
        DependencyProperty.Register(
            "StartDate",
            typeof(DateOnly),
            typeof(AppointmentScheduler), new PropertyMetadata(new DateOnly(), OnStartDatePropertyChanged));
        public static void OnStartDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (AppointmentScheduler)d;
            calendarView.StartDate = (DateOnly)e.NewValue;

        }
        public DateOnly StartDate
        {
            get { return (DateOnly)GetValue(StartDateProperty); }
            set
            {
                SetValue(StartDateProperty, value);
                if (Appointments != null)
                    DayAppointments = new DayAppointmentsCollection(Appointments, value, EndDate);
            }
        }

        public static readonly DependencyProperty EndDateProperty =
        DependencyProperty.Register(
            "EndDate",
            typeof(DateOnly),
            typeof(AppointmentScheduler), new PropertyMetadata(new DateOnly(), OnEndDatePropertyChanged));
        public static void OnEndDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (AppointmentScheduler)d;
            calendarView.EndDate = (DateOnly)e.NewValue;

        }
        public DateOnly EndDate
        {
            get { return (DateOnly)GetValue(EndDateProperty); }
            set
            {
                SetValue(EndDateProperty, value);
                if (Appointments != null)
                    DayAppointments = new DayAppointmentsCollection(Appointments, StartDate, value);

            }
        }

        public static readonly DependencyProperty AppointmentsProperty =
        DependencyProperty.Register(
            "Appointments",
            typeof(ObservableCollection<AppointmentViewModel>),
            typeof(AppointmentScheduler), new PropertyMetadata(null, OnAppointmentsPropertyChanged));
        public static void OnAppointmentsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = (AppointmentScheduler)d;
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

        public List<string>? HourList { get; set; }
        public Brush AppointmentColor
        {
            get => new SolidColorBrush(GetNextCoolColor());
        }

        private double colorIndex = 0;
        private Color GetNextCoolColor()
        {
            Color color = coolColors[(int)colorIndex];
            colorIndex = (colorIndex + 0.5) % coolColors.Length; // Increment index and loop back if necessary
            return color;
        }

        private Color[] coolColors = new Color[]
        {
            Colors.LimeGreen,
            Colors.DodgerBlue,
            Colors.Gold,
            Colors.DarkOrange,
            Colors.DarkMagenta,
            Colors.Turquoise,
            Colors.Red,
            Colors.Navy,
            Colors.DarkViolet,
            Colors.Cyan,
            Colors.Gray,
            Colors.CornflowerBlue
        };


        public AppointmentScheduler()
        {
            InitializeComponent();
            UpdateHoursAndColumns();
        }

        private void UpdateHoursAndColumns()
        {
            HourList = Enumerable.Range(0, HourCount + 1)
            .Select(i => StartHour.AddHours(i).ToString(DateTimeFormatInfo.CurrentInfo.ShortTimePattern).Replace(":00", ""))
            .ToList();
            if (CustomDataGrid != null)
            {
                CustomDataGrid.Columns.Clear();
                var rowHeaderHeight = (double)FindResource("RowHeaderHeight");
                CustomDataGrid.Columns.Add(new DataGridTemplateColumn { Header = HourList[0], MinWidth = rowHeaderHeight, Width = rowHeaderHeight });

                foreach (var header in HourList.Skip(1))
                {
                    CustomDataGrid.Columns.Add(new DataGridTemplateColumn { Header = header });
                }

            }
        }

        private Grid? popupGrid;
        private TextBlock? popupTextBlock;
        private Point dragStartPoint;
        private FrameworkElement? draggedItem;
        private Canvas? draggingCanvas;
        AppointmentViewModel? appointment;
        bool hasBeenDragged;
        //public static HashSet<AppointmentViewModel> ProcessedAppointments { get; } = new HashSet<AppointmentViewModel>();

        private void Appointment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid grid)
            {
                if (popupGrid != null)
                    popupGrid.Visibility = Visibility.Visible;
                draggedItem = (FrameworkElement?)VisualTreeHelper.GetParent(grid);
                dragStartPoint = e.GetPosition(draggedItem);
                draggingCanvas = (Canvas?)VisualTreeHelper.GetParent(draggedItem);
                appointment = (AppointmentViewModel?)draggedItem?.DataContext;
                DragDrop.DoDragDrop(draggedItem, appointment, DragDropEffects.Move);
            }
            if (popupGrid != null)
                popupGrid.Visibility = Visibility.Collapsed;
            draggedItem = null;
            draggingCanvas = null;
            appointment = null;
            dragStartPoint = default;
            hasBeenDragged = false;

        }

      

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (appointment != null && draggedItem != null && draggingCanvas != null && popupGrid != null && popupTextBlock != null)
            {
                hasBeenDragged = true;
                (TimeOnly startTime, TimeSpan duration) = GetStartTimeAndDurationFromPosition(e.GetPosition(draggingCanvas), draggingCanvas.ActualHeight, draggedItem.ActualHeight, dragStartPoint);
                appointment.StartTime = new TimeOnly(startTime.Hour, startTime.Minute, startTime.Second);
                appointment.Duration = duration;
                Canvas.SetTop(popupGrid, Canvas.GetTop(draggedItem) - popupGrid.ActualHeight / 2);
                popupTextBlock.Text = appointment.StartTime.ToString();
            }
        }

     
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (appointment != null && draggedItem != null && draggingCanvas != null)
            {
                if (sender is Canvas canvas)
                {

                    (TimeOnly startTime, TimeSpan duration) = GetStartTimeAndDurationFromPosition(e.GetPosition(draggingCanvas), draggingCanvas.ActualHeight, draggedItem.ActualHeight, dragStartPoint);
                    KeyValuePair<DateOnly, ObservableCollection<AppointmentViewModel>> appointmentsKeyValuePair = ((KeyValuePair<DateOnly, ObservableCollection<AppointmentViewModel>>)canvas.DataContext);
                    DateOnly newDay = appointmentsKeyValuePair.Key;
                    if (newDay != appointment.Date)
                    {
                        appointment.StartDateTime = new DateTime(newDay.Year, newDay.Month, newDay.Day, startTime.Hour, startTime.Minute, startTime.Second);
                        appointment.Duration = duration;
                    }

                    var availableWidth = canvas.ActualWidth;
                    ObservableCollection<AppointmentViewModel> appointments = appointmentsKeyValuePair.Value;

                    //    //ProcessedAppointments.Add(appointment);
                    //    int index = appointments.IndexOf(appointment);
                    //    List<int> collisionIndexes = new List<int>();
                    //    for (int i = 0; i < appointments.Count; i++)
                    //    {
                    //        if (CheckCollision(appointment, appointments[i]))
                    //        {
                    //            collisionIndexes.Add(i);
                    //        }
                    //    }
                    //    MessageBox.Show(collisionIndexes.Count.ToString());
                    //    var appointmentWidth = availableWidth / (collisionIndexes.Count);
                    //    double appointmentleft = appointmentWidth * GetSortedPlace(index, collisionIndexes);
                    //    draggedItem.Width = appointmentWidth;
                    //    Canvas.SetLeft(draggedItem, appointmentleft);
                }
            }
        }

        private (TimeOnly, TimeSpan) GetStartTimeAndDurationFromPosition(Point position, double canvasHeight, double itemHeight, Point startPoint)
        {
            double top = Math.Clamp(position.Y - startPoint.Y, 0, canvasHeight - itemHeight);
            var columnHeight = canvasHeight / HourCount;
            var hours = (top / columnHeight) + StartHour.Hour;
            var minutes = (hours * 60) % 60;
            var startTime = new TimeOnly((int)hours, (int)minutes, 0);

            var totalMinutes = itemHeight / columnHeight * 60.0;
            var duration = TimeSpan.FromMinutes(totalMinutes);
            return (startTime, duration);
        }

        private void PopupGrid_Loaded(object sender, RoutedEventArgs e)
        {
            popupGrid = (Grid)sender;
            popupTextBlock = Extensions.FindVisualChild<TextBlock>(popupGrid);
        }
        public static int GetSortedPlace(int number, List<int> list)
        {
            // Step 1: Create a copy of the list
            List<int> tempList = new List<int>(list);

            // Step 2: Sort the list in ascending order
            tempList.Sort();

            // Step 3: Find the index of the number in the sorted list
            int placeInOrder = tempList.IndexOf(number);

            // Step 4: Return the sorted place
            return placeInOrder;
        }

    }

}
