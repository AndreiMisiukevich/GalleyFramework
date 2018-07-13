using GalleyFramework.Sample.ViewModels;
using GalleyFramework.Views;
using Xamarin.Forms;
using GalleyFramework.Extensions;
using GalleyFramework.Views.Controls;
using System.Linq;

namespace GalleyFramework.Sample.Views
{
    public class StartView : GalleyBaseView<StartViewModel>
    {
        public StartView(StartViewModel model) : base(model)
        {
            BackgroundColor = Color.LightBlue;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    new Label {
                        TextColor = Color.Black
                    }.WithLocText("LocaleName"),
                    new GalleyImageButton
                    {
                        ImageSource = "testImage"
                    }.WithBinding(GalleyImageButton.TapCommandProperty, nameof(StartViewModel.GoToSecondViewCommand)),
                    new ListView
                    {
                        ItemsSource = Enumerable.Range(0, 30).Select(x => x.ToString()).ToArray()
                    }
                }
            };
        }
    }
}
