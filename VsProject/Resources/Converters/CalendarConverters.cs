using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VsProject.Models;
using VsProject.Resources.Controls;
using VsProject.Views;

namespace VsProject.Resources.Converters
{
 

    public class StartTimeToTopConverter :  IMultiValueConverter
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

}
