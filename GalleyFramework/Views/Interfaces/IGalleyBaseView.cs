using System.Collections.Generic;
using GalleyFramework.ViewModels;
using GalleyFramework.Views.Controls;
using Xamarin.Forms;

namespace GalleyFramework.Views.Interfaces
{
    public interface IGalleyBaseView<out TModel> where TModel : GalleyBaseViewModel
    {
        TModel ViewModel { get; }
        bool BackButtonPressed();
        void ResetViewModel(GalleyBaseViewModel model);
    }
}