using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VsProject.Resources.Controls
{

    public partial class TimePickerTextBoxes : UserControl
    {

        public static readonly DependencyProperty SelectedTimeProperty =
    DependencyProperty.Register("SelectedTime", typeof(TimeOnly), typeof(TimePickerTextBoxes),
        new PropertyMetadata(TimeOnly.FromDateTime(DateTime.Now), OnSelectedTimeChanged));

        public static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePickerComboBoxes = (TimePickerTextBoxes)d;
            if (e.OldValue != e.NewValue)
            {
                datePickerComboBoxes.SelectedTime = (TimeOnly)e.NewValue;
            }
        }

        public TimeOnly SelectedTime
        {
            get
            {
                return (TimeOnly)GetValue(SelectedTimeProperty);
            }
            set
            {

                _isUserAction = false;
                SetValue(SelectedTimeProperty, value);
                UpdateTextBoxes();
                _isUserAction = true;
            }
        }
        
        public static readonly DependencyProperty MaxHourProperty =
    DependencyProperty.Register("MaxHour", typeof(int?), typeof(TimePickerTextBoxes),
        new PropertyMetadata(null, OnMaxHourChanged));

        public static void OnMaxHourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timePickerComboBoxes = (TimePickerTextBoxes)d;
            timePickerComboBoxes.txtHours.Max = (int?)e.NewValue;
        }

        public int? MaxHour
        {
            get
            {
                return (int?)GetValue(MaxHourProperty);
            }
            set
            {

                SetValue(MaxHourProperty, value);
            }
        }


        public static readonly DependencyProperty MinHourProperty =
    DependencyProperty.Register("MinHour", typeof(int), typeof(TimePickerTextBoxes),
        new PropertyMetadata(0, OnMinHourChanged));

        public static void OnMinHourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timePickerComboBoxes = (TimePickerTextBoxes)d;
            timePickerComboBoxes.txtHours.Min = (int)e.NewValue;
        }

        public int MinHour
        {
            get
            {
                return (int)GetValue(MinHourProperty);
            }
            set
            {

                SetValue(MinHourProperty, value);
            }
        }


       // public static readonly DependencyProperty TimesProperty = DependencyProperty.Register(
       //"Times", typeof(DatePickerTimes), typeof(TimePickerTextBoxes), new PropertyMetadata(DatePickerTimes.All, OnTimesChanged));

       // public static void OnTimesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       // {
       //     var datePickerComboBoxes = (TimePickerTextBoxes)d;

       // }
       // public DatePickerTimes Times
       // {
       //     get => (DatePickerTimes)GetValue(TimesProperty);
       //     set => SetValue(TimesProperty, value);
       // }

        private int _selectedHour;

        public int SelectedHour
        {
            get => _selectedHour;
            set
            {
                _selectedHour = value;
                SetSelectedTime( value, SelectedTime.Minute, SelectedTime.Second);
            }
        }

        private int _selectedMinute;

        public int SelectedMinute
        {
            get => _selectedMinute;
            set
            {
                _selectedMinute = value;
                SetSelectedTime(SelectedTime.Hour, value, SelectedTime.Second);
            }
        }


        private static readonly string[] months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Where(m => !string.IsNullOrEmpty(m)).Select(CultureInfo.CurrentCulture.TextInfo.ToTitleCase).ToArray();



        private int _monthIndexCompensator = 1;

        private bool _isUserAction = false;
        public TimePickerTextBoxes()
        {

            InitializeComponent();
            txtHours.Max = MaxHour?? 23;
            txtMinutes.Max = 59;
            txtHours.textBox.TextChanged += OnHourTextChanged;
            txtMinutes.textBox.TextChanged += OnMinuteTextChanged;
            UpdateTextBoxes();
        }
        private bool SetSelectedTime( int hour, int minute, int second)
        {
            try
            {
                SelectedTime = new TimeOnly( hour, minute, second);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

     
        private void UpdateTextBoxes()
        {
            txtHours.textBox.Text = SelectedTime.Hour.ToString();
            txtMinutes.textBox.Text = SelectedTime.Minute.ToString();

        }

        private void UpdateSelectedTime()
        {
            if (!_isUserAction)
            {
                return;
            }

            if (!int.TryParse(txtHours.textBox.Text, out int hour) || !int.TryParse(txtMinutes.textBox.Text, out int minute))
            {
                return;
            }


            SetSelectedTime(hour, minute, SelectedTime.Second);



        }


        private void OnHourTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isUserAction)
            {
                return;
            }
            UpdateSelectedTime();
        }
        private void OnMinuteTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isUserAction)
            {
                return;
            }
            UpdateSelectedTime();
        }
    }
}


