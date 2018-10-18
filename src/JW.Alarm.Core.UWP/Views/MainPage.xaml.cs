using Autofac;
using JW.Alarm.Core.UWP;
using JW.Alarm.Services.Contracts;
using JW.Alarm.ViewModels;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace JW.Alarm.Core.Uwp
{

    public sealed partial class MainPage : Page
    {
        public MainViewModel MainViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            customizeTitleBar();

            MainViewModel = IocSetup.Container.Resolve<MainViewModel>();

            NavFrame.Loaded += OnLoad;
            NavFrame.Navigated += OnNavigated;
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await MainViewModel.GetScheduleListAsync();
        }

        private async void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(MainPage))
            {
                await MainViewModel.GetScheduleListAsync();
            }
        }

        private void customizeTitleBar()
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

        private void addScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            NavFrame.Navigate(typeof(ScheduleView), new ScheduleViewModel(IocSetup.Container.Resolve<IScheduleService>()));
        }

        private void SchedulesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = (sender as ListView).SelectedItem as Grid;
            var listViewItem = grid.FindVisualAncestor<ListViewItem>();
            NavFrame.Navigate(typeof(ScheduleView), listViewItem.Content as ScheduleViewModel);
        }
    }


}
