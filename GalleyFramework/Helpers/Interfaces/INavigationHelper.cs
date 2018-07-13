using System;
using GalleyFramework.Views;
using GalleyFramework.ViewModels;
using System.Threading.Tasks;
using GalleyFramework.Views.Interfaces;
using GalleyFramework.Helpers.Flow;

namespace GalleyFramework.Helpers.Interfaces
{
    public interface INavigationHelper
    {
        Task NavigationTask { get; }
        GalleyNavigationStack NavigationStack { get; }
        GalleySuperPage SuperPage { get; }
        GalleySuperView SuperView { get; }

        GalleyBaseViewModel CurrentModel { get; }

        Task Init<TModel>(object args = null, bool animated = false, string animName = null) where TModel : GalleyBaseViewModel;
        Task Init(object args = null, bool animated = false, string animName = null, params Type[] modelTypes);
        Task Init(object args = null, bool animated = false, string animName = null, params GalleyBaseViewModel[] models);

        Task Push<TModel>(object args = null, string animName = null) where TModel : GalleyBaseViewModel;
        Task Push(object args = null, string animName = null, params Type[] modelTypes);
        Task Push(object args = null, string animName = null, params GalleyBaseViewModel[] models);

        Task<bool> Pop(object args = null, int count = 1, string animName = null);
        Task<bool> Pop<TModel>(object args = null, string animName = null) where TModel : GalleyBaseViewModel;
        Task<bool> Pop(Type type, object args = null, string animName = null);

        Task Replace<TModel>(object args = null, string animName = null) where TModel : GalleyBaseViewModel;
        Task Replace(object args = null, string animName = null, params Type[] modelTypes);
        Task Replace(object args = null, string animName = null, params GalleyBaseViewModel[] models);

        Task Switch<TModel>(object args = null, string animName = null) where TModel : GalleyBaseViewModel;
        Task Switch(Type modelType, object args = null, string animName = null);

        TView GetView<TModel, TView>() where TModel : GalleyBaseViewModel
                                       where TView : class, IGalleyBaseView<GalleyBaseViewModel>;
        
        void Bind<TModel, TView>() where TModel : GalleyBaseViewModel where TView : class, IGalleyBaseView<TModel>;
        void RegisterModelCreator<TModel>(Func<TModel> creator) where TModel : GalleyBaseViewModel;
        void RegisterViewCreator<TView>(Func<GalleyBaseViewModel, TView> creator) where TView : class, IGalleyBaseView<GalleyBaseViewModel>;
    }
}
