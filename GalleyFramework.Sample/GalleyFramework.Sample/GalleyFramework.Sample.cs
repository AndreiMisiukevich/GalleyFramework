using GalleyFramework.Infrastructure;
using GalleyFramework.Sample.ViewModels;
using GalleyFramework.Helpers;
using GalleyFramework.Sample.Locales;
using GalleyFramework.Views;
using GalleyFramework.Sample.Views;
using GalleyFramework.Helpers.Interfaces;

namespace GalleyFramework.Sample
{
    public class SampleApp : GalleyApp
    {
        public SampleApp()
        {
            Galley.Loc.SetLocale<EnLocale>();
            Galley.Navigation.Init<StartViewModel>();
        }
    }
}
