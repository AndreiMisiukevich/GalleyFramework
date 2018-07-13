using System;
using GalleyFramework.Views.Popups;
using Xamarin.Forms;
using GalleyFramework.Extensions;
namespace GalleyFramework.Sample.Views.Popups
{
    public class OkPopup : GalleyBasePopup<bool>
    {
        public OkPopup()
        {
            _contentLayout.Children.Add(new Button
            {
                Text = "Ok"
            }.WithClickHandler(() => InvokeClick(true))
             .WithAbsFlags(AbsoluteLayoutFlags.PositionProportional)
             .WithAbsBounds(.5, .5, -1, -1));
        }
    }
}
