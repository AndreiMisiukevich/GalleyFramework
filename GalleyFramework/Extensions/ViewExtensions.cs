using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using GalleyFramework.ViewModels;
using GalleyFramework.Extensions.Flow;

namespace GalleyFramework.Extensions
{
    public static class ViewExtensions
    {
        public static TView WithBinding<TView>(this TView view, BindableProperty bindProp, string propName, BindingMode mode = BindingMode.Default, IValueConverter converter = null, object converterParameter = null, string stringFormat = null) where TView : VisualElement
        {
            view.SetBinding(bindProp, new Binding {
                Converter = converter,
                ConverterParameter = converterParameter,
                Mode = mode,
                Path = propName,
                StringFormat = stringFormat
            });
            return view;
        }

        public static TView WithBinding<TView>(this TView view, BindableProperty bindProp, string propName, IValueConverter converter, object converterParameter = null, string stringFormat = null) where TView : VisualElement
        {
            return view.WithBinding(bindProp, propName, BindingMode.Default, converter, converterParameter, stringFormat);
        }

        public static TView WithGesture<TView>(this TView view, GestureRecognizer gesture) where TView : View
        {
            view.GestureRecognizers.Add(gesture);
            return view;
        }

        public static TView WithTap<TView>(this TView view, string propName, object param = null) where TView : View
        {
            var tapGesture = new TapGestureRecognizer
            {
                CommandParameter = param
            };
            tapGesture.SetBinding(TapGestureRecognizer.CommandProperty, propName);
            view.GestureRecognizers.Add(tapGesture);
            return view;
        }

        public static TView WithTap<TView>(this TView view, Action<TView> action) where TView : View
        {
            var gesture = new TapGestureRecognizer();
            gesture.Tapped += (sender, e) => action?.Invoke(view);
            view.WithGesture(gesture);
            return view;
        }

        public static TView WithTap<TView>(this TView view, Action action) where TView : View
        {
            return view.WithTap((v) => action?.Invoke());
        }

        public static TView WithAbsFlags<TView>(this TView view, AbsoluteLayoutFlags flags) where TView : VisualElement
        {
            AbsoluteLayout.SetLayoutFlags(view, flags);
            return view;
        }

        public static TView WithAbsFlags<TView>(this TView view, AbsFlags flags) where TView : VisualElement
		{
			AbsoluteLayout.SetLayoutFlags(view, (AbsoluteLayoutFlags)flags);
			return view;
		}

        public static TView WithAbsBounds<TView>(this TView view, double x, double y, double width, double height) where TView : VisualElement
        {
            AbsoluteLayout.SetLayoutBounds(view, new Rectangle(x, y, width, height));
            return view;
        }

		public static TView WithAbs<TView>(this TView view, AbsFlags flags, double x, double y, double width, double height) where TView : VisualElement
		{
            view.WithAbsFlags(flags);
            view.WithAbsBounds(x, y, width, height);
			return view;
		}

        public static TView WithAbsFill<TView>(this TView view) where TView : VisualElement
        => view.WithAbs(AbsFlags.All, 0, 0, 1, 1);

        public static TView WithAbsCenter<TView>(this TView view, double width = -1, double height = -1) where TView : VisualElement
        => view.WithAbs(AbsFlags.Pos, .5, .5, width, height);

        public static TView WithClickHandler<TView>(this TView view, Action action) where TView : Button
        {
            view.Clicked += (sender, e) => action?.Invoke();
            return view;
        }

        public static TView WithCommand<TView>(this TView view, string propyName, BindingMode mode = BindingMode.Default) where TView : Button
        {
            view.WithBinding(Button.CommandProperty, propyName, mode);
            return view;
        }

        public static void Layout<TView>(this TView view, double x, double y, double width, double height) where TView : VisualElement
        {
            view.Layout(new Rectangle(x, y, width, height));
        }

        public static Task<bool> LayoutTo<TView>(this TView view, double x, double y, double width, double height, uint lenght = 250, Easing easing = null) where TView : VisualElement
        {
            return view.LayoutTo(new Rectangle(x, y, width, height), lenght, easing);
        }

        public static void Replace(this StackLayout layout, View view, View anotherView)
        {
            var children = layout.Children;
            var index = children.IndexOf(view);
            (index >= 0).Then(() =>
            {
                children.Remove(view);
                children.Insert(0, anotherView);
            });
        }

        public static TView ShareSizeFor<TView>(this TView view, VisualElement anotherView, double widthMultiplier = 1, double heightMultiplier = 1, Action<VisualElement> onSizeChanged = null) where TView : VisualElement
        {
            anotherView.WithSizeLike(view, widthMultiplier, heightMultiplier, onSizeChanged);
            return view;
        }

        public static TView ShareWidthFor<TView>(this TView view, VisualElement anotherView, double widthMultiplier = 1, Action<VisualElement> onSizeChanged = null) where TView : VisualElement
        {
            anotherView.WithWidthLike(view, widthMultiplier, onSizeChanged);
            return view;
        }

        public static TView ShareHeightFor<TView>(this TView view, VisualElement anotherView, double heightMultiplier = 1, Action<VisualElement> onSizeChanged = null) where TView : VisualElement
        {
            anotherView.WithHeightLike(view, heightMultiplier, onSizeChanged);
            return view;
        }

