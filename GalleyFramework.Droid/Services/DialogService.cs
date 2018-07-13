using System.Threading.Tasks;
using GalleyFramework.Helpers.Interfaces.Services;
using GalleyFramework.Extensions;
using GalleyFramework.iOS.Services;
using Xamarin.Forms;
using GalleyFramework.Helpers.Flow;
using Android.Widget;
using Android.App;
using Android.Graphics;
using Android.Util;
using Xamarin.Forms.Platform.Android;
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
            var alertBuilder = new AlertDialog.Builder(Forms.Context);
            var editText = new EditText(Forms.Context)
            {
                Hint = placeholder,
                Text = initialText
            };

            isPassword.Then(() => editText.InputType = Android.Text.InputTypes.TextVariationPassword);

            font.NotNull().Then(() =>
            {
                if (font.Name != null)
                {
                    editText.SetTypeface(Typeface.CreateFromAsset(Android.App.Application.Context.Assets, font.Name), TypefaceStyle.Normal);
                }
                if (font.Size > 0)
                {
                    editText.SetTextSize(ComplexUnitType.Sp, (float)font.Size);
                }
                if (font.Color != null)
                {
                    var color = Xamarin.Forms.Color.FromHex(font.Color).ToAndroid();
                    editText.SetTextColor(color);
                    editText.Background.Mutate().SetColorFilter(color, PorterDuff.Mode.SrcAtop);
                }
            });

            alertBuilder.SetTitle(title);
            alertBuilder.SetMessage(subtitle);


            cancelText.NotNull().Then(() => alertBuilder.SetNegativeButton(cancelText, (sender, e) => tcs.SetResult(null)));
            alertBuilder.SetPositiveButton(okText, (sender, e) => tcs.SetResult(editText.Text));

            var alert = alertBuilder.Create();
            alertBuilder.Dispose();
            alert.SetCanceledOnTouchOutside(true);
            alert.CancelEvent += (o, e) => tcs.SetResult(null);
            alert.SetView(editText, 15, 5, 15, 5);

            alert.Window.SetSoftInputMode(Android.Views.SoftInput.StateVisible);

            alert.Show();

            return tcs.Task;
        }
    }
}