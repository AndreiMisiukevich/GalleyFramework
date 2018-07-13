using System.Threading.Tasks;
using Xamarin.Forms;

namespace GalleyFramework.Views.Popups.Flow
{
    public interface IGalleyModalView
    {
		Task AttachToParent(AbsoluteLayout parent);
		Task RemoveFromParent(bool animated);
    }
}
