using System;
using System.Threading.Tasks;
using GalleyFramework.Helpers.Flow;
using GalleyFramework.Views.Popups;

namespace GalleyFramework.Helpers.Interfaces
{
    public interface IDialogHelper
    {
        Task<string> ShowEntryAlert(string title, string subtitle, string okText, string cancelText = null, string placeholder = null, bool isPassword = false, bool isDestructive = false, GalleyFont font = null, string initialText = null);
        Task ShowAlert(string title, string message);
        Task<string> ShowAnswer(string title, string cancelName, string destructionName, params string[] otherNames);
        Task<bool> ShowAnswer(string title, string message);
        Task<TResult> ShowPopup<TResult>(Func<GalleyBasePopup<TResult>> popupCreator);
    }
}
