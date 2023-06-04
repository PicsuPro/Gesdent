using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VsProject.Resources.Controls
{
    /// <summary>
    /// Interaction logic for NumericArrowsTextBox.xaml
    /// </summary>
    public partial class NumericArrowsTextBox : UserControl
    {

        public int Min { get; set; } = 0;
        public int? Max { get; set; }

        public NumericArrowsTextBox()
        {
            InitializeComponent();
        }



        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textBox.Text, out int value))
            {
                textBox.Text = (Max.HasValue && value >= Max) ? Min.ToString() : (value+1).ToString() ;
                textBox.Focus();
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textBox.Text, out int value))
            {
                textBox.Text = (value > Min) ? (value - 1).ToString() : (Max.HasValue) ? Max.ToString() : Min.ToString();
                textBox.Focus();
            }
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (textBox.IsFocused && e.Key == Key.Up)
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
