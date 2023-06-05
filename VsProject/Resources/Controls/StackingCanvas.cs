using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VsProject.Resources.Controls
{

    public class StackingCanvas : Canvas
    {
        public StackingCanvas()
        {
            
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Size size = base.ArrangeOverride(arrangeSize);
            StackCollidingItems();
            return size;
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            HookChildPropertyChanges(visualAdded as UIElement);
            StackCollidingItems(); // Update stacking when children are added or removed
        }

        private void HookChildPropertyChanges(UIElement child)
        {
            if (child != null)
            {
                DependencyPropertyDescriptor leftDescriptor = DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(Canvas));
                if (leftDescriptor != null)
                {
                    leftDescriptor.AddValueChanged(child, OnChildPropertyChange);
                }
                DependencyPropertyDescriptor widthDescriptor = DependencyPropertyDescriptor.FromProperty(FrameworkElement.WidthProperty, typeof(FrameworkElement));
                if (widthDescriptor != null)
                {
                    widthDescriptor.AddValueChanged(child, OnChildPropertyChange);
                }
            }
        }

        private void OnChildPropertyChange(object sender, EventArgs e)
        {
            StackCollidingItems((FrameworkElement)sender); // Update stacking when child properties change
        }

        private void StackCollidingItems()
        {
            List<FrameworkElement> items = InternalChildren.Cast<FrameworkElement>().ToList();
            double availableWidth = ActualWidth;

            foreach (FrameworkElement item in items)
            {
                int collidingNumber = 1;

                foreach (FrameworkElement otherItem in items)
                {
                    if (otherItem != item && CheckCollision(item, otherItem))
                    {
                        collidingNumber++;
                    }
                }

                // Set the left position and width of the item
                if (item != null)
                {
                    Debug.WriteLine(collidingNumber.ToString());
                    Canvas.SetLeft(item, 0);
                    if (availableWidth > 0)
                        item.Width = availableWidth / collidingNumber;
                    else item.Width = 50 ;
                }
            }
        }


        
        private void StackCollidingItems(FrameworkElement item)
        {
            double availableWidth = ActualWidth;
            List<FrameworkElement> items = InternalChildren.Cast<FrameworkElement>().ToList();
            int collidingNumber = 1;

            foreach (FrameworkElement otherItem in items)
            {
                if (otherItem != item && CheckCollision(item, otherItem))
                {
                    collidingNumber++;
                }
            }

                // Set the left position and width of the item
            if (item != null)
            {
                if (collidingNumber == 0)
                {
                    Canvas.SetLeft(item, 0);
                }
                    item.Width = availableWidth / collidingNumber;
            }
        }



        private bool CheckCollision(FrameworkElement? item1, FrameworkElement? item2)
        {
            if(item1 == null || item2 == null) return false;
            double top1 = Canvas.GetTop(item1);
            double bottom1 = top1 + item1.RenderSize.Height;
            double top2 = Canvas.GetTop(item2);
            double bottom2 = top2 + item2.RenderSize.Height;

            return bottom1 > top2 && top1 < bottom2;
        }

    }



}
