using System.Linq;
using Android.Graphics.Drawables;
using Android.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GalleyFramework.Extensions;
using GalleyFramework.Views.Controls;
using GalleyFramework.Droid.Renderers;
using Android.Support.V4.Content.Res;
using System;
using GalleyFramework.Droid.Extensions;
using Android.Content;
using Android.Runtime;

[assembly: ExportRenderer(typeof(GalleyButton), typeof(GalleyButtonRenderer))]
namespace GalleyFramework.Droid.Renderers
{
    [Preserve(AllMembers = true)]
    public class GalleyButtonRenderer : ButtonRenderer
    {
        public GalleyButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                SetBackground();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if(e.PropertyName == nameof(GalleyButton.RegularImage) || e.PropertyName == nameof(GalleyButton.PressedImage))
            {
                SetBackground();
            }
        }

        private void SetBackground()
        {
            var element = Element.As<GalleyButton>();

            if ((element.RegularImage ?? element.PressedImage).IsNull())
            {
                Control.AsRounded(element.BackgroundColor.ToAndroid(), element.BorderColor.ToAndroid(), element.BorderRadius, (int)element.BorderWidth);
                return;
            }

            var drawable = new StateListDrawable();

			if (element.PressedImage.NotNull())
			{
                var pressedDrawable = GetResource(element.PressedImage ?? element.RegularImage);
                pressedDrawable.Mutate();
                pressedDrawable.SetAlpha((int)Math.Round(255 * element.PressedOpacity));
                drawable.AddState(new[] { Android.Resource.Attribute.StatePressed }, pressedDrawable);
			}

            if (element.RegularImage.NotNull())
            {
                drawable.AddState(StateSet.WildCard.ToArray(), GetResource(element.RegularImage));
            }
            Control.Background = drawable;
        }

        private Drawable GetResource(string name)
        => ResourcesCompat.GetDrawable(Resources, Resources.GetIdentifier(name, "drawable", Context.PackageName), null);

    }
}