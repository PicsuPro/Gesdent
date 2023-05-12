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

   

}
