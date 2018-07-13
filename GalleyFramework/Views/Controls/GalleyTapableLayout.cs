using System.Windows.Input;
using GalleyFramework.Extensions;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using GalleyFramework.Views.Behaviors;
using System;

namespace GalleyFramework.Views.Controls
{
    public static class TapableLayoutExtensions
    {
        public static GalleyTapableLayout WithTap(this GalleyTapableLayout view, string propName, object param = null)
        => view.WithBinding(GalleyTapableLayout.TapCommandProperty, propName)
               .With(v => v.TapCommandParameter = param);

        public static GalleyTapableLayout WithTap(this GalleyTapableLayout view, Action action)
        => view.With(v => v.TapDelegate = action);

        public static GalleyTapableLayout WithFadeViews(this GalleyTapableLayout view, params View[] fadeViews)
        {
            view.AddFadeViews(fadeViews);
            return view;
        }
    }

    public class GalleyTapableLayout : AbsoluteLayout
    {
        public event Action<bool> PressStateChanged;

        public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(
            nameof(TapCommand),
            typeof(ICommand),
            typeof(GalleyTapableLayout),
            default(ICommand));

        public static readonly BindableProperty TapCommandParameterProperty = BindableProperty.Create(
            nameof(TapCommandParameter),
            typeof(object),
            typeof(GalleyTapableLayout),
            default(object));

        public static readonly BindableProperty PressedOpacityProperty = BindableProperty.Create(
            nameof(PressedOpacity),
            typeof(double),
            typeof(GalleyTapableLayout),
            .5);

        public static readonly BindableProperty RegularOpacityProperty = BindableProperty.Create(
            nameof(RegularOpacity),
            typeof(double),
            typeof(GalleyTapableLayout),
            1.0,
            propertyChanged: HandleRegularOpacityPropertyChanged);

        private List<View> _fadeViews;
        private readonly bool? _makeInputTransparentChildren;

        public GalleyTapableLayout(bool? makeInputTransparentChildren = true)
        {
            _makeInputTransparentChildren = makeInputTransparentChildren;
            Extensions.ViewExtensions.WithTap(this, OnTap);
            ChildAdded += OnChildAdded;
        }

        private List<View> FadeViews => _fadeViews ?? (_fadeViews = new List<View>());

        private bool HasTapHandler => TapCommand.NotNull() || TapDelegate.NotNull();

        public Action TapDelegate { get; set; }

        public ICommand TapCommand
        {
            get { return GetValue(TapCommandProperty).As<ICommand>(); }
            set { SetValue(TapCommandProperty, value); }
        }

        public object TapCommandParameter
        {
            get { return GetValue(TapCommandParameterProperty); }
            set { SetValue(TapCommandParameterProperty, value); }
        }

        public double PressedOpacity
        {
            get => (double)GetValue(PressedOpacityProperty);
            set => SetValue(PressedOpacityProperty, value);
        }

        public double RegularOpacity
        {
            get => (double)GetValue(RegularOpacityProperty);
            set => SetValue(RegularOpacityProperty, value);
        }

        public void HandleTouch(bool isPressed)
        => (!isPressed || (IsEnabled && HasTapHandler))
            .Then(() =>
            {
                SetOpacity(isPressed ? PressedOpacity : RegularOpacity);
                PressStateChanged?.Invoke(isPressed);
            });

        public void OnTap() 
        {
            TapCommand?.Execute(TapCommandParameter);
            TapDelegate?.Invoke();
        }

        public void AddFadeViews(params View[] fadeViews) => FadeViews.AddRange(fadeViews);

        private void SetOpacity(double opacity)
        => _fadeViews.IsNull()
                         .Then(() => Opacity = opacity)
                         .Else(() => _fadeViews.Each(c => c.Opacity = opacity));

        private void OnChildAdded(object sender, ElementEventArgs e)
        {
            var view = e.Element.As<View>();
            view.NotNull().Then(() =>
            {
                view.Behaviors.Any(x => x.Is<GalleyFadeChildBehavior>())
                    .Then(() => FadeViews.Add(view));
                _makeInputTransparentChildren.HasValue
                     .Then(() => view.InputTransparent = _makeInputTransparentChildren.GetValueOrDefault());
            });
        }

        private static void HandleRegularOpacityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => bindable.As<GalleyTapableLayout>().SetOpacity((double)newValue);
    }
}
