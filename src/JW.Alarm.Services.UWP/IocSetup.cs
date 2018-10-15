namespace JW.Alarm.Services.Uwp
{
    using Autofac;
    using JW.Alarm.Services.Contracts;
    using JW.Alarm.Services.Uwp;
    using JW.Alarm.Services.Uwp.Storage;
    using JW.Alarm.Services.Uwp.Tasks;
    using System.Net.Http;

    public static class IocSetup
    {
        internal static IContainer Container;
        public static void Initialize(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UwpAlarmService>().As<IAlarmService>();
            containerBuilder.RegisterType<AlarmTask>();
            containerBuilder.RegisterType<SchedulerTask>();
            containerBuilder.RegisterType<HttpClientHandler>();
            containerBuilder.RegisterType<StorageService>().As<IStorageService>();
        }

        public static void SetContainer(IContainer iocContainer)
        {
            Container = iocContainer;
        }
    }
}