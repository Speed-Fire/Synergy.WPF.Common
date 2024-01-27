using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Synergy.WPF.Common.AttachedProperties
{
	public class NoWhitespaceAttachedProperty : BaseAttachedProperty<NoWhitespaceAttachedProperty, bool>
	{
		private static readonly Dictionary<TextBox, bool> _dict = new();

		public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender is not TextBox tbb)
				return;

			if (_dict.ContainsKey(tbb))
				return;

			_dict.Add(tbb, false);

			tbb.TextChanged += (sender, e) =>
			{
				if (_dict[tbb])
					return;

				_dict[tbb] = true;

				var txt = tbb.Text;

				while (txt.Contains(' '))
					txt = txt.Replace(" ", string.Empty);

				while (txt.Contains('\n'))
					txt = txt.Replace("\n", string.Empty);

				while (txt.Contains('\t'))
					txt = txt.Replace("\t", string.Empty);

				tbb.Text = txt;

				_dict[tbb] = false;
			};
		}
	}
}
