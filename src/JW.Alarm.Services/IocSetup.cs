namespace JW.Alarm.Services
{
    using Autofac;

    public static class IocSetup
    {
        internal static IContainer Container;
        public static void Initialize(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DownloadService>();
            containerBuilder.RegisterType<MediaLookUpService>();
            containerBuilder.RegisterType<MediaService>();
        }

        public static void SetContainer(IContainer iocContainer)
        {
            Container = iocContainer;
        }
    }
}