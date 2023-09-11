# Synergy.WPF.Common
My custom common elements for WPF.

Before building your app, make sure, that you added following lines to your App.xaml: 
```
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/Synergy.WPF.Common;component/Themes/Generic.xaml"/>
            <ResourceDictionary Source="pack://application:,,,/Synergy.WPF.Common;component/Styles/Synergy.WPF.Common.Shared.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```
