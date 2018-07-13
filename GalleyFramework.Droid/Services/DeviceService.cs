// 1211
using System;
using System.Text;
using Android.OS;
using GalleyFramework.Droid.Services;
using GalleyFramework.Helpers.Interfaces.Services;
using Xamarin.Forms;
using Android.Content.Res;
using Xamarin.Forms.Internals;

[assembly: Dependency(typeof(DeviceService))]
namespace GalleyFramework.Droid.Services
{
    [Preserve(AllMembers = true)]
    public class DeviceService : IDeviceService
    {
        public GalleyDeviceInfo Info
        {
            get => new GalleyDeviceInfo
            {
                Model = DeviceExtensions.GetModelName(),
                SystemVersion = $"{Build.VERSION.Release}/{Build.VERSION.Sdk}",
                ScreenScale = Resources.System.DisplayMetrics.Density,
                ScreenWidth = ConvertPixelsToDp(Resources.System.DisplayMetrics.WidthPixels),
                ScreenHeight = ConvertPixelsToDp(Resources.System.DisplayMetrics.HeightPixels)
            };
        }

        private double ConvertPixelsToDp(float pixelValue)
        => ((pixelValue) / Resources.System.DisplayMetrics.Density);
    }

    public static class DeviceExtensions
    {
        public static String GetModelName()
        {
            var manufacturer = Build.Manufacturer;
            var model = Build.Model;
            if (model.StartsWith(manufacturer, StringComparison.InvariantCultureIgnoreCase))
            {
                return Capitalize(model);
            }
            return $"{Capitalize(manufacturer)} {model}";
        }

        private static String Capitalize(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            var arr = str.ToCharArray();
            var capitalizeNext = true;

            var phrase = new StringBuilder();
            foreach (var c in arr)
            {
                if (capitalizeNext && char.IsLetter(c))
                {
                    phrase.Append(char.ToUpper(c));
                    capitalizeNext = false;
                    continue;
                }

                if (char.IsWhiteSpace(c))
                {
                    capitalizeNext = true;
                }
                phrase.Append(c);
            }

            return phrase.ToString();
        }
    }
}
