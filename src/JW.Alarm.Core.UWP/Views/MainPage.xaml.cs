using Autofac;
using JW.Alarm.ViewModels;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace JW.Alarm.Core.Uwp
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MainViewModel = IocSetup.Container.Resolve<MainViewModel>();
            CustomizeTitleBar();
        }

        public MainViewModel MainViewModel { get; set; }
        private void CustomizeTitleBar()
        {
            // customize title area
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(customTitleBar);

            // customize buttons' colors
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.SlateBlue;
            titleBar.ButtonBackgroundColor = Colors.SlateBlue;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonForegroundColor = Colors.White;
        }

        private void Toggle_IsEnabled_Toggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
            {
                var listViewItem = toggleSwitch.FindVisualAncestor<ListViewItem>();

                if (listViewItem != null)
                {
                    var schedule = listViewItem.Content as ScheduleViewModel;

                    if (schedule != null)
                    {
                        if (toggleSwitch.IsOn == true)
                        {
                            schedule.EnableCommand.Execute(null);
                        }
                        else
                        {
                            schedule.DisableCommand.Execute(null);
                        }
                    }
                }
            }

        }
    }

   
}
