using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Synergy.WPF.Common.Controls
{
    [TemplatePart(Name = "PART_HoursTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_MinutesTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IncHoursButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DecHoursButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_IncMinutesButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DecMinutesButton", Type = typeof(Button))]
    public class TimePicker : Control
    {
        private const string PART_HoursTextBox = "PART_HoursTextBox";
        private const string PART_MinutesTextBox = "PART_MinutesTextBox";
        private const string PART_IncHoursButton = "PART_IncHoursButton";
        private const string PART_DecHoursButton = "PART_DecHoursButton";
        private const string PART_IncMinutesButton = "PART_IncMinutesButton";
        private const string PART_DecMinutesButton = "PART_DecMinutesButton";

        #region Members

        private TextBox _hoursTextBox;
        private TextBox _minutesTextBox;
        private Button _incHoursButton;
        private Button _decHoursButton;
        private Button _incMinutesButton;
        private Button _decMinutesButton;

        private volatile bool _textChanging;

        private string _bindingInited;

        #endregion

        #region Properties

        #region Time

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(string), typeof(TimePicker), new PropertyMetadata("", OnTimeChanged));

        public string Time
        {
            get => (string)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timePicker = (TimePicker)d;
            if (timePicker != null)
                timePicker.OnTimeChanged((string)e.OldValue, (string)e.NewValue);
        }

        private void OnTimeChanged(string oldValue, string newValue)
        {
            if (_textChanging)
                return;

            if (_hoursTextBox is null || _minutesTextBox is null)
            {
                _bindingInited = newValue;
                return;
            }

            try
            {
                _textChanging = true;

                string time = newValue;

                if (string.IsNullOrWhiteSpace(time))
                    time = "00:00";

                var vals = time.Split(':');

                if (vals.Length != 2)
                    throw new ArgumentException(null, nameof(newValue));

                var hours = uint.Parse(vals[0]);
                var minutes = uint.Parse(vals[1]);

                if (hours > 23 || minutes > 59)
                    throw new ArgumentException(null, nameof(newValue));

                var hoursText = hours.ToString();
                if (hoursText.Length == 1)
                    hoursText = hoursText.Insert(0, "0");

                var minutesText = minutes.ToString();
                if (minutesText.Length == 1)
                    minutesText = minutesText.Insert(0, "0");

                _hoursTextBox.Text = hoursText;
                _minutesTextBox.Text = minutesText;
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                _textChanging = false;
            }
        }

        #endregion

        #region TextForeground

        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register("TextForeground", typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Black));

        public Brush TextForeground
        {
            get => (Brush)GetValue(TextForegroundProperty);
            set => SetValue(TextForegroundProperty, value);
        }

        #endregion

        #region TextBackground

        public static readonly DependencyProperty TextBackgroundProperty =
            DependencyProperty.Register("TextBackground", typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.LightGray));

        public Brush TextBackground
        {
            get => (Brush)GetValue(TextBackgroundProperty);
            set => SetValue(TextBackgroundProperty, value);
        }

        #endregion

        #region OuterBorderBrush

        public static readonly DependencyProperty OuterBorderBrushProperty =
            DependencyProperty.Register("OuterBorderBrush", typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Black));

        public Brush OuterBorderBrush
        {
            get => (Brush)GetValue(OuterBorderBrushProperty);
            set => SetValue(OuterBorderBrushProperty, value);
        }

        #endregion

        #region OuterBorderBackground

        public static readonly DependencyProperty OuterBorderBackgroundProperty =
            DependencyProperty.Register("OuterBorderBackground", typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.White));

        public Brush OuterBorderBackground
        {
            get => (Brush)GetValue(OuterBorderBackgroundProperty);
            set => SetValue(OuterBorderBackgroundProperty, value);
        }

        #endregion

        #region OuterBorderThickness

        public static readonly DependencyProperty OuterBorderThicknessProperty =
            DependencyProperty.Register("OuterBorderThickness", typeof(Thickness), typeof(TimePicker), new PropertyMetadata(new Thickness(1)));

        public Thickness OuterBorderThickness
        {
            get => (Thickness)GetValue(OuterBorderThicknessProperty);
            set => SetValue(OuterBorderThicknessProperty, value);
        }

        #endregion

        #region OuterBorderCornerRadius

        public static readonly DependencyProperty OuterBorderCornerRadiusProperty =
            DependencyProperty.Register("OuterBorderCornerRadius", typeof(CornerRadius), typeof(TimePicker), new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius OuterBorderCornerRadius
        {
            get => (CornerRadius)GetValue(OuterBorderCornerRadiusProperty);
            set => SetValue(OuterBorderCornerRadiusProperty, value);
        }

        #endregion

        #region ButtonForeground

        public static readonly DependencyProperty ButtonForegroundProperty =
            DependencyProperty.Register("ButtonForeground", typeof(Brush), typeof(TimePicker), new PropertyMetadata(Brushes.Purple));

        public Brush ButtonForeground
        {
            get => (Brush)GetValue(ButtonForegroundProperty);
            set => SetValue(ButtonForegroundProperty, value);
        }

        #endregion

        #endregion

        #region Constructors

        static TimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
        }

        public TimePicker()
        {
            _textChanging = false;
            _bindingInited = null;
        }

        #endregion

        #region Base class overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // _incHoursButton
            if (_incHoursButton != null)
                _incHoursButton.Click -= IncHoursButton_Click;

            _incHoursButton = GetTemplateChild(PART_IncHoursButton) as Button;
            if(_incHoursButton != null )
                _incHoursButton.Click += IncHoursButton_Click;


            // _decHoursButton
            if (_decHoursButton != null)
                _decHoursButton.Click -= DecHoursButton_Click;

            _decHoursButton = GetTemplateChild(PART_DecHoursButton) as Button;
            if(_decHoursButton != null)
                _decHoursButton.Click += DecHoursButton_Click;


            // _incMinutesButton
            if (_incMinutesButton != null)
                _incMinutesButton.Click -= IncMinutesButton_Click;

            _incMinutesButton = GetTemplateChild(PART_IncMinutesButton) as Button;
            if (_incMinutesButton != null)
                _incMinutesButton.Click += IncMinutesButton_Click;


            // _decMinutesButton
            if (_decMinutesButton != null)
                _decMinutesButton.Click -= DecMinutesButton_Click;

            _decMinutesButton = GetTemplateChild(PART_DecMinutesButton) as Button;
            if (_decMinutesButton != null)
                _decMinutesButton.Click += DecMinutesButton_Click;


            // _hoursTextBox
            if (_hoursTextBox != null)
            {
                _hoursTextBox.PreviewTextInput -= HoursTextBox_PreviewTextInput;
                _hoursTextBox.TextChanged -= HoursTextBox_TextChanged;
                _hoursTextBox.LostFocus -= HoursTextBox_LostFocus;
                _hoursTextBox.LostKeyboardFocus -= HoursTextBox_LostKeyboardFocus;
            }

            _hoursTextBox = GetTemplateChild(PART_HoursTextBox) as TextBox;
            if (_hoursTextBox != null)
            {
                _hoursTextBox.PreviewTextInput += HoursTextBox_PreviewTextInput;
                _hoursTextBox.TextChanged += HoursTextBox_TextChanged;
                _hoursTextBox.LostFocus += HoursTextBox_LostFocus;
                _hoursTextBox.LostKeyboardFocus += HoursTextBox_LostKeyboardFocus;
            }


            // _minutesTextBox
            if (_minutesTextBox != null)
            {
                _minutesTextBox.PreviewTextInput -= MinutesTextBox_PreviewTextInput;
                _minutesTextBox.TextChanged -= MinutesTextBox_TextChanged;
                _minutesTextBox.LostFocus -= MinutesTextBox_LostFocus;
                _minutesTextBox.LostKeyboardFocus -= MinutesTextBox_LostKeyboardFocus;
            }

            _minutesTextBox = GetTemplateChild(PART_MinutesTextBox) as TextBox;
            if(_minutesTextBox != null )
            {
                _minutesTextBox.PreviewTextInput += MinutesTextBox_PreviewTextInput;
                _minutesTextBox.TextChanged += MinutesTextBox_TextChanged;
                _minutesTextBox.LostFocus += MinutesTextBox_LostFocus;
                _minutesTextBox.LostKeyboardFocus += MinutesTextBox_LostKeyboardFocus;
            }

            Time = "00:00";

            if (!string.IsNullOrEmpty(_bindingInited))
            {
                Time = TimeOnly.Parse(Time).AddHours(1).ToString("HH:mm");
                Time = _bindingInited;
                _bindingInited = string.Empty;
            }
        }

        #endregion

        #region Event handlers

        #region Buttons

        private void IncHoursButton_Click(object sender, RoutedEventArgs e)
        {
            if (_hoursTextBox is null)
                return;

            _textChanging = true;

            if (_hoursTextBox.Text.Equals("23"))
            {
                _hoursTextBox.Text = "00";
            }
            else
            {
                var hours = int.Parse(_hoursTextBox.Text);
                hours++;

                var text = hours.ToString();
                if (text.Length == 1)
                    text = text.Insert(0, "0");

                _hoursTextBox.Text = text;
            }

            UpdateTime();

            _textChanging = false;
        }

        private void DecHoursButton_Click(object sender, RoutedEventArgs e)
        {
            if (_hoursTextBox is null)
                return;

            _textChanging = true;

            if (_hoursTextBox.Text.Equals("00"))
            {
                _hoursTextBox.Text = "23";
            }
            else
            {
                var hours = int.Parse(_hoursTextBox.Text);
                hours--;

                var text = hours.ToString();
                if (text.Length == 1)
                    text = text.Insert(0, "0");

                _hoursTextBox.Text = text;
            }

            UpdateTime();

            _textChanging = false;
        }

        private void IncMinutesButton_Click(object sender, RoutedEventArgs e)
        {
            if (_minutesTextBox is null)
                return;

            _textChanging = true;

            if (_minutesTextBox.Text.Equals("59"))
            {
                _minutesTextBox.Text = "00";
            }
            else
            {
                var minutes = int.Parse(_minutesTextBox.Text);
                minutes++;

                var text = minutes.ToString();
                if (text.Length == 1)
                    text = text.Insert(0, "0");

                _minutesTextBox.Text = text;
            }

            UpdateTime();

            _textChanging = false;
        }

        private void DecMinutesButton_Click(object sender, RoutedEventArgs e)
        {
            if (_minutesTextBox is null)
                return;

            _textChanging = true;

            if (_minutesTextBox.Text.Equals("00"))
            {
                _minutesTextBox.Text = "59";
            }
            else
            {
                var minutes = int.Parse(_minutesTextBox.Text);
                minutes--;

                var text = minutes.ToString();
                if (text.Length == 1)
                    text = text.Insert(0, "0");

                _minutesTextBox.Text = text;
            }

            UpdateTime();

            _textChanging = false;
        }

        #endregion

        #region HoursTextBox

        private void HoursTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (_hoursTextBox.Text.Length == 2)
            {
                e.Handled = true;
                return;
            }

            if (!CheckInputText(e.Text))
                e.Handled = true;
        }

        private void HoursTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_textChanging) return;
            _textChanging = true;

            _hoursTextBox.Text = _hoursTextBox.Text.Replace(" ", "");
            _hoursTextBox.Text = _hoursTextBox.Text.Replace("\n", "");

            UpdateTime();

            _textChanging = false;
        }

        private void HoursTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus_impl(_hoursTextBox, 23);
        }

        private void HoursTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            LostFocus_impl(_hoursTextBox, 23);
        }

        #endregion

        #region MinutesTextBox

        private void MinutesTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (_minutesTextBox.Text.Length == 2)
            {
                e.Handled = true;
                return;
            }

            if (!CheckInputText(e.Text))
                e.Handled = true;
        }

        private void MinutesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_textChanging) return;
            _textChanging = true;

            _minutesTextBox.Text = _minutesTextBox.Text.Replace(" ", "");
            _minutesTextBox.Text = _minutesTextBox.Text.Replace("\n", "");

            UpdateTime();

            _textChanging = false;
        }

        private void MinutesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus_impl(_minutesTextBox, 59);
        }

        private void MinutesTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            LostFocus_impl(_minutesTextBox, 59);
        }

        #endregion

        #endregion

        #region Methods

        private void LostFocus_impl(TextBox tb, int max)
        {
            if (_textChanging) return;
            _textChanging = true;

            if (tb == null)
                return;

            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = "00";
            }
            else
            {
                if (uint.Parse(tb.Text) > max)
                    tb.Text = max.ToString();
            }

            if (tb.Text.Length == 1)
                tb.Text = tb.Text.Insert(0, "0");

            UpdateTime();

            _textChanging = false;
        }

        private void UpdateTime()
        {
            Time = _hoursTextBox.Text + ":" + _minutesTextBox.Text;
        }

        private static bool CheckInputText(string text)
        {
            var regex = new Regex(@"^\d+$", RegexOptions.Compiled);

            return regex.IsMatch(text);
        }

        #endregion
    }
}
