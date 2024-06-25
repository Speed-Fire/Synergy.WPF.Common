using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Synergy.WPF.Common.Controls
{
    public class SliderButton : ToggleButton
    {
        #region Properties

        #region ButtonWidth

        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(double), typeof(SliderButton), new PropertyMetadata(10d));

        public double ButtonWidth
        {
            get => (double)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }

		#endregion

		#region OnLabel

		public static readonly DependencyProperty OnLabelProperty =
            DependencyProperty.Register("OnLabel", typeof(string), typeof(SliderButton), new PropertyMetadata(""));

        public string OnLabel
        {
            get => (string)GetValue(OnLabelProperty);
            set => SetValue(OnLabelProperty, value);
        }

        #endregion

        #region OffLabel

        public static readonly DependencyProperty OffLabelProperty =
            DependencyProperty.Register("OffLabel", typeof(string), typeof(SliderButton), new PropertyMetadata(""));

        public string OffLabel
        {
            get => (string)GetValue(OffLabelProperty);
            set => SetValue(OffLabelProperty, value);
        }

        #endregion

        #region OnColor

        public static readonly DependencyProperty OnColorProperty =
            DependencyProperty.Register("OnColor", typeof(Brush), typeof(SliderButton), new PropertyMetadata(Brushes.LightGreen));

        public Brush OnColor
        {
            get => (Brush)GetValue(OnColorProperty);
            set => SetValue(OnColorProperty, value);
        }

        #endregion

        #region OffColor

        public static readonly DependencyProperty OffColorProperty =
            DependencyProperty.Register("OffColor", typeof(Brush), typeof(SliderButton), new PropertyMetadata(Brushes.Red));

        public Brush OffColor
        {
            get => (Brush)GetValue(OffColorProperty);
            set => SetValue(OffColorProperty, value);
        }

        #endregion

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SliderButton), new PropertyMetadata(new CornerRadius()));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #endregion

        static SliderButton()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(typeof(SliderButton),
                new FrameworkPropertyMetadata(typeof(SliderButton)));
        }
    }
}
