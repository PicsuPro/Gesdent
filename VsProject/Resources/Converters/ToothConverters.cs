using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace VsProject.Resources.Converters
{
    public enum ToothDataType { Front, Top, FrontFill, TopFill }
    class ToothDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is int index) || index < 1)
                return DependencyProperty.UnsetValue;

            if (parameter == null || !(parameter is ToothDataType dataType))
                throw new ArgumentException("Invalid parameter type or value.");

            string resourceName = "tooth" + dataType.ToString() + "Data" + index;
            return (Geometry)Application.Current.TryFindResource(resourceName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ToothIndexToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is int index) || index < 1)
                return DependencyProperty.UnsetValue;
            if(index  <= 16)
                return (index - 1);
            else 
                return 15 - (index -  17);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ToothIndexToRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is int index) || index < 1)
                return DependencyProperty.UnsetValue;
            if (parameter != null && parameter is ToothDataType dataType)
            {
                switch (dataType)
                {
                    case ToothDataType.Top:
                        return ((index - 1) / 16) == 0 ? 3 : 0;
                    case ToothDataType.Front:
                        return 1;
                    default:
                        return 0;
                }
            }
            else
                return ((index - 1) / 16) == 0 ? 0 : 3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ToothIndexToVerticalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is int index) || index < 1)
                return DependencyProperty.UnsetValue;

            return  (VerticalAlignment)((index - 1) / 16) == 0 ? VerticalAlignment.Bottom : VerticalAlignment.Top; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
