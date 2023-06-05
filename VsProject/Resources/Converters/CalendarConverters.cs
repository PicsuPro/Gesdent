using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VsProject.ViewModels;

namespace VsProject.Resources.Converters
{


    public class StartTimeToTopConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is TimeOnly startTime && values[1] is double canvasHeight && values[2] is int hourCount && values[3] is TimeOnly startHour)
            {

                var columnHeight = canvasHeight / (hourCount);
                double top = (startTime.Hour - startHour.Hour) * columnHeight + (startTime.Minute / 60.0) * columnHeight; // Convert the time difference to the corresponding distance in pixels
                return top;
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class DurationToHeightConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is TimeSpan duration && values[1] is double canvasHeight && values[2] is int hourCount)
            {
                var columnHeight = canvasHeight / (hourCount);
                var height = (duration.TotalMinutes * columnHeight) / 60.0;
                return height;

            }
            return Binding.DoNothing;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DataGridWidthToRowWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int columnCount && values[1] is double rowPresenterWidth)
            {
                return (rowPresenterWidth - 2) / columnCount;

            }
            return Binding.DoNothing;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class ExcludeFirstItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<object> items)
            {
                return items.Skip(1);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class DateToDayOfWeekStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly date)
            {
                return culture.TextInfo.ToTitleCase(date.ToString("dddd"));
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class HeightToBottomMarginHalfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                return new Thickness(0, height / 2, 0, -height / 2);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    public class AppointmentLeftConverter : IMultiValueConverter
    {
        public static HashSet<AppointmentViewModel> ProcessedAppointments { get; } = new HashSet<AppointmentViewModel>();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double availableWidth && values[1] is TimeOnly startTime && values[2] is TimeOnly endTime && values[3] is AppointmentViewModel appointment && values[4] is ObservableCollection<AppointmentViewModel> appointments)
            {
                ProcessedAppointments.Add(appointment);
                List<int> collisionIndexes = new List<int>();
                int index = appointments.IndexOf(appointment);

                for (int i = 0; i < appointments.Count; i++)
                {
                    if (i != index && !ProcessedAppointments.Contains(appointments[i]))
                    {
                        ProcessedAppointments.Add(appointments[i]);
                        appointments[i].OnPropertyChanged(nameof(AppointmentViewModel.EndTime));
                    }
                    if (i != index && CheckCollision(startTime, endTime, appointments[i]))
                    {
                        collisionIndexes.Add(i);
                    }
                }

                ProcessedAppointments.Remove(appointment);
                var appointmentWidth = availableWidth / (collisionIndexes.Count + 1);
                double left = appointmentWidth * GetSortedPlace(index, collisionIndexes);
                return left;
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public static int GetSortedPlace(int number, List<int> list)
        {
            // Step 1: Create a copy of the list and add the number
            List<int> tempList = new List<int>(list);
            tempList.Add(number);

            // Step 2: Sort the list in ascending order
            tempList.Sort();

            // Step 3: Find the index of the number in the sorted list
            int placeInOrder = tempList.IndexOf(number);

            // Step 4: Return the sorted place
            return placeInOrder;
        }

        private bool CheckCollision(TimeOnly startTime, TimeOnly endTime, AppointmentViewModel appointment2)
        {
            return startTime < appointment2.EndTime && appointment2.StartTime < endTime;
        }
    }

    public class AppointmentWidthConverter : IMultiValueConverter
    {
        public static HashSet<AppointmentViewModel> ProcessedAppointments { get; } = new HashSet<AppointmentViewModel>();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double availableWidth && values[1] is TimeOnly startTime && values[2] is TimeSpan duration && values[3] is AppointmentViewModel appointment && values[4] is ObservableCollection<AppointmentViewModel> appointments)
            {
                ProcessedAppointments.Add(appointment);

                int collisionNumber = 0;
                int index = appointments.IndexOf(appointment);

                for (int i = 0; i < appointments.Count; i++)
                {
                    if (i != index && !ProcessedAppointments.Contains(appointments[i]))
                    {
                        ProcessedAppointments.Add(appointments[i]);
                        appointments[i].OnPropertyChanged(nameof(AppointmentViewModel.Duration));
                    }
                    if (i != index && CheckCollision(startTime, duration, appointments[i]))
                    {
                        collisionNumber++;
                    }
                }

                ProcessedAppointments.Remove(appointment);
                var appointmentWidth = availableWidth / (collisionNumber + 1);
                return appointmentWidth;
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private bool CheckCollision(TimeOnly startTime, TimeSpan duration, AppointmentViewModel appointment2)
        {
            var endTime = startTime.Add(duration);
            return startTime < appointment2.EndTime && appointment2.StartTime < endTime;
        }
    }


}







