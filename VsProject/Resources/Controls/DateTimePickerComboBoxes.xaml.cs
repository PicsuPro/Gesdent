using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VsProject.Services;

namespace VsProject.Resources.Controls
{
    public enum DateTimePickerTimes
    {
        All,
        Past,
        Future
    }

    public partial class DateTimePickerComboBoxes : UserControl
    {

        public static readonly DependencyProperty SelectedDateProperty =
    DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(DateTimePickerComboBoxes),
        new PropertyMetadata(DateTime.Now, OnSelectedDateChanged));

        public static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePickerComboBoxes = (DateTimePickerComboBoxes)d;
            if (e.OldValue != e.NewValue)
            { 
                datePickerComboBoxes.SelectedDate = (DateTime)e.NewValue;
            } 
        }

        public DateTime SelectedDate
        {
            get
            {
                return (DateTime)GetValue(SelectedDateProperty);
            }
            set
            {

                _isUserAction = false;
                SetValue(SelectedDateProperty, value);
                UpdateComboAndTextBoxes();
                _isUserAction = true;
            }
        }


        public static readonly DependencyProperty TimesProperty = DependencyProperty.Register(
       "Times", typeof(DateTimePickerTimes), typeof(DateTimePickerComboBoxes), new PropertyMetadata(DateTimePickerTimes.All, OnTimesChanged));

        public static void OnTimesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePickerComboBoxes = (DateTimePickerComboBoxes)d;
            datePickerComboBoxes.SetYears();

        }
        public DateTimePickerTimes Times
        {
            get => (DateTimePickerTimes)GetValue(TimesProperty);
            set => SetValue(TimesProperty, value);
        }

        private int _selectedHour;

        public int SelectedHour 
        {
            get => _selectedHour;
            set 
            {
                _selectedHour = value;
                SetSelectedDate(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, value, SelectedDate.Minute, SelectedDate.Second);
            }
        }
        
        private int _selectedMinute;

        public int SelectedMinute 
        {
            get => _selectedMinute;
            set 
            {
                _selectedMinute = value;
                SetSelectedDate(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, SelectedDate.Hour , value, SelectedDate.Second);
            }
        }


        private static readonly string[] months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Where(m => !string.IsNullOrEmpty(m)).Select(CultureInfo.CurrentCulture.TextInfo.ToTitleCase).ToArray();



        private int _monthIndexCompensator = 1;

        private bool _isUserAction = false;
        public DateTimePickerComboBoxes()
        {

            InitializeComponent();

            DayComboBox.ItemsSource = Enumerable.Range(1, 31);
            MonthComboBox.ItemsSource = months;
            SetYears();
            UpdateComboAndTextBoxes();
        }
        private bool SetSelectedDate(int year, int month, int day, int hour, int minute, int second)
        {
            try
            {
                int maxDaysInMonth = DateTime.DaysInMonth(year, month);
                SelectedDate = new DateTime(year, month, Math.Min(day, maxDaysInMonth), hour, minute, second);
                return true;
            }
            catch(ArgumentOutOfRangeException)
            {
                return false;
            }
        }




        private void SetYears()
        {
            if (Times == DateTimePickerTimes.All)
            {
                YearComboBox.ItemsSource = Enumerable.Range(1900, DateTime.Now.Year - 1900 + 11);
            }
            else if (Times == DateTimePickerTimes.Past)
            {
                YearComboBox.ItemsSource = Enumerable.Range(1900, DateTime.Now.Year - 1900 + 1);
            }
            else
            {
                YearComboBox.ItemsSource = Enumerable.Range(DateTime.Now.Year, 11);
            }
        }

        private void UpdateComboAndTextBoxes()
        {


            DayComboBox.SelectedItem = SelectedDate.Day;
            MonthComboBox.SelectedIndex = SelectedDate.Month - _monthIndexCompensator;
            YearComboBox.SelectedItem = SelectedDate.Year;

            txtHours.Text = SelectedDate.Hour.ToString();
            txtMinutes.Text = SelectedDate.Minute.ToString();
            
        }

        private void UpdateSelectedDate()
        {
            if (!_isUserAction)
            {
                return;
            }

            if (DayComboBox.SelectedItem == null || MonthComboBox.SelectedItem == null || YearComboBox.SelectedItem == null || !int.TryParse(txtHours.Text, out int hour) || !int.TryParse(txtMinutes.Text, out int minute))
            {
                return;
            }
            int day = (int)DayComboBox.SelectedItem;
            int month = MonthComboBox.SelectedIndex + _monthIndexCompensator;
            int year = (int)YearComboBox.SelectedItem;


            SetSelectedDate(year, month, day, hour, minute, SelectedDate.Second);



        }

        private void DayComboBox_DropDownOpened(object sender, EventArgs e)
        {
            int daysInMonth = DateTime.DaysInMonth((int)YearComboBox.SelectedItem, MonthComboBox.SelectedIndex + _monthIndexCompensator);

            if ((int?)YearComboBox.SelectedItem == DateTime.Now.Year
                && MonthComboBox.SelectedIndex == DateTime.Now.Month - _monthIndexCompensator
                && Times != DateTimePickerTimes.All)
            {
                int currentDay = DateTime.Now.Day;
                if (Times == DateTimePickerTimes.Past)
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
            if (!_isUserAction)
            {
                return;
            }
            if ((int?)YearComboBox.SelectedItem == DateTime.Now.Year
                && MonthComboBox.SelectedIndex == DateTime.Now.Month - _monthIndexCompensator
                && Times != DateTimePickerTimes.All)
            {
                int currentDay = DateTime.Now.Day;
                if (Times == DateTimePickerTimes.Past)
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

            if (Times == DateTimePickerTimes.All)
            {
                return;
            }
            if ((int?)YearComboBox.SelectedItem == DateTime.Now.Year)
            {
                int currentMonthIndex = DateTime.Now.Month - _monthIndexCompensator;
                if (Times == DateTimePickerTimes.Past)
                {
                    MonthComboBox.ItemsSource = months.Take(currentMonthIndex + _monthIndexCompensator);
                }
                else
                {
                    MonthComboBox.ItemsSource = months.Skip(currentMonthIndex + _monthIndexCompensator - 1);
                    _monthIndexCompensator = currentMonthIndex + _monthIndexCompensator;
                }
            }
            else
            {
                _monthIndexCompensator = 1;
                MonthComboBox.ItemsSource = months;
            }
        }

        private void MonthComboBox_SelectionChanged(object sender, EventArgs? e)
        {
            if (!_isUserAction)
            {
                return;
            }


            if (Times != DateTimePickerTimes.All && (int?)YearComboBox.SelectedItem == DateTime.Now.Year)
            {
                int currentMonthIndex = DateTime.Now.Month - _monthIndexCompensator;
                int currentDay = DateTime.Now.Day;
                if (Times == DateTimePickerTimes.Past)
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
                }
                else
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
            if (!_isUserAction)
            {
                return;
            }
            MonthComboBox_SelectionChanged(this, e);
        }

        private void OnHourTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isUserAction)
            {
                return;
            }
            if (int.TryParse(txtHours.Text, out int hour))
            {
                hour = hour.mod(24);
                UpdateSelectedDate();

            }

        }
    private void OnMinuteTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isUserAction)
            {
                return;
            }
            if (int.TryParse(txtMinutes.Text, out int minute))
            {
                minute = minute.mod(60);
                UpdateSelectedDate();
            }
           
        }
    }
}


