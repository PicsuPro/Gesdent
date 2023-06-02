using System.Windows;
using System.Windows.Media;

namespace VsProject.Services
{
    public static class Extensions
    {
        public static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            // Get the parent of the child
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                // If the parent is the correct type, return it
                if (parent is T)
                {
                    return parent as T;
                }

                // Get the next parent
                parent = VisualTreeHelper.GetParent(parent);
            }

            // No parent of the correct type was found
            return null;
        }

        public static T? FindVisualChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                    return typedChild;

                var foundChild = FindVisualChild<T>(child);

                if (foundChild != null)
                    return foundChild;
            }

            return null;
        }
        public static T? FindLogicalChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                return null;

            foreach (var child in LogicalTreeHelper.GetChildren(parent))
            {
                if (child is T typedChild)
                    return typedChild;

                var foundChild = FindLogicalChild<T>(child as DependencyObject);

                if (foundChild != null)
                    return foundChild;
            }

            return null;
        }

        public static int mod(this int value, int divisor)
        {
            int result = value % divisor;
            return result < 0 ? result + divisor : result;
        }

        public static long mod(this long value, long divisor)
        {
            long result = value % divisor;
            return result < 0 ? result + divisor : result;
        }

    }
}
