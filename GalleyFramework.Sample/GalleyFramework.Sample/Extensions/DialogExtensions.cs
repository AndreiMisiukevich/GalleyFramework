using System;
using System.Threading.Tasks;
using GalleyFramework.Helpers.Interfaces;
using GalleyFramework.Sample.Views.Popups;
namespace GalleyFramework.Sample.Extensions
{
    public static class DialogExtensions
    {
        public static async Task<bool> ShowOkPopup(this IDialogHelper dialog)
        {
            return await dialog.ShowPopup(() => new OkPopup());
        }
    }
}
