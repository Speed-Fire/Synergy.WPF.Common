﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Synergy.WPF.Common.Decorators
{
	public class DpiDecorator : Decorator
	{
		public DpiDecorator()
		{
			this.Loaded += (s, e) =>
			{
				Matrix m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
				ScaleTransform dpiTransform = new ScaleTransform(1 / m.M11 * 1.25, 1 / m.M22 * 1.25);
				if (dpiTransform.CanFreeze)
					dpiTransform.Freeze();
				this.LayoutTransform = dpiTransform;
			};
		}
	}
}
