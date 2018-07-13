using GalleyFramework.Extensions;
using Xamarin.Forms;
using GalleyFramework.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GalleyFramework.Views.Controls;
using GalleyFramework.Views.Interfaces;
using GalleyFramework.Views.Animations;
using GalleyFramework.Infrastructure;
using Xamarin.Forms.Internals;
using System;
using GalleyFramework.Helpers.Flow;

namespace GalleyFramework.Views
{
    public class GalleySuperView : AbsoluteLayout
    {
        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(GalleySuperView), default(bool), propertyChanged: OnIsBysyPropertyChangedDelegate);

		private readonly Dictionary<string, IGalleySuperViewAnimation> _animationsMapping;
        protected IGalleySuperViewAnimation _pushAnimation = new GalleyPushAnimation();
        protected IGalleySuperViewAnimation _switchAnimation = new GalleySwitchAnimation();
        protected IGalleySuperViewAnimation _popAnimation = new GalleyPopAnimation();

        public GalleySuperView(Dictionary<string, IGalleySuperViewAnimation> animationsMapping = null)
        {
            _animationsMapping = animationsMapping;
            this.WithAbsFill()
            .WithBinding(IsBusyProperty, nameof(GalleyBaseViewModel.IsBusy));
        }

        public GalleyMasterSideScrollView ScrollView => Galley.Navigation.SuperPage.Content as GalleyMasterSideScrollView;

        public GalleyBaseView CurrentView { get; protected set; }

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        public virtual bool IsAllowedMakingViewsInvisible => true;

        public virtual async Task InitAnimate(GalleyBaseView view, bool animated = false, string animationName = null)
        {
            if (animated)
            {
                await SwitchAnimate(view, animationName);
            }
            else
            {
                if (await ShouldUseCustomAnimation(view, animationName)) return;
                Children.OfType<GalleyBaseView>().ToArray().Each(c => Children.Remove(c));
                Children.Add(view);
                await SetCurrentView(view);
                await ResetFloatingControls(view);
                SetSuperView(view);
            }
        }

        public async Task PushAnimate(GalleyBaseView view, string animationName = null)
        {
            if (await ShouldUseCustomAnimation(view, animationName)) return;
            await _pushAnimation.Apply(view, this);
        }

        public async Task SwitchAnimate(GalleyBaseView view, string animationName = null)
        {
            if (await ShouldUseCustomAnimation(view, animationName)) return;
            await _switchAnimation.Apply(view, this);
        }

        public async Task PopAnimate(GalleyBaseView view, string animationName = null)
        {
            if (await ShouldUseCustomAnimation(view, animationName)) return;
            await _popAnimation.Apply(view, this);
        }

		public async Task ReplaceAnimate(GalleyBaseView view, string animationName = null)
		=> await PushAnimate(view, animationName);

        public async Task SetCurrentView(GalleyBaseView view, bool needDelayBeforeSetViewBindings = false)
        {
            CurrentView = view;
            if(ScrollView.IsNull())
            {
				await SetViewBindings(view, needDelayBeforeSetViewBindings);
                return;
            }
            await ScrollView.MoveSideMenu();
            await SetViewBindings(view, needDelayBeforeSetViewBindings);
			ScrollView.MasterView = (view as IGalleyMasterDetailView)?.MasterView;
        }

        public void SetSuperView(GalleyBaseView view)
        {
            BindingContext = view.BindingContext;

            Children.OfType<GalleyBaseView>().Where(c => c != view).ToArray()
                    .Each(c =>
                    {
                        if (Math.Abs(c.X) > double.Epsilon || Math.Abs(c.Y) > double.Epsilon)
                        {
                            c.Opacity = 0;
                            c.Layout(0, 0, Width, Height);
                        }
                        var createViewType = c.ContextViewModel?.CreateViewType ?? GalleyCreateType.ViewEveryAppearing;
                        if (c.ShouldBeRemoved ||
                            !IsAllowedMakingViewsInvisible ||
                            createViewType == GalleyCreateType.ViewEveryAppearing)
                        {
                            Children.Remove(c);
                        }
                        c.IsVisible = false;
                        c.Opacity = 1;
                    });
        }

        public async Task SetViewBindings(GalleyBaseView view, bool needDelay = false)
		{
			if (needDelay)
			{
				await Task.Delay(10);
			}
            view.WithAbsFill();
        }

        public async Task ResetFloatingControls(GalleyBaseView view, Task animTask = null)
        {
            var floatingControlsHolder = view.As<IGalleyFloatingControlsHolderView>();
            animTask = animTask ?? Task.FromResult(true);
            var controlsForRemoving = Children.Where(v => v.Is<IGalleyFloatingControl>()).ToArray();

            if (floatingControlsHolder.NotNull())
            {
                await floatingControlsHolder.AddControls(this);
            }

            await Task.WhenAll(
                animTask,
                controlsForRemoving.EachAsync(v => v.As<IGalleyFloatingControl>().RemoveFromSuperView(this), false)
            );
        }

        public void InsertChildIfNeeded(int pos, GalleyBaseView view, bool makeIsVisible = true)
        {
            makeIsVisible.Then(() => {
                view.Opacity = 1;
                view.IsVisible = true;
            });
            Children.Contains(view).Else(() => Children.Insert(pos, view));
        }

        public GalleyBaseView[] GetOtherVisibleViews(View view)
        => Children.OfType<GalleyBaseView>().Where(v => v.IsVisible && v != view).ToArray();

		protected virtual void OnIsBusyChanged(bool value)
		{
			Children.RemoveOfType<View, GalleyLoadingOverlay>();
			value.Then(() => Children.Add(new GalleyLoadingOverlay()));
		}

        protected async Task<bool> ShouldUseCustomAnimation(GalleyBaseView view, string name)
        {
            IGalleySuperViewAnimation animation = null;
            if (name.NotNull() && (_animationsMapping?.TryGetValue(name, out animation) ?? false))
            {
                await animation.Apply(view, this);
            }
            return animation.NotNull();
        }

        private static void OnIsBysyPropertyChangedDelegate(BindableObject view, object oldValue, object newValue)
        => view.As<GalleySuperView>().OnIsBusyChanged((bool)newValue);
    }
}
