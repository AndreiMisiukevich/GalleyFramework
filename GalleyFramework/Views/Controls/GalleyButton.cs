using Xamarin.Forms;

namespace GalleyFramework.Views.Controls
{
    public class GalleyButton : Button
    {
		public static readonly BindableProperty RegularImageProperty = BindableProperty.Create(nameof(RegularImage), typeof(string), typeof(GalleyButton), default(string));

		public string RegularImage
		{
			get => (string)GetValue(RegularImageProperty);
			set => SetValue(RegularImageProperty, value);
		}

		public static readonly BindableProperty PressedImageProperty = BindableProperty.Create(nameof(PressedImage), typeof(string), typeof(GalleyButton), default(string));

		public string PressedImage
		{
			get => (string)GetValue(PressedImageProperty);
			set => SetValue(PressedImageProperty, value);
		}

        public static readonly BindableProperty PressedOpacityProperty = BindableProperty.Create(nameof(PressedOpacity), typeof(double), typeof(GalleyButton), 1.0);

		public double PressedOpacity
		{
			get => (double)GetValue(PressedOpacityProperty);
			set => SetValue(PressedOpacityProperty, value);
		}
    }
}
