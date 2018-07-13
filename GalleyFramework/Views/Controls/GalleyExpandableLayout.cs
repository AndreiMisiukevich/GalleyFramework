using System;
using Xamarin.Forms;
using GalleyFramework.Extensions;

namespace GalleyFramework.Views.Controls
{
    public class GalleyExpandableLayout : StackLayout
    {
        private const string ExpandAnimationName = "expandAnimation";
        private bool _shouldIgnoreAnimation;
        private double _lastVisibleHeight = -1;
        private double _startHeight;
        private double _endHeight;
        private View _mainView;
        private Layout _subView;
        private Func<Layout> _subViewCreator;

        public static readonly BindableProperty SubViewVisibleHeightProperty = BindableProperty.Create(nameof(SubViewVisibleHeight), typeof(double), typeof(GalleyExpandableLayout), 0.0);

        public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
            nameof(IsExpanded),
            typeof(bool),
            typeof(GalleyExpandableLayout),
            default(bool),
            BindingMode.TwoWay,
            propertyChanged: HandleIsExpandedPropertyChangedDelegate);

        public View MainView
        {
            get => _mainView;
            set
            {
                _mainView.NotNull().Then(() => Children.Remove(_mainView));
                var tapableLayout = value.As<GalleyTapableLayout>();
                tapableLayout.NotNull()
                             .Then(() => tapableLayout.TapDelegate = OnTap)
                             .Else(() => value.WithTap(OnTap));

                Children.Insert(0, _mainView = value);
            }
        }

        public Action OnTapDelegate { get; set; }

        public double SubViewVisibleHeight
        {
            get => (double)GetValue(SubViewVisibleHeightProperty);
            set => SetValue(SubViewVisibleHeightProperty, value);
        }

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public Layout SubView
        {
            get => _subView;
            set
            {
                _subView.NotNull().Then(() =>
                {
                    _subView.SizeChanged -= OnSubViewSizeChanged;
                    Children.Remove(_subView);
                });
                value.NotNull().Then(() =>
                {
                    value.IsClippedToBounds = true;
                    value.HeightRequest = 0;
                    value.IsVisible = false;
                    Children.Add(_subView = value);
                });
            }
        }

        public Func<Layout> SubViewCreator 
        {
            private get => _subViewCreator;
            set 
            {
                _subViewCreator = value;
                if(IsExpanded && SubView.IsNull())
                {
                    SubView = value?.Invoke();
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _lastVisibleHeight = -1;
        }

        private void OnTap() => IsExpanded = !IsExpanded;

        private void HandleIsExpandedChanged()
        {
            SubView.IsNull().Then(() => SubView = _subViewCreator?.Invoke());
            if (SubView.IsNull()) return;

            SubView.SizeChanged -= OnSubViewSizeChanged;

            var isExpanding = SubView.AnimationIsRunning(ExpandAnimationName);
            SubView.AbortAnimation(ExpandAnimationName);
            IsExpanded.Then(() => SubView.IsVisible = true);

            _startHeight = 0;
            _endHeight = SubViewVisibleHeight > 0
                ? SubViewVisibleHeight
                : _lastVisibleHeight;

            var shouldInvokeAnimation = true;

            IsExpanded
                .Then(() => (_endHeight <= 0).Then(() =>
                {
                    shouldInvokeAnimation = false;
                    SubView.HeightRequest = -1;
                    SubView.SizeChanged += OnSubViewSizeChanged;
                })).Else(() =>
                {
                    _lastVisibleHeight = _startHeight = SubViewVisibleHeight > 0
                        ? SubViewVisibleHeight
                            : !isExpanding
                         ? SubView.Height
                                  : _lastVisibleHeight;
                    _endHeight = 0;
                });

            _shouldIgnoreAnimation = Height < 0;

            shouldInvokeAnimation.Then(InvokeAnimation);
        }

        private void OnSubViewSizeChanged(object sender, EventArgs e)
        {
            if (SubView.Height <= 0) return;
            SubView.SizeChanged -= OnSubViewSizeChanged;
            SubView.HeightRequest = 0;
            _endHeight = SubView.Height;
            InvokeAnimation();
        }

        private void InvokeAnimation()
        {
            if(_shouldIgnoreAnimation)
            {
                SubView.HeightRequest = _endHeight;
                SubView.IsVisible = IsExpanded;
                return;
            }

            new Animation(v => SubView.HeightRequest = v, _startHeight, _endHeight)
                .Commit(SubView,
                        ExpandAnimationName,
                        finished: (v, r) => IsExpanded.Else(() => SubView.IsVisible = false));
            OnTapDelegate?.Invoke();
        }

        private static void HandleIsExpandedPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        => bindable.As<GalleyExpandableLayout>().HandleIsExpandedChanged();
    }
}
