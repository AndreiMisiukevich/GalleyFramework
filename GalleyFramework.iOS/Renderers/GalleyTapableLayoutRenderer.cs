using GalleyFramework.iOS.Renderers;
using GalleyFramework.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;

[assembly: ExportRenderer(typeof(GalleyTapableLayout), typeof(GalleyTapableLayoutRenderer))]
namespace GalleyFramework.iOS.Renderers
{
    [Preserve(AllMembers = true)]
    public class GalleyTapableLayoutRenderer : VisualElementRenderer<GalleyTapableLayout>
	{
		public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
            Element?.HandleTouch(true);
		}

		public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);
			Element?.HandleTouch(false);
		}

		public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled(touches, evt);
			Element?.HandleTouch(false);
		}
	}
}
