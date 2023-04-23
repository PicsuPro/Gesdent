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
    public enum DatePickerTimes
    {
        All,
        Past,
        Future
    }
    public partial class DatePickerComboBoxes : UserControl
    {

        public static readonly DependencyProperty SelectedDateProperty =
    DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePickerComboBoxes),
        new PropertyMetadata( DateTime.Now,OnSelectedDateChanged) );

        public static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePickerComboBoxes = (DatePickerComboBoxes)d;
            datePickerComboBoxes.isUserAction = false;
            datePickerComboBoxes.UpdateComboBoxes();
            datePickerComboBoxes.isUserAction = true;
        }

        

        public DateTime? SelectedDate
        {
            get { 
                return (DateTime?)GetValue(SelectedDateProperty); }
            set
            {
                SetValue(SelectedDateProperty, value);
                UpdateComboBoxes();
            }
        }

        public static readonly DependencyProperty TimesProperty = DependencyProperty.Register(
       "Times", typeof(DatePickerTimes), typeof(DatePickerComboBoxes), new PropertyMetadata(DatePickerTimes.All, OnTimesChanged));

        public static void OnTimesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePickerComboBoxes = (DatePickerComboBoxes)d;
            datePickerComboBoxes.SetYears();

        }
        public DatePickerTimes Times
        {
            get => (DatePickerTimes)GetValue(TimesProperty);
            set => SetValue(TimesProperty, value);
        }

        private static readonly List<string> months = new List<string>()
        {
            "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
            "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"
        };

        private int monthIndexCompensator = 1;

        private bool isUserAction = false;
        public DatePickerComboBoxes()
        {
           
            InitializeComponent();
            
            // Initialize the ComboBoxes
            DayComboBox.ItemsSource = Enumerable.Range(1, 31);
            MonthComboBox.ItemsSource = months;
            SetYears();

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

        private void SetYears()
        {
            if (Times == DatePickerTimes.All)
            {
                YearComboBox.ItemsSource = Enumerable.Range(1900, DateTime.Now.Year - 1900 + 11);
            }
            else if (Times == DatePickerTimes.Past)
            {
                YearComboBox.ItemsSource = Enumerable.Range(1900, DateTime.Now.Year - 1900 + 1);
            }
            else
            {
                YearComboBox.ItemsSource = Enumerable.Range(DateTime.Now.Year, 11);
            }
        }

        private void UpdateComboBoxes()
        {
            if (SelectedDate == null) return;
            DayComboBox.SelectedItem = ((DateTime)SelectedDate).Day;
            MonthComboBox.SelectedIndex = ((DateTime)SelectedDate).Month - monthIndexCompensator;
            YearComboBox.SelectedItem = ((DateTime)SelectedDate).Year;

        }

        private void UpdateSelectedDate()
        {
            if (!isUserAction)
            {
                return;
            }

            if (DayComboBox.SelectedItem == null || MonthComboBox.SelectedItem == null || YearComboBox.SelectedItem == null)
            {
                return;
            }
            int day = (int)DayComboBox.SelectedItem;
            int month = MonthComboBox.SelectedIndex + monthIndexCompensator;
            int year = (int)YearComboBox.SelectedItem;
           

            SelectedDate = ValidDate(year, month, day);

        }

        private void DayComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (SelectedDate == null) return;
            int daysInMonth = DateTime.DaysInMonth(((DateTime)SelectedDate).Year, ((DateTime)SelectedDate).Month);
            if ((int?)YearComboBox.SelectedItem == DateTime.Now.Year &&
                MonthComboBox.SelectedIndex == DateTime.Now.Month - monthIndexCompensator && 
                Times != DatePickerTimes.All)
            {
                int currentDay = DateTime.Now.Day;
                if (Times == DatePickerTimes.Past)
                {
                    DayComboBox.ItemsSource = Enumerable.Range(1, currentDay);
                }
                else
                {
                    DayComboBox.ItemsSource = Enumerable.Range(currentDay, daysInMonth - currentDay);
                }
            }
            else
            {
                DayComboBox.ItemsSource = Enumerable.Range(1, daysInMonth);
            }
            
        }
        private void DayComboBox_SelectionChanged(object sender, EventArgs e)
        {
            if (!isUserAction)
            {
                return;
            }
            if ((int?)YearComboBox.SelectedItem == DateTime.Now.Year &&
                MonthComboBox.SelectedIndex == DateTime.Now.Month - monthIndexCompensator &&
                Times != DatePickerTimes.All)
            {
                int currentDay = DateTime.Now.Day;
                if (Times == DatePickerTimes.Past)
                {
                    if ((int)DayComboBox.SelectedItem > currentDay)
                    {
                        DayComboBox.SelectedIndex = DayComboBox.Items.IndexOf(currentDay);
                    }
                }
                else
                {
                    if ((int)DayComboBox.SelectedItem < currentDay)
                    {
                        DayComboBox.SelectedIndex = DayComboBox.Items.IndexOf(currentDay);
                    }
                }
            }
            UpdateSelectedDate();
        }

        private void MonthComboBox_DropDownOpened(object sender, EventArgs e)
        {

            if(Times == DatePickerTimes.All)
            {
                return;
            }
            if ((int?)YearComboBox.SelectedItem == DateTime.Now.Year)
            {
                int currentMonthIndex = DateTime.Now.Month - monthIndexCompensator;
                if (Times == DatePickerTimes.Past )
                {
                     MonthComboBox.ItemsSource = months.Take(currentMonthIndex + monthIndexCompensator);
                }else
                {
                    MonthComboBox.ItemsSource= months.Skip(currentMonthIndex + monthIndexCompensator -1);
                    monthIndexCompensator = currentMonthIndex + monthIndexCompensator;
                }
            }
            else
            {
                monthIndexCompensator = 1;
                MonthComboBox.ItemsSource = months;
            }
        }

        private void MonthComboBox_SelectionChanged(object sender, EventArgs? e)
        {
            if (!isUserAction)
            {
                return;
            }


            if (Times != DatePickerTimes.All && (int?)YearComboBox.SelectedItem == DateTime.Now.Year)
            {
                int currentMonthIndex = DateTime.Now.Month - monthIndexCompensator;
                int currentDay = DateTime.Now.Day;
                if (Times == DatePickerTimes.Past)
                {
                    if (MonthComboBox.SelectedIndex > currentMonthIndex)
                    {
                        MonthComboBox.SelectedIndex = currentMonthIndex;
                    }
                    if (MonthComboBox.SelectedIndex == currentMonthIndex)
                    {
                        if ((int)DayComboBox.SelectedItem > currentDay)
                        {
                            DayComboBox.SelectedIndex = DayComboBox.Items.IndexOf(currentDay);
                        }
                    }
                }else
                {
                    if (MonthComboBox.SelectedIndex < currentMonthIndex)
                    {
                        MonthComboBox.SelectedIndex = currentMonthIndex;
                    }
                    if (MonthComboBox.SelectedIndex == currentMonthIndex)
                    {
                        if ((int)DayComboBox.SelectedItem < currentDay)
                        {
                            DayComboBox.SelectedIndex = DayComboBox.Items.IndexOf(currentDay);
                        }
                    }
                }
            }
            UpdateSelectedDate();
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isUserAction)
            {
                return;
            }
            MonthComboBox_SelectionChanged(this,null);
        }
    }
}


