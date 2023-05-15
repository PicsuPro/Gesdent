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
            if (values[0] is TimeOnly startTime && values[1] is double canvasHeight)
            {
                var columnHeight = canvasHeight / (21 - 7);
                double top = (startTime.Hour - 7) * columnHeight + (startTime.Minute / 60.0) * columnHeight; // Convert the time difference to the corresponding distance in pixels
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
            if (values[0] is TimeSpan duration && values[1] is double canvasHeight)
            {
                var columnHeight = canvasHeight / (21 - 7);
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
    
    public class DataGridWidthToRowWidthConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dataGridWidth && parameter is double columnHeaderWidth)
            {
                return (dataGridWidth - columnHeaderWidth -2) / 7;

            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double rowWidth && parameter is double columnHeaderWidth)
            {
                return ((rowWidth * 7) + columnHeaderWidth);

            }
            return Binding.DoNothing;
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

    public class IntToDayStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.DayNames[i]);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }




}
