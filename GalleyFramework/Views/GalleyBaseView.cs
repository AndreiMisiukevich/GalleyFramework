using Xamarin.Forms;
using GalleyFramework.ViewModels;
using GalleyFramework.Views.Interfaces;
using GalleyFramework.Extensions;
using System.Linq;
using GalleyFramework.Views.Popups.Flow;
using GalleyFramework.Extensions.Flow;
using System;

namespace GalleyFramework.Views
{
    public abstract class GalleyBaseView : AbsoluteLayout
    {
        public Action BackgroundRedrawingInvoked;

		public static readonly BindableProperty ShouldBeRemovedProperty = BindableProperty.Create(nameof(ShouldBeRemoved), typeof(bool), typeof(GalleyBaseView), default(bool));

        private View _content;

		public bool ShouldBeRemoved
		{
            get => (bool)GetValue(ShouldBeRemovedProperty);
			set => SetValue(ShouldBeRemovedProperty, value);
		}

        public View Content
        {
            set
            {
                _content.NotNull().Then(() => Children.Remove(_content));
                Children.Add(_content = value.WithAbs(AbsFlags.All, 0, 0, 1, 1));
            }
        }
		public Color? BackgroundGradientColorFrom { get; set; }
		public Color? BackgroundGradientColorTo { get; set; }
        public bool IsHorizontalGradientBackground { get; set; }

        public GalleyBaseViewModel ContextViewModel => BindingContext.As<GalleyBaseViewModel>();

        public void InvokeBackgroundRedrawing()
        => BackgroundRedrawingInvoked?.Invoke();
    }

    public abstract class GalleyBaseView<TModel> : GalleyBaseView, IGalleyBaseView<TModel> where TModel: GalleyBaseViewModel
    {
        protected GalleyBaseView(TModel model)
        {
            ResetViewModel(model);
            this.SetBinding(ShouldBeRemovedProperty, nameof(GalleyBaseViewModel.IsDestroyed));
        }

        public TModel ViewModel { get; private set; }

        public void ResetViewModel(GalleyBaseViewModel model)
        {
			ViewModel = model.As<TModel>();
			BindingContext = model;
            OnResetViewModel(model);
        }

        public bool BackButtonPressed()
        {
            var modalView = GalleyPopupsCounter.Popups.LastOrDefault();
            var isHandled = false;
            modalView.NotNull().Then(() =>
            {
                modalView.RemoveFromParent(true);
                isHandled = true;
            });
            return OnBackButtonPressed(isHandled);
        }

		protected virtual void OnResetViewModel(GalleyBaseViewModel model)
		{
		}

        protected virtual bool OnBackButtonPressed(bool isHandled)
        {
            if (!isHandled)
            {
                ViewModel?.BackCommand.Execute(null);
            }
			return true;
        }
    }
}
