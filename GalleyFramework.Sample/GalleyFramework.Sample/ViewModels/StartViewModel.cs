using System.Windows.Input;
using GalleyFramework.Infrastructure;
using GalleyFramework.ViewModels;
using GalleyFramework.Helpers.Flow;
using GalleyFramework.Sample.Locales;
using Xamarin.Forms;
using GalleyFramework.Extensions;

namespace GalleyFramework.Sample.ViewModels
{
    public class StartViewModel : GalleyBaseViewModel
    {
        public StartViewModel() : base(GalleyCreateType.ViewFirstAppearing)
        {

        }

        public ICommand GoToSecondViewCommand => GetCommand(async () =>
        {
            await Galley.Navigation.Push<AnotherViewModel>(animName: "push");
        });
    }
}
