using System;
using GalleyFramework.Helpers.Interfaces;
namespace GalleyFramework.Infrastructure
{
    public static class Galley
    {
        public static event Action Initialized;

        private static readonly object _locker;

        static Galley() 
        {
            _locker = new object();
        }

        public static INavigationHelper Navigation { get; private set; }
        public static IDialogHelper Dialog { get; private set; }
        public static IHttpHelper Http { get; private set; }
        public static ILocalizationHelper Loc { get; private set; }
        public static IDeviceHelper Device { get; private set; }

        public static void SetUp(GalleyInitializer initializer)
        {
			lock (_locker)
			{
                Navigation = initializer.GetNavigationHelper();
				Dialog = initializer.GetDialogHelper();
                Http = initializer.GetHttpHelper();
                Loc = initializer.GetLocalizationHelper();
                Device = initializer.GetDeviceHelper();
                Initialized?.Invoke();
			}   
        }
    }
}
