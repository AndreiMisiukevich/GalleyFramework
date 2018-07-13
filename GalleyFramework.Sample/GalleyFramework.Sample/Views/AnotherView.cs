using System;
using GalleyFramework.Views;
using GalleyFramework.Sample.ViewModels;
using Xamarin.Forms;
using GalleyFramework.Infrastructure;
using GalleyFramework.Views.Interfaces;
using GalleyFramework.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalleyFramework.Sample.Views.Controls;
using GalleyFramework.Views.Controls;

namespace GalleyFramework.Sample.Views
{
    public class AnotherView : GalleyBaseView<AnotherViewModel>, IGalleyFloatingControlsHolderView
    {
        public AnotherView(AnotherViewModel m): base(m)
        {
            BackgroundGradientColorFrom = Color.Red;
            BackgroundGradientColorTo = Color.Yellow;

            Content = new StackLayout
            {
                Children = {
                    new TestScroll(),
                    new GalleyExpandableLayout
                    {
                        MainView = new GalleyTapableLayout() { HeightRequest = 50, BackgroundColor = Color.Aqua  },
                        SubView = new StackLayout { BackgroundColor = Color.Azure,
                            IsClippedToBounds = true,
                            Children =
                            {
                                new Label() { Text = "ALALAALALAALALAALALAALALAALALAALALAALALAALALAALALAALALAALALAALALAALALAALALAALALA ALALAALALAALALAALALAALALA ALALAALALAALALAALALAALALAALALAALALA ALALAALALAALALAALALAALALAALALAALALA ALALAALALAALALAALALAALALA ALALAALALAALALAALALAALALA"},
                            }
                        },
                        SubViewVisibleHeight = -1,
                        OnTapDelegate = () => {
                            BackgroundGradientColorFrom = Color.Blue;
                            BackgroundGradientColorTo = Color.Gold;
                            InvokeBackgroundRedrawing();
                        }
                    }
                }
            };

            FloatingControls = new HashSet<IGalleyFloatingControl>(new[] { new FloatingButton() });
        }

        public HashSet<IGalleyFloatingControl> FloatingControls { get; }

        public async Task AddControls(GalleySuperView superView)
        {
            await FloatingControls.EachAsync(v => v.AddToSuperView(superView), false);
        }
    }

    public class TestScroll : GalleySideScrollView
    {
        public TestScroll()
        {
            _viewStack.Children.Add(new ContentView
            {
                BackgroundColor = Color.Black,
                WidthRequest = Galley.Navigation.SuperPage.Width,
                HeightRequest = 50
            }.WithWidthLike(Galley.Navigation.SuperPage));

			_viewStack.Children.Add(new ContentView
			{
				BackgroundColor = Color.Red,
				HeightRequest = 50
			}.WithWidthLike(Galley.Navigation.SuperPage));
        }

        protected override double GetAdditionalViewMovingWidth(double detailWidth)
        => detailWidth * 0.2;

        protected override double GetAdditionalViewWidth()
        => _viewStack.Children[1].Width;
    }
}
