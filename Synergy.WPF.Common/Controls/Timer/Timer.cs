using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Synergy.WPF.Common.Controls
{
	public enum ManipulationMode
	{
		Manual = 0,
		Programatic
	}

	[TemplatePart(Name = PART_MinutesTextBox, Type = typeof(TextBox))]
	[TemplatePart(Name = PART_SecondsTextBox, Type = typeof(TextBox))]
	[TemplatePart(Name = PART_IncMinutesButton, Type = typeof(Button))]
	[TemplatePart(Name = PART_DecMinutesButton, Type = typeof(Button))]
	[TemplatePart(Name = PART_IncSecondsButton, Type = typeof(Button))]
	[TemplatePart(Name = PART_DecSecondsButton, Type = typeof(Button))]
	[TemplatePart(Name = PART_StartButton, Type = typeof(Button))]
	[TemplatePart(Name = PART_StopButton, Type = typeof(Button))]
	[TemplatePart(Name = PART_ResumeButton, Type = typeof(Button))]
	[TemplatePart(Name = PART_ProgressBar, Type = typeof(ProgressBar))]
	public class Timer : Control
	{
		private const string PART_MinutesTextBox = "PART_MinutesTextBox";
		private const string PART_SecondsTextBox = "PART_SecondsTextBox";

		private const string PART_IncMinutesButton = "PART_IncMinutesButton";
		private const string PART_DecMinutesButton = "PART_DecMinutesButton";
		private const string PART_IncSecondsButton = "PART_IncSecondsButton";
		private const string PART_DecSecondsButton = "PART_DecSecondsButton";

		private const string PART_StartButton = "PART_StartButton";
		private const string PART_StopButton = "PART_StopButton";
		private const string PART_ResumeButton = "PART_ResumeButton";

		private const string PART_ProgressBar = "PART_ProgressBar";

		#region Members

		private TextBox _secondsTextBox;
		private TextBox _minutesTextBox;

		private Button _incSecondsButton;
		private Button _decSecondsButton;
		private Button _incMinutesButton;
		private Button _decMinutesButton;

		private Button _startButton;
		private Button _stopButton;
		private Button _resumeButton;

		private ProgressBar _progressBar;

		private volatile bool _textChanging;
		private volatile bool _timerRunning;

		private Core.Timers.Timer _bindingInited;

		#endregion

		#region Properties

		#region ManipulationMode

		public static readonly DependencyProperty ManipulationModeProperty=
			DependencyProperty.Register("ManipulationMode", typeof(ManipulationMode), typeof(Timer), new PropertyMetadata(ManipulationMode.Manual));

		public ManipulationMode ManipulationMode
		{
			get => (ManipulationMode)GetValue(ManipulationModeProperty);
			set => SetValue(ManipulationModeProperty, value);
		}

		#endregion

		#region InnerTimer

		public static readonly DependencyProperty InnerTimerProperty=
			DependencyProperty.Register("InnerTimer", typeof(Core.Timers.Timer), typeof(Timer), new PropertyMetadata(OnTimerChanged));

		public Core.Timers.Timer InnerTimer
		{
			get => (Core.Timers.Timer)GetValue(InnerTimerProperty);
			set => SetValue(InnerTimerProperty, value);
		}

		private static void OnTimerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var Timer = (Timer)d;
			if (Timer != null)
				Timer.OnTimerChanged((Core.Timers.Timer)e.OldValue, (Core.Timers.Timer)e.NewValue);
		}

		private void OnTimerChanged(Core.Timers.Timer oldValue,  Core.Timers.Timer newValue)
		{
			if (_textChanging)
				return;

			if (_secondsTextBox is null || _minutesTextBox is null)
			{
				_bindingInited = newValue;
				return;
			}

			if(oldValue is not null)
			{
				UnsubscribeTimerEvents(oldValue);
			}

			try
			{
				_textChanging = true;

				SubscribeTimerEvents(newValue);

				var time = newValue.GetRestTime();

				SetTime(time);
			}
			catch (Exception)
			{
				UnsubscribeTimerEvents(newValue);

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
			DependencyProperty.Register("TextForeground", typeof(Brush), typeof(Timer), new PropertyMetadata(Brushes.Black));

		public Brush TextForeground
		{
			get => (Brush)GetValue(TextForegroundProperty);
			set => SetValue(TextForegroundProperty, value);
		}

		#endregion

		#region TextBackground

		public static readonly DependencyProperty TextBackgroundProperty =
			DependencyProperty.Register("TextBackground", typeof(Brush), typeof(Timer), new PropertyMetadata(Brushes.LightGray));

		public Brush TextBackground
		{
			get => (Brush)GetValue(TextBackgroundProperty);
			set => SetValue(TextBackgroundProperty, value);
		}

		#endregion

		#region OuterBorderBrush

		public static readonly DependencyProperty OuterBorderBrushProperty =
			DependencyProperty.Register("OuterBorderBrush", typeof(Brush), typeof(Timer), new PropertyMetadata(Brushes.Black));

		public Brush OuterBorderBrush
		{
			get => (Brush)GetValue(OuterBorderBrushProperty);
			set => SetValue(OuterBorderBrushProperty, value);
		}

		#endregion

		#region OuterBorderBackground

		public static readonly DependencyProperty OuterBorderBackgroundProperty =
			DependencyProperty.Register("OuterBorderBackground", typeof(Brush), typeof(Timer), new PropertyMetadata(Brushes.White));

		public Brush OuterBorderBackground
		{
			get => (Brush)GetValue(OuterBorderBackgroundProperty);
			set => SetValue(OuterBorderBackgroundProperty, value);
		}

		#endregion

		#region OuterBorderThickness

		public static readonly DependencyProperty OuterBorderThicknessProperty =
			DependencyProperty.Register("OuterBorderThickness", typeof(Thickness), typeof(Timer), new PropertyMetadata(new Thickness(1)));

		public Thickness OuterBorderThickness
		{
			get => (Thickness)GetValue(OuterBorderThicknessProperty);
			set => SetValue(OuterBorderThicknessProperty, value);
		}

		#endregion

		#region OuterBorderCornerRadius

		public static readonly DependencyProperty OuterBorderCornerRadiusProperty =
			DependencyProperty.Register("OuterBorderCornerRadius", typeof(CornerRadius), typeof(Timer), new PropertyMetadata(new CornerRadius(0)));

		public CornerRadius OuterBorderCornerRadius
		{
			get => (CornerRadius)GetValue(OuterBorderCornerRadiusProperty);
			set => SetValue(OuterBorderCornerRadiusProperty, value);
		}

		#endregion

		#region ButtonForeground

		public static readonly DependencyProperty ButtonForegroundProperty =
			DependencyProperty.Register("ButtonForeground", typeof(Brush), typeof(Timer), new PropertyMetadata(Brushes.Purple));

		public Brush ButtonForeground
		{
			get => (Brush)GetValue(ButtonForegroundProperty);
			set => SetValue(ButtonForegroundProperty, value);
		}

		#endregion

		#endregion

		#region Constructors

		static Timer()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(Timer), new FrameworkPropertyMetadata(typeof(Timer)));
		}

		public Timer()
		{
			_textChanging = false;
			_timerRunning = false;
			_bindingInited = null;
		}

		#endregion

		#region Base class overrides

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();


			// _progressBar
			_progressBar = GetTemplateChild(PART_ProgressBar) as ProgressBar;


			// _startButton
			if(_startButton != null)
				_startButton.Click -= _startButton_Click;

			_startButton = GetTemplateChild(PART_StartButton) as Button;
			if (_startButton != null)
				_startButton.Click += _startButton_Click;


			// _stopButton
			if(_stopButton != null)
				_stopButton.Click -= _stopButton_Click;

			_stopButton = GetTemplateChild(PART_StopButton) as Button;
			if (_stopButton != null)
				_stopButton.Click += _stopButton_Click;


			// _resumeButton
			if(_resumeButton != null)
				_resumeButton.Click -= _resumeButton_Click;

			_resumeButton = GetTemplateChild(PART_ResumeButton) as Button;
			if (_resumeButton != null)
				_resumeButton.Click += _resumeButton_Click;


			// _incSecondsButton
			if (_incSecondsButton != null)
				_incSecondsButton.Click -= IncSecondsButton_Click;

			_incSecondsButton = GetTemplateChild(PART_IncSecondsButton) as Button;
			if (_incSecondsButton != null)
				_incSecondsButton.Click += IncSecondsButton_Click;


			// _decSecondsButton
			if (_decSecondsButton != null)
				_decSecondsButton.Click -= DecSecondsButton_Click;

			_decSecondsButton = GetTemplateChild(PART_DecSecondsButton) as Button;
			if (_decSecondsButton != null)
				_decSecondsButton.Click += DecSecondsButton_Click;


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


			// _secondsTextBox
			if (_secondsTextBox != null)
			{
				_secondsTextBox.PreviewTextInput -= SecondsTextBox_PreviewTextInput;
				_secondsTextBox.TextChanged -= SecondsTextBox_TextChanged;
				_secondsTextBox.LostFocus -= SecondsTextBox_LostFocus;
				_secondsTextBox.LostKeyboardFocus -= SecondsTextBox_LostKeyboardFocus;
			}

			_secondsTextBox = GetTemplateChild(PART_SecondsTextBox) as TextBox;
			if (_secondsTextBox != null)
			{
				_secondsTextBox.PreviewTextInput += SecondsTextBox_PreviewTextInput;
				_secondsTextBox.TextChanged += SecondsTextBox_TextChanged;
				_secondsTextBox.LostFocus += SecondsTextBox_LostFocus;
				_secondsTextBox.LostKeyboardFocus += SecondsTextBox_LostKeyboardFocus;
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
			if (_minutesTextBox != null)
			{
				_minutesTextBox.PreviewTextInput += MinutesTextBox_PreviewTextInput;
				_minutesTextBox.TextChanged += MinutesTextBox_TextChanged;
				_minutesTextBox.LostFocus += MinutesTextBox_LostFocus;
				_minutesTextBox.LostKeyboardFocus += MinutesTextBox_LostKeyboardFocus;
			}

			if(_bindingInited != null)
			{
				InnerTimer = _bindingInited;
			}
		}

		#endregion

		#region Event handlers

		#region Inner timer

		private void InnerTimer_TimerFinished(object sender, Core.Timers.TimerFinishedEventArgs e)
		{
			Dispatcher?.Invoke(() =>
			{
				BlockChanging(false);
				_timerRunning = false;
			});
		}

		private void InnerTimer_TimerElapsed(object sender, Core.Timers.TimerElapsedEventArgs e)
		{
			// for progress bar
			Dispatcher?.Invoke(() =>
			{
				_progressBar.Value = 100 - (e.RestTime.TotalMilliseconds / e.TotalTime.TotalMilliseconds) * 39.3;
			});
		}

		private void InnerTimer_SecondElapsed(object sender, Core.Timers.TimerElapsedEventArgs e)
		{
			Dispatcher?.Invoke(() => SetTime(e.RestTime));
		}

		private void InnerTimer_Reseted(object sender, Core.Timers.TimerResetedEventArgs e)
		{
			Dispatcher?.Invoke(() =>
			{
				if (ManipulationMode == ManipulationMode.Manual) return;

				SetTime(e.ResetedTime);
			});
		}

		private void InnerTimer_Resumed(object sender, Core.Timers.TimerResumedEventArgs e)
		{
			Dispatcher?.Invoke(() =>
			{
				BlockChanging(true);
				_timerRunning = true;
			});
		}

		private void InnerTimer_Stopped(object sender, Core.Timers.TimerStoppedEventArgs e)
		{
			Dispatcher?.Invoke(() =>
			{
				BlockChanging(false);
				_timerRunning = false;
			});
		}

		private void InnerTimer_Started(object sender, Core.Timers.TimerStartedEventArgs e)
		{
			Dispatcher?.Invoke(() =>
			{
				_progressBar.Value = 100;

				BlockChanging(true);
				_timerRunning = true;
			});
		}

		private void InnerTimer_Disposed(object sender, EventArgs e)
		{
			UnsubscribeTimerEvents(InnerTimer);
		}

		#endregion

		#region UI

		#region Buttons

		private void _resumeButton_Click(object sender, RoutedEventArgs e)
		{
			InnerTimer?.Resume();
		}

		private void _stopButton_Click(object sender, RoutedEventArgs e)
		{
			InnerTimer?.Stop();
		}

		private void _startButton_Click(object sender, RoutedEventArgs e)
		{
			InnerTimer?.Start();
		}

		private void IncSecondsButton_Click(object sender, RoutedEventArgs e)
		{
			if (_secondsTextBox is null)
				return;

			_textChanging = true;

			if (_secondsTextBox.Text.Equals("59"))
			{
				_secondsTextBox.Text = "00";
			}
			else
			{
				var hours = int.Parse(_secondsTextBox.Text);
				hours++;

				var text = hours.ToString();
				if (text.Length == 1)
					text = text.Insert(0, "0");

				_secondsTextBox.Text = text;
			}

			UpdateTime();

			_textChanging = false;
		}

		private void DecSecondsButton_Click(object sender, RoutedEventArgs e)
		{
			if (_secondsTextBox is null)
				return;

			_textChanging = true;

			if (_secondsTextBox.Text.Equals("00"))
			{
				_secondsTextBox.Text = "59";
			}
			else
			{
				var hours = int.Parse(_secondsTextBox.Text);
				hours--;

				var text = hours.ToString();
				if (text.Length == 1)
					text = text.Insert(0, "0");

				_secondsTextBox.Text = text;
			}

			UpdateTime();

			_textChanging = false;
		}

		private void IncMinutesButton_Click(object sender, RoutedEventArgs e)
		{
			if (_minutesTextBox is null)
				return;

			_textChanging = true;

			if (_minutesTextBox.Text.Equals("99"))
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
				_minutesTextBox.Text = "99";
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

		#region SecondsTextBox

		private void SecondsTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			if (_secondsTextBox.Text.Length == 2)
			{
				e.Handled = true;
				return;
			}

			if (!CheckInputText(e.Text))
				e.Handled = true;
		}

		private void SecondsTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if(_timerRunning) return;

			if (ManipulationMode == ManipulationMode.Programatic) return;

			if (_textChanging) return;
			_textChanging = true;

			_secondsTextBox.Text = _secondsTextBox.Text.Replace(" ", "");
			_secondsTextBox.Text = _secondsTextBox.Text.Replace("\n", "");

			UpdateTime();

			_textChanging = false;
		}

		private void SecondsTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			LostFocus_impl(_secondsTextBox, 23);
		}

		private void SecondsTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			LostFocus_impl(_secondsTextBox, 23);
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
			if (_timerRunning) return;

			if (ManipulationMode == ManipulationMode.Programatic) return;

			if (_textChanging) return;
			_textChanging = true;

			_minutesTextBox.Text = _minutesTextBox.Text.Replace(" ", "");
			_minutesTextBox.Text = _minutesTextBox.Text.Replace("\n", "");

			UpdateTime();

			_textChanging = false;
		}

		private void MinutesTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			LostFocus_impl(_minutesTextBox, 99);
		}

		private void MinutesTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			LostFocus_impl(_minutesTextBox, 99);
		}

		#endregion

		#endregion

		#endregion

		#region Methods

		private void LostFocus_impl(TextBox tb, int max)
		{
			if (_textChanging || _timerRunning) return;
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
			var minutesMSs = uint.Parse(_minutesTextBox.Text) * 60 * 1000;
			var secondsMSs = uint.Parse(_secondsTextBox.Text) * 1000;

			InnerTimer.Reset(minutesMSs + secondsMSs);
		}

		private static bool CheckInputText(string text)
		{
			var regex = new Regex(@"^\d+$", RegexOptions.Compiled);

			return regex.IsMatch(text);
		}

		private void BlockChanging(bool value)
		{
			if(ManipulationMode == ManipulationMode.Programatic) return;

			_secondsTextBox.IsReadOnly = value;
			_minutesTextBox.IsReadOnly = value;

			_incMinutesButton.IsEnabled = !value;
			_incSecondsButton.IsEnabled = !value;
			_decMinutesButton.IsEnabled = !value;
			_decSecondsButton.IsEnabled = !value;
		}

		private void SetTime(TimeSpan time)
		{
			var minutes = (uint)time.TotalMinutes;
			var seconds = time.Seconds;

			_minutesTextBox.Text = minutes > 99 ? "^^" : minutes.ToString();
			_secondsTextBox.Text = seconds.ToString();

			if (_minutesTextBox.Text.Length == 1)
				_minutesTextBox.Text = "0" + _minutesTextBox.Text;

			if (_secondsTextBox.Text.Length == 1)
				_secondsTextBox.Text = "0" + _secondsTextBox.Text;
		}

		private void Reset()
		{
			if (InnerTimer is null) return;

			
		}

		private void SubscribeTimerEvents(Core.Timers.Timer timer)
		{
			timer.Elapsed += InnerTimer_TimerElapsed;
			timer.SecondElapsed += InnerTimer_SecondElapsed;
			timer.Finished += InnerTimer_TimerFinished;
			timer.Started += InnerTimer_Started;
			timer.Stopped += InnerTimer_Stopped;
			timer.Resumed += InnerTimer_Resumed;
			timer.Reseted += InnerTimer_Reseted;
			timer.Disposed += InnerTimer_Disposed;
		}

		private void UnsubscribeTimerEvents(Core.Timers.Timer timer)
		{
			timer.Elapsed -= InnerTimer_TimerElapsed;
			timer.SecondElapsed -= InnerTimer_SecondElapsed;
			timer.Finished -= InnerTimer_TimerFinished;
			timer.Started -= InnerTimer_Started;
			timer.Stopped -= InnerTimer_Stopped;
			timer.Resumed -= InnerTimer_Resumed;
			timer.Reseted -= InnerTimer_Reseted;
			timer.Disposed -= InnerTimer_Disposed;
		}

		#endregion
	}
}
