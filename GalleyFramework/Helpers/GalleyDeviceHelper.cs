// 1211
using System;
using GalleyFramework.Extensions;
using GalleyFramework.Helpers.Interfaces;
using Xamarin.Forms;
using GalleyFramework.Helpers.Interfaces.Services;
using System.Globalization;

namespace GalleyFramework.Helpers
{
    public class GalleyDeviceHelper : IDeviceHelper
    {
        public GalleyDeviceHelper()
        {
            var info = DependencyService.Get<IDeviceService>().Info;
            Os = Device.RuntimePlatform;
            LocaleName = CultureInfo.CurrentCulture.Name;
            SystemVersion = info.SystemVersion;
            Model = info.Model;
            ScreenScale = info.ScreenScale;
            ScreenWidth = info.ScreenWidth;
            ScreenHeight = info.ScreenHeight;
        }

        public string Os { get; private set; }
        public string LocaleName { get; private set; }
        public string SystemVersion { get; private set; }
        public string Model { get; private set; }
        public double ScreenScale { get; private set; }
        public double ScreenWidth { get; private set; }
        public double ScreenHeight { get; private set; }

        public bool OpenDialer(string number)
        => OpenUrl($"tel:{number}");

        public bool OpenSms(string number)
        => OpenUrl($"sms:{number}");

        public bool OpenEmail(string email)
        => OpenUrl($"mailto:{email}");

        public bool OpenUrl(string url)
        {
            if(string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            try
            {
                Device.OpenUri(new Uri(url));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
