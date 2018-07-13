using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using GalleyFramework.Views;
using GalleyFramework.iOS.Renderers;
using CoreGraphics;
using CoreAnimation;
using System.Diagnostics;
using System;
using UIKit;
using GalleyFramework.Extensions;
using Foundation;

[assembly: ExportRenderer(typeof(GalleyBaseView), typeof(GalleyBaseViewRenderer))]
namespace GalleyFramework.iOS.Renderers
{
    [Preserve(AllMembers = true)]
    public class GalleyBaseViewRenderer : VisualElementRenderer<GalleyBaseView>
    {
        private CAGradientLayer _gradientLayer;

        protected override void OnElementChanged(ElementChangedEventArgs<GalleyBaseView> e)
        {
            base.OnElementChanged(e);
            e.OldElement.NotNull().Then(() => {
                _gradientLayer = null;
                e.OldElement.BackgroundRedrawingInvoked -= DrawBackground;
            });
            e.NewElement.NotNull().Then(() => e.NewElement.BackgroundRedrawingInvoked += DrawBackground);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            DrawBackground();
        }

        private void DrawBackground()
        {
            try
            {
                _gradientLayer?.RemoveFromSuperLayer();

                if (!Element.BackgroundGradientColorFrom.HasValue ||
                    !Element.BackgroundGradientColorTo.HasValue)
                {
                    return;
                }

                var startColor = Element.BackgroundGradientColorFrom.GetValueOrDefault().ToCGColor();
                var endColor = Element.BackgroundGradientColorTo.GetValueOrDefault().ToCGColor();

                var startX = .5;
                var startY = 0.0;
                var endX = .5;
                var endY = 1.0;

                if (Element.IsHorizontalGradientBackground)
                {
                    startX = 0;
                    startY = .5;
                    endX = 1;
                    endY = .5;
                }

                _gradientLayer = new CAGradientLayer
                {
                    StartPoint = new CGPoint(startX, startY),
                    EndPoint = new CGPoint(endX, endY),
                    Frame = UIScreen.MainScreen.Bounds, // used it instead of rect
                    Colors = new CGColor[]
                    {
                        startColor,
                        endColor
                    }
                };

                NativeView.Layer.InsertSublayer(_gradientLayer, 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
