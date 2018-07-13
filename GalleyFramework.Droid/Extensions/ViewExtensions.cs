using Android.Graphics;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using GalleyFramework.Extensions;

namespace GalleyFramework.Droid.Extensions
{
    public static class ViewExtensions
    {
		public static void AsRounded(this Android.Views.View view, Color backColor, Color borderColor, int borderRadius = 10, int borderWidth = 1)
		{
			var corners = new float[] { borderRadius, borderRadius, borderRadius, borderRadius, borderRadius, borderRadius, borderRadius, borderRadius };
            var shape = view.Background.As<GradientDrawable>() ?? new GradientDrawable();
			shape.SetShape(ShapeType.Rectangle);
			shape.SetCornerRadii(corners);
			shape.SetColor(backColor);
			shape.SetStroke(borderWidth, borderColor);
			view.SetBackground(shape);
			//view.Invalidate();
		}
    }
}
