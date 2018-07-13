using System;
using Xamarin.Forms;
using GalleyFramework.Views.Interfaces;
using GalleyFramework.Views;
using System.Threading.Tasks;
using GalleyFramework.Extensions;
using GalleyFramework.ViewModels;

namespace GalleyFramework.Sample.Views.Controls
{
    public class FloatingButton : Button, IGalleyFloatingControl
    {
        public FloatingButton()
        {
            Text = "Pop";
            BackgroundColor = Color.White;
            BorderRadius = 40;
            this.WithCommand(nameof(GalleyBaseViewModel.BackCommand));
        }

        public async Task AddToSuperView(GalleySuperView superView)
        {
            var opacity = Opacity;
            Opacity = 0;
            superView.Children.Add(this);
            this.Layout(200, superView.Height, 80, 80);
            Opacity = opacity;
            await this.LayoutTo(200, 400, 80, 80);
            this.WithAbsBounds(200, 400, 80, 80);
        }

        public async Task RemoveFromSuperView(GalleySuperView superView)
        {
            await this.LayoutTo(X, superView.Height, Width, Height);
            superView.Children.Remove(this);
        }
    }
}
