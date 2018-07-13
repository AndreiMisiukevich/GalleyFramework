using System.Threading.Tasks;
using Xamarin.Forms;

namespace GalleyFramework.Views.Animations
{
    public interface IGalleySuperViewAnimation
    {
        Task Apply(GalleyBaseView view, GalleySuperView superView);
    }
}
