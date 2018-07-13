using System;
using System.Threading.Tasks;
using GalleyFramework.Views.Popups;
using GalleyFramework.Helpers.Interfaces;
using GalleyFramework.Infrastructure;
using Xamarin.Forms;
using GalleyFramework.Helpers.Interfaces.Services;
using GalleyFramework.Helpers.Flow;

namespace GalleyFramework.Services
{
    public class GalleyDialogHelper : IDialogHelper
    {
        public Task<string> ShowEntryAlert(string title, string subtitle, string okText, string cancelText = null, string placeholder = null, bool isPassword = false, bool isDestructive = false, GalleyFont font = null, string initialText = null)
        => DependencyService.Get<IDialogService>().ShowEntryAlert(title, subtitle, okText, cancelText, placeholder, isPassword, isDestructive, font, initialText);

        public async Task ShowAlert(string title, string message)
        {
            try
            {
                await Galley.Navigation.SuperPage.DisplayAlert(title, message, Galley.Loc.CurrentLocale.OkText);
            }
            catch
            {
                await Task.FromResult(false);
            }
        }

        public async Task<string> ShowAnswer(string title, string cancelName, string destructionName, params string[] otherNames)
        {
            try
            {
                return await Galley.Navigation.SuperPage.DisplayActionSheet(title, cancelName, destructionName, otherNames);
            }
            catch
            {
                return await Task.FromResult(string.Empty);
            }
        }

        public async Task<bool> ShowAnswer(string title, string message)
        {
            try
            {
                return await Galley.Navigation.SuperPage.DisplayAlert(title, message, Galley.Loc.CurrentLocale.YesText, Galley.Loc.CurrentLocale.NoText);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<TResult> ShowPopup<TResult>(Func<GalleyBasePopup<TResult>> popupCreator)
        {
            try
            {
                var content = Galley.Navigation.SuperView;
                var popup = popupCreator.Invoke();
                await popup.AttachToParent(content);
                var tcs = new TaskCompletionSource<TResult>();
                popup.Clicked += async (r) =>
                {
                    tcs.SetResult(r);
                    await popup.RemoveFromParent(true);
                };
                return await tcs.Task;
            }
            catch
            {
                return await Task.FromResult(default(TResult));
            }
        }
    }
}