        public static TView WithSizeLike<TView>(this TView view, VisualElement anotherView, double widthMultiplier = 1, double heightMultiplier = 1, Action<VisualElement> onSizeChanged = null) where TView : VisualElement
        {
            OnSizeChanged(view, anotherView, widthMultiplier, heightMultiplier, onSizeChanged);
            anotherView.SizeChanged += (sender, e) => OnSizeChanged(view, anotherView, widthMultiplier, heightMultiplier, onSizeChanged);
            return view;
        }

        public static TView WithWidthLike<TView>(this TView view, VisualElement anotherView, double widthMultiplier = 1, Action<VisualElement> onWidthChanged = null) where TView : VisualElement
        {
            OnWidthChanged(view, anotherView, widthMultiplier, onWidthChanged);
            anotherView.SizeChanged += (sender, e) => OnWidthChanged(view, anotherView, widthMultiplier, onWidthChanged);
            return view;
        }

        public static TView WithHeightLike<TView>(this TView view, VisualElement anotherView, double heightMultiplier = 1, Action<VisualElement> onHeightChanged = null) where TView : VisualElement
        {
            OnHeightChanged(view, anotherView, heightMultiplier, onHeightChanged);
            anotherView.SizeChanged += (sender, e) => OnHeightChanged(view, anotherView, heightMultiplier, onHeightChanged);
            return view;
        }

        public static Label WithLocText(this Label label, string textKey, BindingMode mode = BindingMode.Default)
        {
            return label.WithBinding(Label.TextProperty, $"{nameof(GalleyBaseViewModel.Locale)}.{textKey}", mode);
        }

        public static Button WithLocText(this Button button, string textKey, BindingMode mode = BindingMode.Default)
        {
            return button.WithBinding(Button.TextProperty, $"{nameof(GalleyBaseViewModel.Locale)}.{textKey}", mode);
        }

        public static Editor WithLocText(this Editor editor, string textKey, BindingMode mode = BindingMode.Default)
        {
            return editor.WithBinding(Editor.TextProperty, $"{nameof(GalleyBaseViewModel.Locale)}.{textKey}", mode);
        }

        public static Entry WithLocText(this Entry entry, string textKey, BindingMode mode = BindingMode.Default)
        {
            return entry.WithBinding(Entry.TextProperty, $"{nameof(GalleyBaseViewModel.Locale)}.{textKey}", mode);
        }

        public static Entry WithLocPlaceholder(this Entry entry, string textKey, BindingMode mode = BindingMode.Default)
        {
            return entry.WithBinding(Entry.PlaceholderProperty, $"{nameof(GalleyBaseViewModel.Locale)}.{textKey}", mode);
        }

		public static Label WithText(this Label label, string propName, BindingMode mode = BindingMode.Default)
		{
			return label.WithBinding(Label.TextProperty, propName, mode);
		}

		public static Button WithText(this Button button, string propName, BindingMode mode = BindingMode.Default)
		{
			return button.WithBinding(Button.TextProperty, propName, mode);
		}

		public static Editor WithText(this Editor editor, string propName, BindingMode mode = BindingMode.Default)
		{
			return editor.WithBinding(Editor.TextProperty, propName, mode);
		}

		public static Entry WithText(this Entry entry, string propName, BindingMode mode = BindingMode.Default)
		{
			return entry.WithBinding(Entry.TextProperty, propName, mode);
		}

        public static TView WithOpacity<TView>(this TView view, double opacity) where TView : VisualElement
        {
            view.Opacity = opacity;
            return view;
        }

        public static TView WithRotation<TView>(this TView view, double rotation) where TView : VisualElement
        {
            view.Rotation = rotation;
            return view;
        }

        public static TView With<TView>(this TView view, Action<TView> action) where TView : VisualElement
        {
            action(view);
            return view;
        }

        #region Private section
        private static void OnSizeChanged<TView>(TView view, VisualElement anotherView, double widthMultiplier = 1, double heightMultiplier = 1, Action<VisualElement> onSizeChanged = null) where TView : VisualElement
        {
            if (anotherView.Width > 0 && anotherView.Height > 0)
            {
                var width = anotherView.Width * widthMultiplier;
                var height = anotherView.Height * heightMultiplier;

                view.WidthRequest = width;
                view.HeightRequest = height;
                onSizeChanged?.Invoke(anotherView);
            }
        }

        private static void OnWidthChanged<TView>(TView view, VisualElement anotherView, double widthMultiplier = 1, Action<VisualElement> onSizeChanged = null) where TView : VisualElement
        {
            if (anotherView.Width > 0)
            {
                var width = anotherView.Width * widthMultiplier;
                view.WidthRequest = width;
                onSizeChanged?.Invoke(anotherView);
            }
        }

        private static void OnHeightChanged<TView>(TView view, VisualElement anotherView, double heightMultiplier = 1, Action<VisualElement> onSizeChanged = null) where TView : VisualElement
        {
            if (anotherView.Height > 0)
            {
                var height = anotherView.Height * heightMultiplier;
                view.HeightRequest = height;
                onSizeChanged?.Invoke(anotherView);
            }
        }
        #endregion
    }
}