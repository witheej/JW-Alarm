using Autofac;
using JW.Alarm.ViewModels;
using Windows.UI.Xaml.Controls;


namespace JW.Alarm.Core.Uwp
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MainViewModel = IocSetup.Container.Resolve<MainViewModel>();
        }

        public MainViewModel MainViewModel { get; set; }

    }
}
