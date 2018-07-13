using System;
using System.Threading.Tasks;
using GalleyFramework.Extensions;
using GalleyFramework.Views;
using GalleyFramework.Views.Animations;
using Xamarin.Forms;

namespace GalleyFramework.Sample.Animations
{
    public class SamplePushAnimation : IGalleySuperViewAnimation
    {
        public async Task Apply(GalleyBaseView view, GalleySuperView superView)
        {
            view.Opacity = 0;
            superView.Children.Add(view);
            view.Layout(superView.Width, superView.Height, superView.Width, superView.Height);
            view.Opacity = 1;
            await superView.ResetFloatingControls(view, view.LayoutTo(0, 0, superView.Width, superView.Height, 500, Easing.Linear));
            await superView.SetCurrentView(view);
            superView.SetSuperView(view);
        }
    }
}
