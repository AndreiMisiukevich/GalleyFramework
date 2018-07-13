using System;
using System.Linq;
using System.Threading.Tasks;
using GalleyFramework.Extensions;
using GalleyFramework.Views.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GalleyFramework.Views.Animations
{
    public class GalleyPopAnimation : IGalleySuperViewAnimation
    {
        private readonly uint _time;

        public GalleyPopAnimation(uint time = 250)
        {
            _time = time;
        }

        public async Task Apply(GalleyBaseView view, GalleySuperView superView)
        {
            superView.InsertChildIfNeeded(0, view);
            await superView.SetCurrentView(view, true);

            await superView.ResetFloatingControls(view, superView.GetOtherVisibleViews(view)
                  .EachAsync(async c => await c.LayoutTo(superView.Width, 0, superView.Width, superView.Height, _time)));

            superView.SetSuperView(view);
        }
    }
}
