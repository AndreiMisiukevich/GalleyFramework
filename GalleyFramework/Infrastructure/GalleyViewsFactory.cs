using GalleyFramework.Views;
namespace GalleyFramework.Infrastructure
{
    public class GalleyViewsFactory
    {
        public virtual GalleyMasterSideScrollView CreateSideScrollView(GalleySuperPage page)
        => new GalleyMasterSideScrollView(page);

        public virtual GalleySuperPage CreateSuperPage(GalleySuperView view)
        => new GalleySuperPage(view);

        public virtual GalleySuperView CreateSuperView()
        => new GalleySuperView();
    }
}
