using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalleyFramework.Views.Interfaces
{
    public interface IGalleyFloatingControl
    {
        Task AddToSuperView(GalleySuperView superView);
        Task RemoveFromSuperView(GalleySuperView superView);
    }

    public interface IGalleyFloatingControlsHolderView
    {
        HashSet<IGalleyFloatingControl> FloatingControls { get; }
        Task AddControls(GalleySuperView superView);
    }
}
