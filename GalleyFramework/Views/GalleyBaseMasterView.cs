using GalleyFramework.ViewModels;

namespace GalleyFramework.Views
{
    public abstract class GalleyBaseMasterView : GalleyBaseView<GalleyBaseMasterViewModel>
    {
        public GalleyBaseMasterView(GalleyBaseMasterViewModel model) : base(model)
        {
        }

        public virtual double WidthPercent => 0.7;
        public virtual double MotionWidthPercent => 0.35;

        protected override bool OnBackButtonPressed(bool isHandled)
        {
            return false; //TODO: implement hiding
        }

        protected override void OnResetViewModel(GalleyBaseViewModel model)
        {

        }
    }
}
