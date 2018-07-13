using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using GalleyFramework.Views;
using GalleyFramework.Droid.Renderers;
using GalleyFramework.Extensions;
using Android.Content;
using Android.Runtime;

[assembly: ExportRenderer(typeof(GalleyBaseView), typeof(GalleyBaseViewRenderer))]
namespace GalleyFramework.Droid.Renderers
{
    [Preserve(AllMembers = true)]
    public class GalleyBaseViewRenderer : VisualElementRenderer<GalleyBaseView>
    {
        public GalleyBaseViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<GalleyBaseView> e)
        {
            base.OnElementChanged(e);
            e.OldElement.NotNull().Then(() => e.OldElement.BackgroundRedrawingInvoked -= Invalidate);
            e.NewElement.NotNull().Then(() => e.NewElement.BackgroundRedrawingInvoked += Invalidate);
        }

        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            var colorFrom = Element.BackgroundGradientColorFrom;
            var colorTo = Element.BackgroundGradientColorTo;

            if (colorFrom.HasValue && colorTo.HasValue)
            {
                var startColor = colorFrom.GetValueOrDefault().ToAndroid();
                var endColor = colorTo.GetValueOrDefault().ToAndroid();

                var width = 0;
                var height = Height;
                if (Element.IsHorizontalGradientBackground)
                {
                    width = Width;
                    height = 0;
                }

                using (var gradient = new Android.Graphics.LinearGradient(
                    0, 0, width, height,
                    startColor,
                    endColor,
                    Android.Graphics.Shader.TileMode.Mirror))
                {

                    using (var paint = new Android.Graphics.Paint { Dither = true })
                    {
                        paint.SetShader(gradient);
                        canvas.DrawPaint(paint);
                    }
                }
            }
            base.DispatchDraw(canvas);
        }
    }
}
