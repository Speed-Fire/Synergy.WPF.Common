using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Synergy.WPF.Common.Controls
{
	[TemplatePart(Name = PART_TipsList, Type = typeof(ListBox))]
	[TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
	[TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
	[TemplatePart(Name = PART_ToggleButton, Type = typeof(ToggleButton))]
	public class SearchBox : Control
	{
		private const string PART_TipsList = "PART_TipsList";
		private const string PART_Popup = "PART_Popup";
		private const string PART_TextBox = "PART_TextBox";
		private const string PART_ToggleButton = "PART_ToggleButton";

		private ListBox _tipsList;
		private Popup _popup;
		private TextBox _textBox;
		private ToggleButton _toggleButton;

		private IEnumerable<object> _casted;

		private int _offset = 0;
		private int _count = 0;

		#region Properties

		#region Text

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(SearchBox),
				new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public string Text
		{
			get => (string)/*_textBox.*/GetValue(TextProperty);
			set => /*_textBox.*/SetValue(TextProperty, value);
		}

		#endregion

		#region GapSize

		public static readonly DependencyProperty GapSizeProperty =
			DependencyProperty.Register("GapSize", typeof(int), typeof(SearchBox), new PropertyMetadata(5, OnGapSizeChanged));

		public int GapSize
		{
			get => (int)GetValue(GapSizeProperty);
			set => SetValue(GapSizeProperty, value);
		}

		private static void OnGapSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var searchbox = (SearchBox)d;
			searchbox?.OnGapSizeChanged((int)e.OldValue, (int)e.NewValue);
		}

		private void OnGapSizeChanged(int oldValue, int newValue)
		{
			if (_tipsList is null || ItemsSource is null || oldValue.Equals(newValue))
				return;

			_offset = 0;
			_tipsList.ItemsSource = ItemsSource.Cast<object>().Skip(_offset).Take(newValue);
		}

		#endregion

		#region ItemsSource

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(SearchBox), new PropertyMetadata(default, OnItemsSourceChanged));

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var searchbox = (SearchBox)d;
			searchbox?.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
		}

		private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			if(_tipsList is null || (oldValue is not null && oldValue.Equals(newValue)))
				return;

			_offset = 0;
			_count = 0;

			if (newValue is null)
			{
				_tipsList.ItemsSource = null;
				return;
			}

			_casted = newValue.Cast<object>();
			_count = _casted.Count();
			_tipsList.ItemsSource = _casted.Skip(_offset).Take(GapSize);
		}

		#endregion

		#region TextBoxStyle

		public static readonly DependencyProperty TextBoxStyleProperty =
			DependencyProperty.Register("TextBoxStyle", typeof(Style), typeof(SearchBox), new PropertyMetadata(default, OnTextBoxStyleChanged));

		public Style TextBoxStyle
		{
			get => (Style)GetValue(TextBoxStyleProperty);
			set => SetValue(TextBoxStyleProperty, value);
		}

		private static void OnTextBoxStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var searchbox = (SearchBox)d;
			searchbox?.OnTextBoxStyleChanged((Style)e.OldValue, (Style)e.NewValue);
		}

		private void OnTextBoxStyleChanged(Style oldValue, Style newValue)
		{
			if (_textBox is null || oldValue.Equals(newValue))
				return;

			_textBox.Style = newValue;
		}

		#endregion

		#region Regex

		public static readonly DependencyProperty RegexProperty =
			DependencyProperty.Register("Regex", typeof(string), typeof(SearchBox), new PropertyMetadata(default));

		public string Regex
		{
			get => (string)GetValue(RegexProperty);
			set => SetValue(RegexProperty, value);
		}

		#endregion

		#region Blocker

		public static readonly DependencyProperty BlockerProperty =
			DependencyProperty.Register("Blocker", typeof(bool), typeof(SearchBox), new PropertyMetadata(false));

		public bool Blocker
		{
			get => (bool)GetValue(BlockerProperty);
			set => SetValue(BlockerProperty, value);
		}

		#endregion

		#endregion

		static SearchBox()
		{
			DefaultStyleKeyProperty
				.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (_popup is not null)
				_popup.PreviewMouseWheel -= Popup_MouseWheel;

			_popup = GetTemplateChild(PART_Popup) as Popup;
			_popup.PreviewMouseWheel += Popup_MouseWheel;

			if(_tipsList is not null)
			{
				_tipsList.SelectionChanged -= TipsList_SelectionChanged;
			}

			_tipsList = GetTemplateChild(PART_TipsList) as ListBox;
			_tipsList.SelectionChanged += TipsList_SelectionChanged;

			if (_textBox is not null)
			{
				_textBox.PreviewTextInput -= TextBox_PreviewTextInput;
				_textBox.TextChanged -= TextBox_TextChanged;
				_textBox.LostFocus -= TextBox_LostFocus;
				_textBox.LostKeyboardFocus -= TextBox_LostKeyboardFocus;
				_textBox.LayoutUpdated -= TextBox_LayoutUpdated;
				_textBox.MouseWheel -= Popup_MouseWheel;
			}

			_textBox = GetTemplateChild(PART_TextBox) as TextBox;
			_textBox.Style = TextBoxStyle;
			_textBox.PreviewTextInput += TextBox_PreviewTextInput;
			_textBox.TextChanged += TextBox_TextChanged;
			_textBox.LostFocus += TextBox_LostFocus;
			_textBox.LostKeyboardFocus += TextBox_LostKeyboardFocus;
			_textBox.LayoutUpdated += TextBox_LayoutUpdated;
			_textBox.MouseWheel += Popup_MouseWheel;

			if (_toggleButton is not null)
			{
				_toggleButton.Checked -= ToggleButton_Checked;
				_toggleButton.Unchecked -= ToggleButton_Unchecked;
			}

			_toggleButton = GetTemplateChild(PART_ToggleButton) as ToggleButton;
			_toggleButton.Focusable = false;
			_toggleButton.Checked += ToggleButton_Checked;
			_toggleButton.Unchecked += ToggleButton_Unchecked;
			_toggleButton.IsChecked = false;
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			_popup.IsOpen = true;
		}

		private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
		{
			_popup.IsOpen = false;
		}

		private void TextBox_LayoutUpdated(object sender, EventArgs e)
		{
			if (_popup.IsOpen)
			{
				_popup.HorizontalOffset += 1;
				_popup.HorizontalOffset -= 1;
			}
		}

		private void TextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
		{
			_toggleButton.IsChecked = false;
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			_toggleButton.IsChecked = false;
		}

		private void TipsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(_tipsList.SelectedIndex < 0) return;

			Blocker = true;

			var tip = _tipsList.SelectedItem;
			_textBox.Text = tip.ToString();

			_toggleButton.IsChecked = false;
			Blocker = false;
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			Text = _textBox.Text;

			if (!string.IsNullOrEmpty(_textBox.Text) && !Blocker && !_popup.IsOpen)
				_toggleButton.IsChecked = true;
		}

		private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(Regex))
				return;

			var regex = new Regex(Regex);

			if (!regex.IsMatch(e.Text))
				e.Handled = true;
		}

		private readonly object _locker = new();

		private void Popup_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
		{
			lock (_locker)
			{
				if (e.Delta < 0)
				{
					if (_offset + GapSize >= _count)
						return;

					_offset++;
				}
				else if (e.Delta > 0)
				{
					if (_offset == 0)
						return;

					_offset--;
				}

				_tipsList.ItemsSource = _casted.Skip(_offset).Take(GapSize);
			}
		}
	}
}
