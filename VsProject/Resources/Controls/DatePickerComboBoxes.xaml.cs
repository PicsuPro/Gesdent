using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
    DependencyProperty.Register("SelectedDate", typeof(DateOnly), typeof(DatePickerComboBoxes),
        new PropertyMetadata(new DateOnly(9999,1,1), OnSelectedDateChanged));

        public static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePickerComboBoxes = (DatePickerComboBoxes)d;
            datePickerComboBoxes.SelectedDate = (DateOnly)e.NewValue;
        }

        public DateOnly SelectedDate
        {
            get
            {
                return (DateOnly)GetValue(SelectedDateProperty);
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


        private static readonly string[] months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Where(m => !string.IsNullOrEmpty(m)).Select(CultureInfo.CurrentCulture.TextInfo.ToTitleCase).ToArray();



        private int _monthIndexCompensator = 1;

        private bool _isUserAction = false;
        public DatePickerComboBoxes()
        {

            InitializeComponent();
            DayComboBox.ItemsSource = Enumerable.Range(1, 31);
            MonthComboBox.ItemsSource = months;
            SetYears();
            UpdateComboAndTextBoxes();
        }
        private bool SetSelectedDate(int year, int month, int day)
        {
            try
            {
                int maxDaysInMonth = DateTime.DaysInMonth(year, month);
                SelectedDate = new DateOnly(year, month, Math.Min(day, maxDaysInMonth));
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
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
            else if (Times == DatePickerTimes.Future)
            {
                YearComboBox.ItemsSource = Enumerable.Range(DateTime.Now.Year, 11);
            }
        }

        private void UpdateComboAndTextBoxes()
        {


            DayComboBox.SelectedItem = SelectedDate.Day;
            MonthComboBox.SelectedIndex = SelectedDate.Month - _monthIndexCompensator;
            YearComboBox.SelectedItem = SelectedDate.Year;

        }

        private void UpdateSelectedDate()
        {
            if (!_isUserAction)
            {
                return;
            }

            if (DayComboBox.SelectedItem == null || MonthComboBox.SelectedItem == null || YearComboBox.SelectedItem == null)
            {
                return;
            }
            int day = (int)DayComboBox.SelectedItem;
            int month = MonthComboBox.SelectedIndex + _monthIndexCompensator;
            int year = (int)YearComboBox.SelectedItem;


            SetSelectedDate(year, month, day);



        }

        private void DayComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if(YearComboBox.SelectedItem == null || MonthComboBox.SelectedItem == null) { return; }
            int daysInMonth = DateTime.DaysInMonth((int)YearComboBox.SelectedItem, MonthComboBox.SelectedIndex + _monthIndexCompensator);

            if ((int?)YearComboBox.SelectedItem == DateTime.Now.Year
                && MonthComboBox.SelectedIndex == DateTime.Now.Month - _monthIndexCompensator
                && Times != DatePickerTimes.All)
            {
                int currentDay = DateTime.Now.Day;
                if (Times == DatePickerTimes.Past)
                {
                    DayComboBox.ItemsSource = Enumerable.Range(1, currentDay);
                }
                else if (Times == DatePickerTimes.Future)
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
                && Times != DatePickerTimes.All)
            {
                int currentDay = DateTime.Now.Day;
                var currentDayIndex = DayComboBox.Items.IndexOf(currentDay);
                if (DayComboBox.SelectedItem != null)
                { 
                    if (Times == DatePickerTimes.Past)
                    {
                        if ((int)DayComboBox.SelectedItem > currentDay)
                        {
                            DayComboBox.SelectedIndex = currentDayIndex;
                        }
                    }
                    else if(Times == DatePickerTimes.Future)
                    {
                        if ((int)DayComboBox.SelectedItem < currentDay)
                        {
                            DayComboBox.SelectedIndex = currentDayIndex;
                        }
                    }
                }
            }
            UpdateSelectedDate();
        }

        private void MonthComboBox_DropDownOpened(object sender, EventArgs e)
        {

            if (Times == DatePickerTimes.All)
            {
                return;
            }
            if ((int?)YearComboBox.SelectedItem == DateTime.Now.Year)
            {
                int currentMonthIndex = DateTime.Now.Month - _monthIndexCompensator;
                if (Times == DatePickerTimes.Past)
                {
                    MonthComboBox.ItemsSource = months.Take(currentMonthIndex + _monthIndexCompensator);
                }
                else if (Times == DatePickerTimes.Future)
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


            if (Times != DatePickerTimes.All && (int?)YearComboBox.SelectedItem == DateTime.Now.Year)
            {
                int currentMonthIndex = DateTime.Now.Month - _monthIndexCompensator;
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
                }
                else if (Times == DatePickerTimes.Future)
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

    }
}


