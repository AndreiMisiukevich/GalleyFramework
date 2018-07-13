using Xamarin.Forms;
using GalleyFramework.Extensions;
using GalleyFramework.Infrastructure;
namespace GalleyFramework.Views.Controls
{
    public sealed class GalleyMasterCloseOverlay : ContentView
    {
        public GalleyMasterCloseOverlay()
        {
            this.WithTap(() =>
            {
                (Galley.Navigation.SuperView.ScrollView)?.MoveSideMenu();
            }).WithAbsFlags(AbsoluteLayoutFlags.All)
                .WithAbsBounds(0, 0, 1, 1);
        }
    }
}
