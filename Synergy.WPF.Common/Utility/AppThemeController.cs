using Synergy.WPF.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Synergy.WPF.Common.Utility
{
    public class AppThemeController
    {
        private readonly Dictionary<string, string> _themesDictionary;
        public IEnumerable<string> Themes => _themesDictionary.Keys;

        private ResourceDictionary CurrentTheme { get; set; } = null;

        private string currentThemeName = "";
        public string CurrentThemeName
        {
            get
            {
                return currentThemeName;
            }
            private set
            {
                currentThemeName = value;
            }
        }

        public AppThemeController()
        {
            _themesDictionary = new()
            {
                {"Light", "pack://application:,,,/Synergy.WPF.Common;component/Themes/Light.xaml" },
                {"Dark", "pack://application:,,,/Synergy.WPF.Common;component/Themes/Dark.xaml" }
            };
        }

        /// <summary>
        /// Add theme to app theme collection.
        /// </summary>
        /// <param name="themeName">Name of your theme.</param>
        /// <param name="themePackPath">Theme application path (Example: "pack://application:,,,/Themes/Light.xaml")</param>
        /// <returns>True if theme is successful added, otherwise - false.</returns>
        public bool AddTheme(string themeName, string themePackPath)
        {
            if (_themesDictionary.ContainsKey(themeName))
                return false;

            _themesDictionary[themeName] = themePackPath;
            return true;
        }

        /// <summary>
        /// Set theme specified by themeName.
        /// </summary>
        /// <param name="themeName">Theme name.</param>
        /// <exception cref="ThemeNotFoundException">Theme name is not registered.</exception>
        public void SetTheme(string themeName)
        {
            if (!_themesDictionary.ContainsKey(themeName))
                throw new ThemeNotFoundException(themeName);

            if (CurrentThemeName == themeName) return;

            if (CurrentTheme != null)
                Application.Current.Resources.MergedDictionaries.Remove(CurrentTheme);

            CurrentTheme = new ResourceDictionary();

            CurrentTheme.Source = new Uri(_themesDictionary[themeName]);
            CurrentThemeName = themeName;

            Application.Current.Resources.MergedDictionaries.Add(CurrentTheme);
        }
    }
}
