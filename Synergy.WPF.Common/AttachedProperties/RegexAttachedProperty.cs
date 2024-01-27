using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Synergy.WPF.Common.AttachedProperties
{
	public class RegexAttachedProperty : BaseAttachedProperty<RegexAttachedProperty, string>
	{
		private class TBData
		{
			public Regex Regex { get; set; }
			public TextCompositionEventHandler Action { get; set; }
		}

		private static readonly Dictionary<TextBoxBase, TBData> _dict = new();

		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is not TextBoxBase tb)
				return;

			if (_dict.TryGetValue(tb, out TBData value))
			{
				tb.PreviewTextInput -= value.Action;
				_dict.Remove(tb);
			}

			var pattern = (string)e.NewValue;
			var regex = new Regex(pattern);

			TextCompositionEventHandler action = (sender, e) =>
			{
				if (!regex.IsMatch(e.Text))
					e.Handled = true;
			};

			tb.PreviewTextInput += action;

			_dict[tb] = new() { Regex = regex, Action = action };
		}
	}
}
