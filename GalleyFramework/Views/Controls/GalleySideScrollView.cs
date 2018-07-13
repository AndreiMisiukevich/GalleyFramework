using System;
using GalleyFramework.Extensions;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GalleyFramework.Views.Controls
{
    public enum GalleyScrollDirection { Close, Open }
    public enum GalleyScrollState { Closed, Opened, Moving }

    public class GalleyScrollView : ScrollView
    {
        public virtual void OnTouchStarted()
        {
        }

        public virtual void OnTouchEnded()
        {
        }

        public virtual Task OnFlingStarted(bool needScroll = true, bool animated = true, bool inMainThread = false)
        => Task.FromResult(true);
    }

    public abstract class GalleySideScrollView : GalleyScrollView
    {
		protected readonly StackLayout _viewStack;
		protected GalleyScrollDirection _currentDirection;
		protected GalleyScrollState _currentState;
		protected bool _hasBeenAccelerated;
		protected double _prevScrollX;

		protected bool IsOpenDirection => _currentDirection == GalleyScrollDirection.Open;
		protected bool IsDirectionAndStateSame => (int)_currentDirection == (int)_currentState;
		protected virtual bool IsScrollEnabled => true;

        public GalleySideScrollView()
        {
			Orientation = ScrollOrientation.Horizontal;
			VerticalOptions = LayoutOptions.Fill;
			HorizontalOptions = LayoutOptions.Fill;

			Content = _viewStack = new StackLayout
			{
				VerticalOptions = LayoutOptions.Fill,
				HorizontalOptions = LayoutOptions.Fill,
				Spacing = 0,
				Orientation = StackOrientation.Horizontal
			};

            Scrolled += OnScrolled;
        }

		public override void OnTouchStarted()
		{
			_hasBeenAccelerated = false;
		}

		public override void OnTouchEnded()
		{
			IsScrollEnabled.Then(async () =>
			{
				if (Device.RuntimePlatform == Device.Android)
				{
					await Task.Delay(10);
				}

                if (_hasBeenAccelerated || !IsScrollEnabled)
				{
					return;
				}

				var width = GetAdditionalViewWidth();
				var isOpen = IsOpenDirection
							? ScrollX > GetAdditionalViewMovingWidth(width)
							: ScrollX > width - GetAdditionalViewMovingWidth(width);
				await ScrollToAsync(isOpen ? width : 0, 0, true);
			});
		}

		public override async Task OnFlingStarted(bool needScroll = true, bool animated = true, bool inMainThread = false)
		{
			_hasBeenAccelerated = true;
			if (needScroll && IsScrollEnabled)
			{
				await ScrollToAsync(IsOpenDirection ? GetAdditionalViewWidth() : 0, 0, animated).Execute(inMainThread);
			}
		}

		public async Task MoveSideMenu(bool isOpen = false, bool animated = true)
		{
			_currentDirection = isOpen
				? GalleyScrollDirection.Open
				: GalleyScrollDirection.Close;

			await OnFlingStarted(!IsDirectionAndStateSame, animated, true);
		}

		protected async void OnMoveActionInvoked(bool isOpen = false)
		{
			await MoveSideMenu(isOpen, true);
		}

        protected virtual void OnScrolled(object sender, ScrolledEventArgs args)
        {
            _currentDirection = Math.Abs(_prevScrollX - ScrollX) < double.Epsilon
                    ? _currentDirection
                    : _prevScrollX > ScrollX
                        ? GalleyScrollDirection.Close
                        : GalleyScrollDirection.Open;
            _prevScrollX = ScrollX;

            IsScrollEnabled.Then(CheckScrollState);
        }

        protected virtual void CheckScrollState()
        {
            if (Math.Abs(ScrollX) <= double.Epsilon) //TODO: refactor this
            {
                _currentState = GalleyScrollState.Closed;
            }
            else if (Math.Abs(ScrollX - GetAdditionalViewWidth()) <= double.Epsilon)
            {
                _currentState = GalleyScrollState.Opened;
            }
            else
            {
                _currentState = GalleyScrollState.Moving;
            }
        }

        protected abstract double GetAdditionalViewWidth();
        protected abstract double GetAdditionalViewMovingWidth(double detailWidth);
	}
}
