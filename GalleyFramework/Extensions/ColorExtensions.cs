using Xamarin.Forms;

namespace GalleyFramework.Extensions
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255), (int)(color.A * 255));
        }

        public static string ToHexNoAlpha(this Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", (int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255));
        }
    }
}