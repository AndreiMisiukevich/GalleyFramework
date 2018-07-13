using Android.Content;
using Android.Runtime;
using Android.Views;
using GalleyFramework.Droid.Renderers;
using GalleyFramework.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GalleyTapableLayout), typeof(GalleyTapableLayoutRenderer))]
namespace GalleyFramework.Droid.Renderers
{
    [Preserve(AllMembers = true)]
    public class GalleyTapableLayoutRenderer : VisualElementRenderer<GalleyTapableLayout>
    {
        public GalleyTapableLayoutRenderer(Context context) : base(context)
        {
        }

		public override bool OnTouchEvent(MotionEvent e)
		{
            switch (e.ActionMasked)
			{
				case MotionEventActions.Down:
                    Element?.HandleTouch(true);
					break;

				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
                    Element?.HandleTouch(false);
					break;
			}
			return base.OnTouchEvent(e);
		}
    }
}
