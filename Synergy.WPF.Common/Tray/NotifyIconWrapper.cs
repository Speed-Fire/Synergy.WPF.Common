using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Reflection;

namespace Synergy.WPF.Common.Tray
{
    public class NotifyIconWrapper : FrameworkElement, IDisposable
    {
        public class NotifyRequestRecord
        {
            public string Title { get; set; } = "";
            public string Message { get; set; } = "";
            public int Duretion { get; set; } = 1000;
            public ToolTipIcon Icon { get; set; } = ToolTipIcon.None;
        }

        #region Members

        private readonly NotifyIcon _notifyIcon;

        #endregion

        #region Properties

        #region Text

        public static readonly DependencyProperty TextProperty=
            DependencyProperty.Register("Text", typeof(string), typeof(NotifyIconWrapper), new PropertyMetadata(OnTextChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var notifyIcon = ((NotifyIconWrapper)d)._notifyIcon;

            if (notifyIcon is null)
                return;

            notifyIcon.Text = e.NewValue as string;
        }

        #endregion

        #region NotifyRequest

        public static readonly DependencyProperty NotifyRequestProperty =
            DependencyProperty.Register("NotifyRequest", typeof(NotifyRequestRecord), typeof(NotifyIconWrapper), new PropertyMetadata(OnNotifyRequestChanged));

        public NotifyRequestRecord NotifyRequest
        {
            get => (NotifyRequestRecord)GetValue(NotifyRequestProperty);
            set => SetValue(NotifyRequestProperty, value);
        }

        private static void OnNotifyRequestChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var req = (NotifyRequestRecord)e.NewValue;
            ((NotifyIconWrapper)d)?._notifyIcon?.ShowBalloonTip(req.Duretion, req.Title, req.Message, req.Icon);
        }

        #endregion

        #endregion

        #region Event

        #region OpenSelectedEvent

        public static readonly RoutedEvent OpenSelectedEvent =
            EventManager.RegisterRoutedEvent("OpenSelected", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(NotifyIconWrapper));

        public event RoutedEventHandler OpenSelected
        {
            add => AddHandler(OpenSelectedEvent, value);
            remove => RemoveHandler(OpenSelectedEvent, value);
        }

        #endregion

        #region ExitSelectedEvent

        public static readonly RoutedEvent ExitSelectedEvent =
            EventManager.RegisterRoutedEvent("ExitSelected", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(NotifyIconWrapper));

        public event RoutedEventHandler ExitSelected
        {
            add => AddHandler(ExitSelectedEvent, value);
            remove => RemoveHandler(ExitSelectedEvent, value);
        }

        #endregion

        #endregion

        #region Constructors

        public NotifyIconWrapper()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _notifyIcon = new()
            {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visible = true,
                ContextMenuStrip = CreateContextMenu()
            };

            _notifyIcon.DoubleClick += OpenItemOnClick;
            System.Windows.Application.Current.Exit += (obj, e) =>
            {
                Dispose();
            };
        }

        #endregion

        #region Methods

        private ContextMenuStrip CreateContextMenu()
        {
            var openItem = new ToolStripMenuItem("Open");
            openItem.Click += OpenItemOnClick;

            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += ExitItemOnClick;

            return new() { Items = { openItem, exitItem } };
        }

        #endregion

        #region Event handlers

        private void OpenItemOnClick(object sender, EventArgs e)
        {
            var args = new RoutedEventArgs(OpenSelectedEvent);
            RaiseEvent(args);
        }

        private void ExitItemOnClick(object sender, EventArgs e)
        {
            var args = new RoutedEventArgs(ExitSelectedEvent);
            RaiseEvent(args);
        }

        #endregion

        public void Dispose()
        {
            _notifyIcon?.Dispose();
        }
    }
}
