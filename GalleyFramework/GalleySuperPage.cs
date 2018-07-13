using Xamarin.Forms;
using GalleyFramework.Extensions;
using GalleyFramework.ViewModels;
using GalleyFramework.Infrastructure;
using GalleyFramework.Views;
using GalleyFramework.Views.Interfaces;

namespace GalleyFramework
{
    public class GalleySuperPage : ContentPage
    {
        private bool _wasDisappearing;
        
        public GalleySuperPage(GalleySuperView superView, bool isAppHasSideMenu = false)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            this.WithBinding(IsBusyProperty, nameof(GalleyBaseViewModel.IsBusy));
            SuperView = superView;

            Content = isAppHasSideMenu
                ? GalleyApp.Current.ViewFactory.CreateSideScrollView(this)
                           : (View)SuperView;
        }

        public GalleySuperView SuperView { get; }

        protected override bool OnBackButtonPressed()
        => SuperView.CurrentView.As<IGalleyBaseView<GalleyBaseViewModel>>()?.BackButtonPressed() ?? false;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(_wasDisappearing)
            {
                _wasDisappearing = false;
                Galley.Navigation.CurrentModel?.Show();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _wasDisappearing = true;
            Galley.Navigation.CurrentModel?.Hide();
        }
    }
}
