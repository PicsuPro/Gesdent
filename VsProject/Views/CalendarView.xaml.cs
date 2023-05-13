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
using VsProject.ViewModels;

namespace VsProject.Views
{
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {


        static readonly TimeOnly startHour = new TimeOnly(7, 0); // SetUser the start time to 7:00 
        static readonly TimeOnly endHour = new TimeOnly(20, 0); // SetUser the start time to 20:00 
        public List<string> HourList = Enumerable.Range(0, (int)(endHour - startHour).TotalHours + 1)
                .Select(i => startHour.AddHours(i).ToString(DateTimeFormatInfo.CurrentInfo.ShortTimePattern).Replace(":00", ""))
                .ToList();
            public string[] WeekDaysList = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;

        public CalendarView()
        {
            InitializeComponent();
            foreach (var hour in HourList)
            {
                CustomDataGrid.Columns.Add(new DataGridTextColumn() { Header = hour });
            }

        }

        private Point dragStartPoint;
        private FrameworkElement? draggedItem;
        private Canvas? draggingCanvas;
        Appointment? appointment;

        private void Appointment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                draggedItem = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
                dragStartPoint = e.GetPosition(draggedItem);
                draggingCanvas = VisualTreeHelper.GetParent(draggedItem) as Canvas;
                appointment = draggedItem.DataContext as Appointment;
                DragDrop.DoDragDrop(draggedItem, appointment, DragDropEffects.Move);
            }

        }


        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            Point position = e.GetPosition(draggingCanvas);
            double canvasHeight = draggingCanvas.ActualHeight;
            double top = Math.Clamp(position.Y - dragStartPoint.Y, 0, canvasHeight - draggedItem.ActualHeight);
            var columnHeight = canvasHeight / (21 - 7);
            var hours = (top / columnHeight) + 7;
            var minutes = (hours* 60) % 60;
            var startTime = new TimeOnly((int)hours, (int)minutes, 0);
            var totalMinutes = draggedItem.ActualHeight / columnHeight * 60.0;
            var endTime = startTime.AddMinutes(totalMinutes) ;
            appointment.StartTimeOnly = startTime;
            appointment.EndTimeOnly = endTime;

        }


        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            draggedItem = null;
            draggingCanvas = null;
            appointment = null;
            dragStartPoint = default;
        }
    }

}
