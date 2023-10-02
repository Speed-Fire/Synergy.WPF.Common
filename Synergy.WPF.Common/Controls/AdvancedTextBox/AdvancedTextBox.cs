using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Synergy.WPF.Common.Controls
{
    [TemplatePart(Name = PART_Placeholder, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_ContentHost, Type = typeof(ScrollViewer))]
    public class AdvancedTextBox : TextBox
    {
        private const string PART_Placeholder = "PART_Placeholder";
        private const string PART_ContentHost = "PART_ContentHost";

        #region Members

        private TextBlock _placeholder;
        private ScrollViewer _contentHost;

        private volatile bool _mustBeHidden;

        #endregion

        #region Properties

        #region PlaceholderBrush

        public static DependencyProperty PlaceholderBrushProperty =
            DependencyProperty.Register("PlaceholderBrush", typeof(Brush), typeof(AdvancedTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Brush PlaceholderBrush
        {
            get => (Brush)GetValue(PlaceholderBrushProperty);
            set => SetValue(PlaceholderBrushProperty, value);
        }

        #endregion

        #region Placeholder

        public static DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(AdvancedTextBox), new PropertyMetadata(null));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        #endregion

        #region CornerRadius

        public static DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(AdvancedTextBox), new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #endregion

        #region Constructors

        static AdvancedTextBox()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(typeof(AdvancedTextBox), new FrameworkPropertyMetadata(typeof(AdvancedTextBox)));
        }

        public AdvancedTextBox()
        {
            _mustBeHidden = false;
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _placeholder = GetTemplateChild(PART_Placeholder) as TextBlock;
            _contentHost = GetTemplateChild(PART_ContentHost) as ScrollViewer;

            _contentHost.Padding = new Thickness(0);

            if(_mustBeHidden)
                _placeholder.Visibility = Visibility.Collapsed;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            var isEmpty = string.IsNullOrEmpty(Text);

            if (_placeholder is null && !isEmpty)
            {
                _mustBeHidden = true;
                return;
            }

            if (isEmpty)
            {
                _placeholder.Visibility = Visibility.Visible;
            }
            else
            {
                _placeholder.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
