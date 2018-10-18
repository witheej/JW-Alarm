using Autofac;
using JW.Alarm.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


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
    }

   
}
