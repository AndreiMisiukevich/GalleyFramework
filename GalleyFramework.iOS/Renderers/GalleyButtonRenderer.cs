using GalleyFramework.Extensions;
using GalleyFramework.iOS.Renderers;
using GalleyFramework.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System;
using GalleyFramework.iOS.Extensions;
using Foundation;

[assembly: ExportRenderer(typeof(GalleyButton), typeof(GalleyButtonRenderer))]
namespace GalleyFramework.iOS.Renderers
{
    [Preserve(AllMembers = true)]
    public class GalleyButtonRenderer : ButtonRenderer
    {
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
            if (e.PropertyName == nameof(GalleyButton.RegularImage) || e.PropertyName == nameof(GalleyButton.PressedImage))
            {
                SetBackground();
            }
        }

        private void SetBackground()
        {
            var element = Element.As<GalleyButton>();
            Control.TitleLabel.HighlightedTextColor = Control.TitleLabel.TextColor.ColorWithAlpha((nfloat)element.PressedOpacity);
            if (element.RegularImage.NotNull())
            {
                Control.SetBackgroundImage(UIImage.FromBundle(element.RegularImage), UIControlState.Normal);
            }

            if (element.PressedImage.NotNull())
            {
                Control.SetBackgroundImage(UIImage.FromBundle(element.PressedImage).WithOpacity(element.PressedOpacity), UIControlState.Highlighted);
                Control.SetBackgroundImage(UIImage.FromBundle(element.PressedImage).WithOpacity(element.PressedOpacity), UIControlState.Selected);
            }
        }
    }
}