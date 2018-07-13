using System;
using Xamarin.Forms;
using GalleyFramework.iOS.Services;
namespace GalleyFramework.iOS
{
    public static class GF
    {
        public static void Init()
        {
            DependencyService.Register<DialogService>();
            DependencyService.Register<DeviceService>();
        }
    }
}
