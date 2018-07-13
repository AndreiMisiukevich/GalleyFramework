using GalleyFramework.Extensions;
using Xamarin.Forms;
namespace GalleyFramework.Views.Controls
{
    public class GalleyLoadingOverlay : AbsoluteLayout
    {
        public GalleyLoadingOverlay()
        {
            BackgroundColor = Color.Black.MultiplyAlpha(.3);

            Children.Add(new ActivityIndicator
            {
                Color = Color.White,
                IsRunning = true
            }.WithAbsCenter());

            this.WithAbsFill();
        }
    }
}
