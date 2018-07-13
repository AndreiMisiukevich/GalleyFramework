using System;
using GalleyFramework.Helpers.Flow;
using GalleyFramework.ViewModels.Flow;
using System.Threading.Tasks;

namespace GalleyFramework.ViewModels
{
    public abstract class GalleyBaseMasterViewModel : GalleyBaseViewModel
    {
        public event Action<bool> MoveActionInvoked;

        protected GalleyBaseViewModel MainViewModel { get; private set; }

		protected override void OnInitialized(GalleyArgsWrapper args)
		{
			MainViewModel = args.GetValue<GalleyBaseViewModel>();
		}

        protected override void OnDestroyed()
        {
            MoveActionInvoked = null;
        }

        protected GalleyBaseMasterViewModel() : this(GalleyCreateType.ViewFirstAppearing)
		{
		}

		protected GalleyBaseMasterViewModel(GalleyCreateType createType) : base(createType)
        {
		}

        protected void MoveSideMenu(bool isOpen)
        {
            MoveActionInvoked?.Invoke(isOpen);
        }
    }
}
