using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

    public class AppointmentWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4 || !(values[0] is double actualWidth) || !(values[1] is double actualHeight) || !(values[2] is UIElementCollection children) || !(values[3] is FrameworkElement element))
            {
                return Binding.DoNothing;
            }

            double availableWidth = actualWidth;
            int stackedElements = 0;

            foreach (FrameworkElement child in children)
            {
                if (child != element && CheckCollision(actualHeight, Canvas.GetTop(child) + child.ActualHeight, actualHeight, Canvas.GetTop(element) + actualHeight))
                {
                    stackedElements++;
                    double right = Canvas.GetLeft(child) + child.ActualWidth;
                    if (right > availableWidth)
                    {
                        availableWidth = right;
                    }
                }
            }

            double width = availableWidth / (stackedElements + 1);

            return width;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private bool CheckCollision(double top1, double bottom1, double top2, double bottom2)
        {
            return bottom1 > top2 && top1 < bottom2;
        }
    }

    public class AppointmentLeftConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4 || !(values[0] is double actualWidth) || !(values[1] is double actualHeight) || !(values[2] is double top) || !(values[3] is UIElement element) || !(values[4] is UIElementCollection children))
            {
                return Binding.DoNothing;
            }

            double availableWidth = actualWidth;
            int stackedElements = 0;

            foreach (UIElement child in children)
            {
                if (child != element && CheckCollision(top, actualHeight, child))
                {
                    stackedElements++;
                    double right = Canvas.GetLeft(child) + child.RenderSize.Width;
                    if (right > availableWidth)
                    {
                        availableWidth = right;
                    }
                }
            }

            double elementWidth = availableWidth / (stackedElements + 1);

            return elementWidth;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private bool CheckCollision(double top1, double bottom1, UIElement item2)
        {
            double top2 = Canvas.GetTop(item2);
            double bottom2 = top2 + item2.RenderSize.Height;

            return bottom1 > top2 && top1 < bottom2;
        }
    }






}
