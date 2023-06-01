using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace VsProject.Resources.Controls
{
    public class ArrowsTextBox : TextBox
    {
        static ArrowsTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ArrowsTextBox), new FrameworkPropertyMetadata(typeof(ArrowsTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var increaseButton = GetTemplateChild("PART_IncreaseButton") as Button;
            var decreaseButton = GetTemplateChild("PART_DecreaseButton") as Button;

            if (increaseButton != null && decreaseButton != null)
            {
                increaseButton.Click += Increase_Click;
                decreaseButton.Click += Decrease_Click;
            }
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Text, out int value))
            {
                value++;
                Text = value.ToString();
                Focus();
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Text, out int value))
            {
                value--;
                Text = value.ToString();
                Focus();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (IsFocused && e.Key == Key.Up)
            {
                Increase_Click(this, null);
                e.Handled = true;
            }
            else if (IsFocused && e.Key == Key.Down)
            {
                Decrease_Click(this, null);
                e.Handled = true;
            }
        }
    }
}
