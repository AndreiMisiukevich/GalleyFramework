// 1711
using System;
using System.Linq;
using System.Threading.Tasks;
using GalleyFramework.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GalleyFramework.Views.Animations
{
    public class GalleySwitchAnimation : IGalleySuperViewAnimation
    {
        private readonly uint _time;

        public GalleySwitchAnimation(uint time = 250)
        {
            _time = time;
        }

        public async Task Apply(GalleyBaseView view, GalleySuperView superView)
        {
            var oldViewIndex = superView.Children.ToArray().IndexOf(superView.CurrentView);
            superView.InsertChildIfNeeded(0, view, !superView.IsAllowedMakingViewsInvisible);
            await superView.SetCurrentView(view, true);
            var newViewIndex = superView.Children.ToArray().IndexOf(superView.CurrentView);

            Task animationTask;
            if (newViewIndex > oldViewIndex)
            {
                view.Opacity = 0;
                view.IsVisible = true;
                animationTask = Task.Run(async () => await view.FadeTo(1, _time));
            }
            else
            {
                view.Opacity = 1;
                view.IsVisible = true;
                animationTask = superView.GetOtherVisibleViews(view).EachAsync(async c => await c.FadeTo(0, _time), false);
            }

            await superView.ResetFloatingControls(view, animationTask);

            superView.SetSuperView(view);
        }
    }
}
