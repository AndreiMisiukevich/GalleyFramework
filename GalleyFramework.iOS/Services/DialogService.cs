using System.Threading.Tasks;
using GalleyFramework.Helpers.Interfaces.Services;
using UIKit;
using GalleyFramework.Extensions;
using System.Linq;
using GalleyFramework.iOS.Services;
using Xamarin.Forms;
using System;
using GalleyFramework.Helpers.Flow;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Internals;

[assembly: Dependency(typeof(DialogService))]
namespace GalleyFramework.iOS.Services
{
    [Preserve(AllMembers = true)]
    public class DialogService : IDialogService
    {
        public Task<string> ShowEntryAlert(string title, string subtitle, string okText, string cancelText = null, string placeholder = null, bool isPassword = false, bool isDestructive = false, GalleyFont font = null, string initialText = null)
        {
            var tcs = new TaskCompletionSource<string>();
            var alert = UIAlertController.Create(title, subtitle, UIAlertControllerStyle.Alert);
            cancelText.NotNull().Then(() => alert.AddAction(UIAlertAction.Create(cancelText, UIAlertActionStyle.Cancel, t => tcs.SetResult(null))));
            alert.AddAction(UIAlertAction.Create(okText, isDestructive ? UIAlertActionStyle.Destructive : UIAlertActionStyle.Default, t => tcs.SetResult(alert.TextFields.FirstOrDefault()?.Text)));
            alert.AddTextField(f =>
            {
                f.SecureTextEntry = isPassword;
                f.Text = initialText;
                font.NotNull().Then(() => {
                    if (font.Name != null && font.Size > 0)
                    {
                        f.Font = UIFont.FromName(font.Name, (nfloat)font.Size);
                    }
                    if (font.Color != null)
                    {
                        f.TextColor = Color.FromHex(font.Color).ToUIColor();
                    }
                });
            });

            UIApplication.SharedApplication?.KeyWindow?.RootViewController?.PresentViewController(alert, true, null);
            return tcs.Task;
        }
    }
}