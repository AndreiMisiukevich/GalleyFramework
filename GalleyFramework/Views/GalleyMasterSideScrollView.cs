using GalleyFramework.Extensions;
using GalleyFramework.ViewModels;
using GalleyFramework.Views.Controls;
using Xamarin.Forms;

namespace GalleyFramework.Views
{
    public class GalleyMasterSideScrollView : GalleySideScrollView
    {
		private const int InverseRotation = 180;

		private readonly GalleySuperPage _page;
		private GalleyBaseMasterView _masterView;
        private GalleyScrollState _prevEndState;

		public GalleyMasterSideScrollView(GalleySuperPage page)
		{
			_page = page;
			_page.SizeChanged += (sender, e) => OnSuperPageSizeChanged();

			Rotation = InverseRotation;
            _viewStack.Rotation = -InverseRotation;
            _viewStack.Children.Add(_page.SuperView.WithSizeLike(_page));
		}

		public GalleyBaseMasterView MasterView
		{
			get => _masterView;
			set
			{
				if (value.EqualsOrBothNull(_masterView))
				{
					return;
				}

				_masterView?.ViewModel.Destroy();
				_viewStack.Children.RemoveOfType<View, GalleyBaseMasterView>();
				_masterView = value;
				if (value != null)
				{
					value.ViewModel.Init(_page.SuperView.BindingContext);
					OnSuperPageSizeChanged();
					value.ViewModel.MoveActionInvoked += OnMoveActionInvoked;
					_viewStack.Children.Insert(0, value);
				}
			}
		}

		protected void OnSuperPageSizeChanged()
		{
			var width = _page.Width;
			var height = _page.Height;
			if (IsScrollEnabled && width > 0 && height > 0)
			{
				MasterView.WidthRequest = width * MasterView.WidthPercent;
				MasterView.HeightRequest = height;
			}
		}

		protected override void CheckScrollState()
		{
            base.CheckScrollState();
            switch (_currentState)
            {
                case GalleyScrollState.Closed:
                    (_prevEndState != GalleyScrollState.Closed)
                    .Then(() =>
                    {
                        MasterView.ViewModel.Hide();
                        _page.SuperView.Children.RemoveOfType<View, GalleyMasterCloseOverlay>();
                    });
                    _prevEndState = _currentState;
                    break;
                case GalleyScrollState.Opened:
                    (_prevEndState != GalleyScrollState.Opened)
                   .Then(() =>
                   {
                       MasterView.ViewModel.Show();
                       _page.SuperView.Children.Add(new GalleyMasterCloseOverlay());
                   });
                    _prevEndState = _currentState;
                    break;
            }
		}

        protected override double GetAdditionalViewWidth() => _page.Width * MasterView.WidthPercent;
        protected override double GetAdditionalViewMovingWidth(double detailWidth) => detailWidth * MasterView.MotionWidthPercent;
        protected override bool IsScrollEnabled => MasterView.NotNull();
    }
}
