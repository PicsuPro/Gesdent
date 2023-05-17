using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace VsProject.Services
{
    public class Extensions
    {
        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
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
    }
}
