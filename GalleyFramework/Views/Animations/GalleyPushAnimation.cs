// 1711
using System;
using System.Linq;
using System.Threading.Tasks;
using GalleyFramework.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GalleyFramework.Views.Animations
{
    public class GalleyPushAnimation : IGalleySuperViewAnimation
    {
        private readonly uint _time;

        public GalleyPushAnimation(uint time = 250)
        {
            _time = time;
        }

        public async Task Apply(GalleyBaseView view, GalleySuperView superView)
        {
            view.WidthRequest = superView.Width;
            view.HeightRequest = superView.Height;
            view.TranslationX = superView.Width;
            superView.InsertChildIfNeeded(superView.Children.Count, view);

            var pushTcs = new TaskCompletionSource<bool>();
            new Animation((v) => view.TranslationX = v, superView.Width, 0)
                .Commit(view, "pushAnimation", 16, _time, Easing.Linear, async (v, b) =>
                {
                    await superView.SetCurrentView(view);
                    superView.SetSuperView(view);
                    pushTcs.SetResult(true);
                });

            await superView.ResetFloatingControls(view, pushTcs.Task);
        }
    }
}
