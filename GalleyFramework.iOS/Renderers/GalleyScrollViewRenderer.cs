using System;
using Xamarin.Forms.Platform.iOS;
using GalleyFramework.iOS.Renderers;
using Xamarin.Forms;
using GalleyFramework.Views.Controls;
using Foundation;

[assembly: ExportRenderer(typeof(GalleyScrollView), typeof(GalleyScrollViewRenderer))]
namespace GalleyFramework.iOS.Renderers
{
    [Preserve(AllMembers = true)]
    public class GalleyScrollViewRenderer : ScrollViewRenderer
    {
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

            if (e.NewElement != null)
            {
				ShowsHorizontalScrollIndicator = false;
				ShowsVerticalScrollIndicator = false;

				DraggingEnded -= OnDraggingEnded;
				DraggingStarted -= OnDraggingStarted;

                DraggingEnded += OnDraggingEnded;
                DraggingStarted += OnDraggingStarted;

                DecelerationStarted -= OnDecelerationStarted;
                DecelerationStarted += OnDecelerationStarted;

                DecelerationRate = DecelerationRateFast;

                Bounces = false;
            }
		}


		private void OnDecelerationStarted(object sender, EventArgs e)
		{
            (Element as GalleyScrollView)?.OnFlingStarted();
		}

        private void OnDraggingEnded(object sender, EventArgs e)
        {
            (Element as GalleyScrollView)?.OnTouchEnded();
        }

		private void OnDraggingStarted(object sender, EventArgs e)
		{
            (Element as GalleyScrollView)?.OnTouchStarted();
		}
    }
}
