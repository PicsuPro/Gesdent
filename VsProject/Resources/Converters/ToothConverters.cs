﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace VsProject.Resources.Converters
{

    public enum ToothDataType { Front, Fill, Number, Decay, ApicalReaction }
    public class ToothDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is int index) || index < 1)
                return DependencyProperty.UnsetValue;

            if (parameter == null || !(parameter is ToothDataType dataType))
                throw new ArgumentException("Invalid parameter type or value.");

            string resourceName = "tooth" + dataType.ToString() + "Data" + index.ToString("D2");
            return (Geometry)Application.Current.TryFindResource(resourceName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



}
