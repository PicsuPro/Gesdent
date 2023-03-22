using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VsProject.Resources.Controls
{

    public partial class DatePickerComboBoxes : UserControl
    {

        public static readonly DependencyProperty SelectedDateProperty =
    DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(DatePickerComboBoxes),
        new PropertyMetadata(OnSelectedDateChanged) );

        public static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePickerComboBoxes = (DatePickerComboBoxes)d;
            datePickerComboBoxes.UpdateComboBoxes();
        }
        public DateTime SelectedDate
        {
            get { 
                return (DateTime)GetValue(SelectedDateProperty); }
            set
            {
                SetValue(SelectedDateProperty, value);
            }
        }



        private static readonly List<string> months = new List<string>()
        {
            "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
            "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"
        };
        public DatePickerComboBoxes()
        {
           
            InitializeComponent();
            
            // Initialize the ComboBoxes
            DayComboBox.ItemsSource = Enumerable.Range(1, 31);
            MonthComboBox.ItemsSource = months;
            YearComboBox.ItemsSource = Enumerable.Range(1900, DateTime.Now.Year - 1900 + 1);

            UpdateComboBoxes();
        }
        private static DateTime ValidDate(int year, int month, int day)
        {

            int daysInMonth = DateTime.DaysInMonth(year, month);

            if (day > daysInMonth)
            {
                day = daysInMonth;
            }

            return new DateTime(year, month, day);
        }

        private void UpdateComboBoxes()
        {
            DayComboBox.SelectedItem = SelectedDate.Day;
            MonthComboBox.SelectedIndex = SelectedDate.Month - 1;
            YearComboBox.SelectedItem = SelectedDate.Year;

        }

        private void UpdateSelectedDate(object sender, SelectionChangedEventArgs e)
        {
            if (DayComboBox.SelectedItem == null || MonthComboBox.SelectedItem == null || YearComboBox.SelectedItem == null)
            {
                return;
            }
            int day = (int)DayComboBox.SelectedItem;
            int month = MonthComboBox.SelectedIndex + 1;
            int year = (int)YearComboBox.SelectedItem;
           

            SelectedDate = ValidDate(year, month, day);

        }

        private void DayComboBox_DropDownOpened(object sender, EventArgs e)
        {
            DayComboBox.ItemsSource = Enumerable.Range(1, DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month)).ToList();
            
        }
    }
}


