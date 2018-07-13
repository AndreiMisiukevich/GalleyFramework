// 1410
using System;
using System.Threading.Tasks;
using GalleyFramework.Helpers.Flow;

namespace GalleyFramework.Helpers.Interfaces.Services
{
    public interface IDialogService
    {
        Task<string> ShowEntryAlert(string title, string subtitle, string okText, string cancelText = null, string placeholder = null, bool isPassword = false, bool isDestructive = false, GalleyFont font = null, string initialText = null);
    }
}
