using Xamarin.Forms;
using GalleyFramework.Extensions.Flow;
using GalleyFramework.Extensions;
using GalleyFramework.Views.Behaviors;
using System;

namespace GalleyFramework.Views.Controls
{
    public enum GalleyImageButtonPosition
    {
        Center = 0,
        Left,
        Right,
        Top,
        Bottom
    }

    public interface IGalleyImage
    {
        ImageSource Source { get; set; }
    }

    public class GalleyImageButton : GalleyTapableLayout
    {
        public static Func<IGalleyImage> DefaultImageFactory { get; set; }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            nameof(ImageSource),
            typeof(ImageSource),
            typeof(GalleyImageButton),
            default(ImageSource),
            propertyChanged: HandleImageSourcePropertyChanged);
        
		public ImageSource ImageSource
		{
			get => GetValue(ImageSourceProperty).As<ImageSource>();
			set => SetValue(ImageSourceProperty, value);
		}

		public View ImageView { get; }

		public IGalleyImage Image => ImageView.As<IGalleyImage>();

        public GalleyImageButton(GalleyImageButtonPosition position = GalleyImageButtonPosition.Center) : this(null, position)
        {
        }

        public GalleyImageButton(AbsFlags flags, double x, double y, double width, double height) : this(null, flags, x, y, width, height)
        {
        }

        public GalleyImageButton(IGalleyImage button, GalleyImageButtonPosition position = GalleyImageButtonPosition.Center) : this(button)
        {
            ImageView.WithAbs(AbsFlags.Pos, position.ToX(), position.ToY(), -1, -1);
        }

        public GalleyImageButton(IGalleyImage button, AbsFlags flags, double x, double y, double width, double height) : this(button)
        {
            ImageView.WithAbs(flags, x, y, width, height);
        }

        protected GalleyImageButton(IGalleyImage button)
        {
            ImageView = button.As<View>() ?? DefaultImageFactory?.Invoke().As<View>() ?? new GalleyDefaultImage();
            ImageView.Behaviors.Add(new GalleyFadeChildBehavior());
            Children.Add(ImageView);
        }

        private static void HandleImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => bindable.As<GalleyImageButton>().Image.Source = newValue.As<ImageSource>();

        private class GalleyDefaultImage : Image, IGalleyImage { }
    }
}
