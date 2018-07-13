using System;
using System.Threading.Tasks;
using GalleyFramework.Extensions;
using Xamarin.Forms;
using GalleyFramework.Views.Popups.Flow;

namespace GalleyFramework.Views.Popups
{
    public abstract class GalleyBasePopup<TClickParameter> : AbsoluteLayout, IGalleyModalView
    {
        internal event Action<TClickParameter> Clicked;
        protected readonly View _overlay;
		protected readonly AbsoluteLayout _contentLayout;
		protected readonly Frame _mainFrame;

        protected GalleyBasePopup()
        {
			this.WithAbsBounds(0, 0, 1, 1)
				.WithAbsFlags(AbsoluteLayoutFlags.All);

            Children.Add(_overlay = new ContentView
			{
				BackgroundColor = Color.Black
			}.WithAbsFlags(AbsoluteLayoutFlags.All).WithAbsBounds(0, 0, 1, 1));


			Children.Add(_mainFrame = new Frame
			{
				Margin = new Thickness(30, 0),
				Padding = 0,
				HasShadow = false,
				BackgroundColor = Color.White,
				Content = _contentLayout = new AbsoluteLayout
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand
				}
			}.WithAbsBounds(.5, .5, -1, -1)
			 .WithAbsFlags(AbsoluteLayoutFlags.PositionProportional));
        }

		public async Task AttachToParent(AbsoluteLayout parent)
		{
            GalleyPopupsCounter.Popups.Add(this);
            await AttachToParentAnimation(parent);
		}

		public async Task RemoveFromParent(bool animated)
		{
			Clicked = null;
            GalleyPopupsCounter.Popups.Remove(this);
            if (animated)
            {
                await RemoveFromParentAnimation();
            }
			Parent.As<AbsoluteLayout>()?.Children.Remove(this);
		}

        protected virtual async Task AttachToParentAnimation(AbsoluteLayout parent)
        {
			_overlay.Opacity = _mainFrame.Opacity = 0;
			_mainFrame.Scale = 0;
			_mainFrame.Rotation = 90;
			parent.Children.Add(this);
			await Task.WhenAll(
			   _overlay.FadeTo(.5, 400, Easing.Linear),
			   _mainFrame.FadeTo(1, 350, Easing.Linear),
			   _mainFrame.ScaleTo(1, 350, Easing.Linear),
			   _mainFrame.RotateTo(0, 350, Easing.Linear));
        }

		protected virtual async Task RemoveFromParentAnimation()
		{
			await Task.WhenAll(_overlay.FadeTo(0, 350, Easing.Linear),
				   _mainFrame.FadeTo(0, 250, Easing.Linear),
				   _mainFrame.ScaleTo(0, 250, Easing.Linear));
		}

        protected void InvokeClick(TClickParameter parameter)
        => Clicked?.Invoke(parameter);

    }
}
