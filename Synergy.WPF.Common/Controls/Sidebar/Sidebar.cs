using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#nullable disable

namespace Synergy.WPF.Common.Controls
{
    public class Sidebar : ListBox
    {
        static Sidebar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Sidebar), new FrameworkPropertyMetadata(typeof(Sidebar)));       
        }

        public static readonly DependencyProperty TitleIconProperty =
            DependencyProperty.Register("TitleIcon", typeof(Geometry), typeof(Sidebar), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleIconColorProperty =
            DependencyProperty.Register("TitleIconColor", typeof(SolidColorBrush), typeof(Sidebar), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(42, 132, 241))));

        public static readonly DependencyProperty NavItemsProperty =
            DependencyProperty.Register("NavItems", typeof(IEnumerable<NavItem>), typeof(Sidebar), new PropertyMetadata(new List<NavItem>()));

        public static readonly DependencyProperty OutputProperty =
            DependencyProperty.Register("Output", typeof(Frame), typeof(Sidebar), new PropertyMetadata(null));

        public Geometry TitleIcon
        {
            get { return (Geometry)GetValue(TitleIconProperty); }
            set { SetValue(TitleIconProperty, value); }
        }


        public SolidColorBrush TitleIconColor
        {
            get { return (SolidColorBrush)GetValue(TitleIconColorProperty); }
            set { SetValue(TitleIconColorProperty, value); }
        }


        public IEnumerable<NavItem> NavItems
        {
            get { return (IEnumerable<NavItem>)GetValue(NavItemsProperty); }
            set { SetValue(NavItemsProperty, value); }
        }


        public Frame Output
        {
            get { return (Frame)GetValue(OutputProperty); }
            set { SetValue(OutputProperty, value); }
        }


        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (e.AddedItems.Count == 0)
                return;

            if (!(e.AddedItems[0] is NavItem))
                throw new Exception();

            var navitem = (NavItem)e.AddedItems[0];

            if (Output is null)
                throw new NullReferenceException();

            if (navitem.NavCommand != null)
                navitem.NavCommand.Execute(Output);
            else
                Output.Navigate(navitem.NavLink);
        }
    }
}
