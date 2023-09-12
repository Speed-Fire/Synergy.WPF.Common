using Synergy.WPF.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Synergy.WPF.Common.Controls.NotifyingGrid
{
    public class NotifyingGrid : Grid
    {
        private static Dictionary<string, WeakReference<NotifyingGrid>> _notifyingGrids = new();

        static NotifyingGrid()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(typeof(NotifyingGrid),
                                  new FrameworkPropertyMetadata(typeof(NotifyingGrid)));
        }

        #region Dependency properties

        public static readonly DependencyProperty NtfNameProperty =
            DependencyProperty.Register("NtfName", typeof(string), typeof(NotifyingGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty NtfBackgroundProperty =
            DependencyProperty.Register("NtfBackground", typeof(Brush), typeof(NotifyingGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty NtfForegroundProperty =
            DependencyProperty.Register("NtfForeground", typeof(Brush), typeof(NotifyingGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty NtfZIndexProperty =
            DependencyProperty.Register("NtfZIndex", typeof(int), typeof(NotifyingGrid), new PropertyMetadata(10));

        public string NtfName
        {
            get => (string)GetValue(NtfNameProperty);
            set => SetValue(NtfNameProperty, value);
        }

        public Brush NtfBackground
        {
            get => (Brush)GetValue(NtfBackgroundProperty);
            set => SetValue(NtfBackgroundProperty, value);
        }

        public Brush NtfForeground
        {
            get => (Brush)GetValue(NtfForegroundProperty);
            set => SetValue(NtfForegroundProperty, value);
        }

        public int NtfZIndex
        {
            get => (int)GetValue(NtfZIndexProperty);
            set => SetValue(NtfZIndexProperty, value);
        }

        #endregion

        #region Notification view fields

        private Grid _innerGrid;

        private TextBlock _title;
        private TextBlock _description;

        private Grid _buttonGrid;
        private NormalButton _button1;
        private NormalButton _button2;
        private NormalButton _button3;

        private volatile bool _btn1Clicked = false;
        private volatile bool _btn2Clicked = false;
        private volatile bool _btn3Clicked = false;

        #endregion

        #region Overrides

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            InitNotificationView();

            Register(this);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            if (this.ColumnDefinitions.Count > 0)
                _innerGrid?.SetValue(Grid.ColumnSpanProperty, this.ColumnDefinitions.Count);
            if (this.RowDefinitions.Count > 0)
                _innerGrid?.SetValue(Grid.RowSpanProperty, this.RowDefinitions.Count);
        }

        #endregion

        #region Notification showing

        private void InitNotificationView()
        {
            _innerGrid = new Grid();
            HideInnerGrid(true);

            var backgroundGrid = new Grid()
            {
                Opacity = 0.5,
                Background = new SolidColorBrush(Colors.DarkGray)
            };
            backgroundGrid.SetValue(Grid.ColumnSpanProperty, 3);
            backgroundGrid.SetValue(Grid.RowSpanProperty, 3);
            backgroundGrid.SetValue(Panel.ZIndexProperty, 0);
            _innerGrid.Children.Add(backgroundGrid);


            if (this.ColumnDefinitions.Count > 0)
                _innerGrid.SetValue(Grid.ColumnSpanProperty, this.ColumnDefinitions.Count);
            if (this.RowDefinitions.Count > 0)
                _innerGrid.SetValue(Grid.RowSpanProperty, this.RowDefinitions.Count);
            _innerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _innerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _innerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _innerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _innerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _innerGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });


            var border = new Border
            {
                Background = NtfBackground,
                CornerRadius = new CornerRadius(15)
            };
            border.SetValue(Grid.ColumnProperty, 1);
            border.SetValue(Grid.RowProperty, 1);
            border.SetValue(Panel.ZIndexProperty, 1);


            var borderGrid = new Grid();
            borderGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // title
            borderGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // separator
            borderGrid.RowDefinitions.Add(new RowDefinition() { Height =
                new GridLength(1, GridUnitType.Star) }); // description
            borderGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // separator
            borderGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); // buttons

            #region Border grid elements

            _title = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = TextAlignment.Center,
                Foreground = NtfForeground,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
            };
            _title.SetValue(Grid.RowProperty, 0);


            var separator1 = new Rectangle
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                MinHeight = 2,
                Fill = new SolidColorBrush(Colors.Gray)
            };
            separator1.SetValue(Grid.RowProperty, 1);


            _description = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = TextAlignment.Left,
                Foreground = NtfForeground,
                FontSize = 14,
                Margin = new Thickness(10, 2, 10, 2)
            };
            _description.SetValue(Grid.RowProperty, 2);


            var separator2 = new Rectangle
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                MinHeight = 2,
                Fill = new SolidColorBrush(Colors.Gray)
            };
            separator2.SetValue(Grid.RowProperty, 3);

            #region Buttons
            _buttonGrid = new Grid();
            _buttonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _buttonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _buttonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _buttonGrid.SetValue(Grid.RowProperty, 4);

            _button1 = new NormalButton()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Colors.LightSkyBlue),
                Style = (Style)Application.Current.TryFindResource("NormalButtonStyle"),
                BorderThickness = new Thickness(0),
                MinHeight = 30,
                MinWidth = 0,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = new TextBlock()
                {
                    FontSize = 14
                }
            };
            _button1.SetValue(Grid.ColumnProperty, 0);

            _button2 = new NormalButton()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Colors.LightSkyBlue),
                Style = (Style)Application.Current.TryFindResource("NormalButtonStyle"),
                BorderThickness = new Thickness(0),
                MinHeight = 30,
                MinWidth = 0,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = new TextBlock()
                {
                    FontSize = 14,
                }
            };
            _button2.SetValue(Grid.ColumnProperty, 1);

            _button3 = new NormalButton()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Colors.LightSkyBlue),
                Style = (Style)Application.Current.TryFindResource("NormalButtonStyle"),
                BorderThickness = new Thickness(0),
                MinHeight = 30,
                MinWidth = 0,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Content = new TextBlock()
                {
                    FontSize = 14,
                }
            };
            _button3.SetValue(Grid.ColumnProperty, 2);

            _buttonGrid.Children.Add(_button1);
            _buttonGrid.Children.Add(_button2);
            _buttonGrid.Children.Add(_button3);

            #endregion

            borderGrid.Children.Add(_title);
            borderGrid.Children.Add(separator1);
            borderGrid.Children.Add(_description);
            borderGrid.Children.Add(separator2);
            borderGrid.Children.Add(_buttonGrid);

            #endregion

            border.Child = borderGrid;
            _innerGrid.Children.Add(border);

            _innerGrid.SetValue(Panel.ZIndexProperty, 10);

            DisableButton(_button1, true);
            DisableButton(_button2, true);
            DisableButton(_button3, true);

            this.Children.Add(_innerGrid);
        }

        internal async Task<MessageBoxResult> ShowNotificationAsync(string title, string message, MessageBoxButton buttons)
        {
            _title.Text = title;
            _description.Text = message;

            PrepareButtons(buttons);

            HideInnerGrid(false);

            await Task.Run(() =>
            {
                while(!(_btn1Clicked || _btn2Clicked || _btn3Clicked)) { }
            });

            HideInnerGrid(true);

            var res = GetButtonResult(buttons);

            return res;
        }

        private void PrepareButtons(MessageBoxButton buttons)
        {
            _btn1Clicked = _btn2Clicked = _btn3Clicked = false;

            var btn1TB = (TextBlock)_button1.Content;
            var btn2TB = (TextBlock)_button2.Content;
            var btn3TB = (TextBlock)_button3.Content;

            switch (buttons)
            {
                case MessageBoxButton.OK:
                    {
                        DisableButton(_button1, false);

                        btn1TB.Text = ButtonStrings.GetStringOK();

                        _button1.Click += _button1_Click;

                        _buttonGrid.ColumnDefinitions[1].Width = GridLength.Auto;
                        _buttonGrid.ColumnDefinitions[2].Width = GridLength.Auto;
                    }
                    break;
                case MessageBoxButton.OKCancel:
                    {
                        DisableButton(_button1, false);
                        DisableButton(_button2, false);

                        btn1TB.Text = ButtonStrings.GetStringOK();
                        btn2TB.Text = ButtonStrings.GetStringCancel();

                        _button1.Click += _button1_Click;
                        _button2.Click += _button2_Click;

                        _buttonGrid.ColumnDefinitions[2].Width = GridLength.Auto;
                    }
                    break;
                case MessageBoxButton.YesNoCancel:
                    {
                        DisableButton(_button1, false);
                        DisableButton(_button2, false);
                        DisableButton(_button3, false);

                        btn1TB.Text = ButtonStrings.GetStringYes();
                        btn2TB.Text = ButtonStrings.GetStringNo();
                        btn3TB.Text = ButtonStrings.GetStringCancel();

                        _button1.Click += _button1_Click;
                        _button2.Click += _button2_Click;
                        _button3.Click += _button3_Click;
                    }
                    break;
                case MessageBoxButton.YesNo:
                    {
                        DisableButton(_button1, false);
                        DisableButton(_button2, false);

                        btn1TB.Text = ButtonStrings.GetStringYes();
                        btn2TB.Text = ButtonStrings.GetStringNo();

                        _button1.Click += _button1_Click;
                        _button2.Click += _button2_Click;

                        _buttonGrid.ColumnDefinitions[2].Width = GridLength.Auto;
                    }
                    break;
            }
        }

        private MessageBoxResult GetButtonResult(MessageBoxButton buttons)
        {
            var btn1TB = (TextBlock)_button1.Content;
            var btn2TB = (TextBlock)_button2.Content;
            var btn3TB = (TextBlock)_button3.Content;

            MessageBoxResult res = MessageBoxResult.None;

            switch (buttons)
            {
                case MessageBoxButton.OK:
                    {
                        DisableButton(_button1, true);

                        btn1TB.Text = string.Empty;

                        _button1.Click -= _button1_Click;

                        _buttonGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                        _buttonGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                        res = MessageBoxResult.OK;
                    }
                    break;
                case MessageBoxButton.OKCancel:
                    {
                        DisableButton(_button1, true);
                        DisableButton(_button2, true);

                        btn1TB.Text = string.Empty;
                        btn2TB.Text = string.Empty;

                        _button1.Click -= _button1_Click;
                        _button2.Click -= _button2_Click;

                        _buttonGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                        res = _btn1Clicked ? MessageBoxResult.OK : MessageBoxResult.Cancel;
                    }
                    break;
                case MessageBoxButton.YesNoCancel:
                    {
                        DisableButton(_button1, true);
                        DisableButton(_button2, true);
                        DisableButton(_button3, true);

                        btn1TB.Text = string.Empty;
                        btn2TB.Text = string.Empty;
                        btn3TB.Text = string.Empty;

                        _button1.Click -= _button1_Click;
                        _button2.Click -= _button2_Click;
                        _button3.Click -= _button3_Click;

                        res = _btn1Clicked ? MessageBoxResult.Yes :
                            (_btn2Clicked ? MessageBoxResult.No : MessageBoxResult.Cancel);
                    }
                    break;
                case MessageBoxButton.YesNo:
                    {
                        DisableButton(_button1, true);
                        DisableButton(_button2, true);

                        btn1TB.Text = string.Empty;
                        btn2TB.Text = string.Empty;

                        _button1.Click -= _button1_Click;
                        _button2.Click -= _button2_Click;

                        _buttonGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                        res = _btn1Clicked ? MessageBoxResult.Yes : MessageBoxResult.No;
                    }
                    break;
            }

            _btn1Clicked = _btn2Clicked = _btn3Clicked = false;

            return res;
        }

        private void DisableButton(Button button, bool isDisabled)
        {
            button.IsEnabled = !isDisabled;
            button.Visibility = isDisabled ? Visibility.Collapsed : Visibility.Visible;
        }

        private void HideInnerGrid(bool hide)
        {
            _innerGrid.IsEnabled = !hide;
            _innerGrid.Visibility = hide ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion

        #region Button click event handlers

        private object _lock = new();

        private void _button1_Click(object sender, RoutedEventArgs e)
        {
            lock (_lock)
            {
                if (_btn2Clicked || _btn3Clicked)
                    return;

                _btn1Clicked = true;
            }
        }

        private void _button2_Click(object sender, RoutedEventArgs e)
        {
            lock (_lock)
            {
                if (_btn1Clicked || _btn3Clicked)
                    return;

                _btn2Clicked = true;
            }
        }

        private void _button3_Click(object sender, RoutedEventArgs e)
        {
            lock (_lock)
            {
                if (_btn2Clicked || _btn1Clicked)
                    return;

                _btn3Clicked = true;
            }
        }

        #endregion

        #region Static methods

        private static void Register(NotifyingGrid grid)
        {
            var name = grid.NtfName;

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("NotifyingGrid's name is empty or equals to null.");

            if (_notifyingGrids.ContainsKey(name))
            {
                if (_notifyingGrids[name].TryGetTarget(out _))
                {
                    throw new InvalidOperationException($"NotifyingGrid with name {name} is already registered!");
                }
            }

            _notifyingGrids[name] = new WeakReference<NotifyingGrid>(grid);
        }

        public static async Task<MessageBoxResult> ShowNotificationAsync(string gridName, string title, string message, MessageBoxButton buttons)
        {
            NotifyingGrid grid = null;

            _notifyingGrids[gridName].TryGetTarget(out grid);

            return await grid.ShowNotificationAsync(title, message, buttons);
        }

        #endregion
    }
}
