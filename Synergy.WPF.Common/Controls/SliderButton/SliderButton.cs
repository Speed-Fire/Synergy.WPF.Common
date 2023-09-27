using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Synergy.WPF.Common.Controls
{
    public class SliderButton : ToggleButton
    {
        #region Properties

        #region OnLabel

        public static DependencyProperty OnLabelProperty =
            DependencyProperty.Register("OnLabel", typeof(string), typeof(SliderButton), new PropertyMetadata(""));

        public string OnLabel
        {
            get => (string)GetValue(OnLabelProperty);
            set => SetValue(OnLabelProperty, value);
        }

        #endregion

        #region OffLabel

        public static DependencyProperty OffLabelProperty =
            DependencyProperty.Register("OffLabel", typeof(string), typeof(SliderButton), new PropertyMetadata(""));

        public string OffLabel
        {
            get => (string)GetValue(OffLabelProperty);
            set => SetValue(OffLabelProperty, value);
        }

        #endregion

        #region OnColor

        public static DependencyProperty OnColorProperty =
            DependencyProperty.Register("OnColor", typeof(Brush), typeof(SliderButton), new PropertyMetadata(Brushes.LightGreen));

        public Brush OnColor
        {
            get => (Brush)GetValue(OnColorProperty);
            set => SetValue(OnColorProperty, value);
        }

        #endregion

        #region OffColor

        public static DependencyProperty OffColorProperty =
            DependencyProperty.Register("OffColor", typeof(Brush), typeof(SliderButton), new PropertyMetadata(Brushes.Red));

        public Brush OffColor
        {
            get => (Brush)GetValue(OffColorProperty);
            set => SetValue(OffColorProperty, value);
        }

        #endregion

        #region CornerRadius

        public static DependencyProperty CornerRadiusProperty =
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
