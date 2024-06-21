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

namespace Synergy.WPF.Common.Controls
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Synergy.WPF.Common.Controls"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Synergy.WPF.Common.Controls;assembly=Synergy.WPF.Common.Controls"
    ///
    /// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    /// на данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///
    ///
    /// Шаг 2)
    /// Теперь можно использовать элемент управления в файле XAML.
    ///
    ///     <MyNamespace:NavButton/>
    ///
    /// </summary>
    public class NavItem : ListBoxItem
    {
        static NavItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavItem), new FrameworkPropertyMetadata(typeof(NavItem)));
        }

        public static readonly DependencyProperty NavLinkProperty =
            DependencyProperty.Register("NavLink", typeof(Uri), typeof(NavItem), new PropertyMetadata(null));

        public static readonly DependencyProperty NavCommandProperty =
            DependencyProperty.Register("NavCommand", typeof(ICommand), typeof(NavItem), new PropertyMetadata(null));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(NavItem), new PropertyMetadata(null));

        public static readonly DependencyProperty IconColorProperty =
    DependencyProperty.Register("IconColor", typeof(Brush), typeof(NavItem), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(123, 135, 146))));

        public static readonly DependencyProperty HoveredIconColorProperty =
            DependencyProperty.Register("HoveredIconColor", typeof(Brush), typeof(NavItem), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(42, 132, 241))));

        public static readonly DependencyProperty SelectedIconColorProperty =
            DependencyProperty.Register("SelectedIconColor", typeof(Brush), typeof(NavItem), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(42, 132, 241))));


        public Uri NavLink
        {
            get { return (Uri)GetValue(NavLinkProperty); }
            set { SetValue(NavLinkProperty, value); }
        }


        public ICommand NavCommand
        {
            get { return (ICommand)GetValue(NavCommandProperty); }
            set { SetValue(NavCommandProperty, value); }
        }


        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }



        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }



        public Brush HoveredIconColor
        {
            get { return (Brush)GetValue(HoveredIconColorProperty); }
            set { SetValue(HoveredIconColorProperty, value); }
        }



        public Brush SelectedIconColor
        {
            get { return (Brush)GetValue(SelectedIconColorProperty); }
            set { SetValue(SelectedIconColorProperty, value); }
        }

    }
}
