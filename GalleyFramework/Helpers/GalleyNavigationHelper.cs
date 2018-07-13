using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleyFramework.Views;
using GalleyFramework.ViewModels;
using Xamarin.Forms;
using GalleyFramework.Helpers.Interfaces;
using GalleyFramework.Helpers.Flow;
using GalleyFramework.Extensions;
using System.Reflection;
using GalleyFramework.Views.Interfaces;
using GalleyFramework.ViewModels.Flow;

namespace GalleyFramework.Services
{
    public class GalleyNavigationHelper : INavigationHelper
    {
        protected readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();

        protected readonly Dictionary<Type, Func<GalleyBaseViewModel>> _modelCreators =
            new Dictionary<Type, Func<GalleyBaseViewModel>>();

        protected readonly Dictionary<Type, Func<GalleyBaseViewModel, IGalleyBaseView<GalleyBaseViewModel>>> _viewCreators =
            new Dictionary<Type, Func<GalleyBaseViewModel, IGalleyBaseView<GalleyBaseViewModel>>>();

        protected GalleyNavigationLayer CurrentLayer => NavigationStack.CurrentLayer;
        protected GalleyViewItem CurrentItem => CurrentLayer?.CurrentItem;
        protected GalleyBaseView CurrentView => CurrentItem?.View;

        public Task NavigationTask { get; protected set; } = Task.FromResult(true);
        public GalleyNavigationStack NavigationStack { get; } = new GalleyNavigationStack();

        public GalleySuperPage SuperPage => GalleyApp.Current.MainPage;
        public GalleySuperView SuperView => SuperPage.SuperView;

        public GalleyBaseViewModel CurrentModel => CurrentItem?.Model;

        public async Task Init<TModel>(object args = null, bool animated = false, string animName = null) where TModel : GalleyBaseViewModel
        {
            await Init(args, animated, animName, typeof(TModel));
        }

        public async Task Init(object args = null, bool animated = false, string animName = null, params Type[] modelTypes)
        {
            var models = CreateViewModels(modelTypes);
            await Init(args, animated, animName, models);
        }

        public virtual async Task Init(object args = null, bool animated = false, string animName = null, params GalleyBaseViewModel[] models)
        {
            if (!NavigationTask.IsCompleted)
            {
                return;
            }
            InitModels(models, args);
            while (CurrentModel != null)
            {
                RemoveCurrentLayer();
            }
            AddCurrentLayer(models);
            await (NavigationTask = SuperView.InitAnimate(CurrentView, animated, animName).Execute(true));
        }

        public async Task Push<TModel>(object args = null, string animName = null) where TModel : GalleyBaseViewModel
        {
            await Push(args, animName, typeof(TModel));
        }

        public async Task Push(object args = null, string animName = null, params Type[] modelTypes)
        {
            var models = CreateViewModels(modelTypes);
            await Push(args, animName, models);
        }

        public virtual async Task Push(object args = null, string animName = null, params GalleyBaseViewModel[] models)
        {
            if (!NavigationTask.IsCompleted)
            {
                return;
            }
            InitModels(models, args);
            CurrentModel?.Hide();
            AddCurrentLayer(models);
            await (NavigationTask = SuperView.PushAnimate(CurrentView, animName).Execute(true));
        }

        public virtual async Task<bool> Pop(object args = null, int count = 1, string animName = null)
        {
            return await PopInternal(() => count-- > 0, args, animName);
        }

        public virtual async Task<bool> Pop<TModel>(object args = null, string animName = null) where TModel : GalleyBaseViewModel
        {
            return await Pop(typeof(TModel), args, animName);
        }

        public virtual async Task<bool> Pop(Type type, object args, string animName = null)
        {
            return await PopInternal(() => CurrentLayer.All(l => l.Model.GetType() != type), args, animName);
        }

        public async Task Replace<TModel>(object args = null, string animName = null) where TModel : GalleyBaseViewModel
        {
            await Replace(args, animName, typeof(TModel));
        }

        public async Task Replace(object args = null, string animName = null, params Type[] modelTypes)
        {
            var models = CreateViewModels(modelTypes);
            await Replace(args, animName, models);
        }

        public virtual async Task Replace(object args = null, string animName = null, params GalleyBaseViewModel[] models)
        {
            if (!NavigationTask.IsCompleted)
            {
                return;
            }
            InitModels(models, args);
            RemoveCurrentLayer();
            AddCurrentLayer(models);
            await (NavigationTask = SuperView.ReplaceAnimate(CurrentView, animName).Execute(true));
        }

        public async Task Switch<TModel>(object args = null, string animName = null) where TModel : GalleyBaseViewModel
        {
            await Switch(typeof(TModel), args, animName);
        }

        public virtual async Task Switch(Type modelType, object args = null, string animName = null)
        {
            if (!NavigationTask.IsCompleted)
            {
                return;
            }
            var item = CurrentLayer.FirstOrDefault(i => i.Model.GetType() == modelType);
            if (item != CurrentItem)
            {
                CurrentModel.Hide();
                CurrentLayer.Remove(item);
                CurrentLayer.Insert(0, item);
                CurrentModel.Show(args: new GalleyArgsWrapper(args));
                await (NavigationTask = SuperView.SwitchAnimate(CurrentView, animName).Execute(true));
            }
        }

        public void Bind<TModel, TView>() where TModel : GalleyBaseViewModel where TView : class, IGalleyBaseView<TModel>
        => _bindings[typeof(TModel)] = typeof(TView);

        public void RegisterModelCreator<TModel>(Func<TModel> creator) where TModel : GalleyBaseViewModel
        => _modelCreators[typeof(TModel)] = creator;

        public void RegisterViewCreator<TView>(Func<GalleyBaseViewModel, TView> creator) where TView : class, IGalleyBaseView<GalleyBaseViewModel>
        => _viewCreators[typeof(TView)] = creator;


        public TView GetView<TModel, TView>() where TModel : GalleyBaseViewModel
                                              where TView : class, IGalleyBaseView<GalleyBaseViewModel>
        => CreateViewItem(CreateViewModel<TModel>()).View.As<TView>();

        protected async Task<bool> PopInternal(Func<bool> popStepCheckFunction, object args, string animName)
        {
            if (!NavigationTask.IsCompleted) return false;
            var popResult = false;
            while (popStepCheckFunction.Invoke() && NavigationStack.Count > 1)
            {
                RemoveCurrentLayer();
                popResult = true;
            }
            if (!popResult) return false;
            CurrentModel.Show(args: new GalleyArgsWrapper(args));
            await (NavigationTask = SuperView.PopAnimate(CurrentView, animName).Execute(true));
            return true;
        }

        protected GalleyBaseViewModel[] CreateViewModels(params Type[] modelTypes)
        => modelTypes.Select(CreateViewModel).ToArray();

        protected GalleyBaseViewModel CreateViewModel<TModel>() where TModel : GalleyBaseViewModel
        => CreateViewModel(typeof(TModel));

        protected GalleyBaseViewModel CreateViewModel(Type modelType)
        => (_modelCreators.ContainsKey(modelType)
                ? _modelCreators[modelType]?.Invoke()
                : Activator.CreateInstance(modelType).As<GalleyBaseViewModel>());

        protected void RemoveCurrentLayer()
        => NavigationStack.Any().Then(() =>
           {
               CurrentModel.Hide();
               CurrentLayer.Each(i => i.Model.Destroy());
               NavigationStack.Pop();
           });

        protected virtual Type GetViewType(Type modelType)
        {
            var viewNamespace = modelType.Namespace.Replace(".ViewModels", ".Views");
            var viewName = modelType.Name.TailTrunc(5);
            var fullName = $"{viewNamespace}.{viewName}";
            var viewType = modelType.GetTypeInfo().Assembly.GetType(fullName);

            viewType.IsNull().Then(() => throw new TypeLoadException($"There is no such view's type: {fullName}"));
            return viewType;
        }

        protected void AddCurrentLayer(GalleyBaseViewModel[] models)
        {
            NavigationStack.Push(new GalleyNavigationLayer(models.Select(CreateViewItem)));
            CurrentModel.Show();
        }

        protected GalleyViewItem CreateViewItem(GalleyBaseViewModel model)
        => new GalleyViewItem(GetViewType(model), model, _viewCreators);

        protected void InitModels(GalleyBaseViewModel[] models, object args)
        => models.Each(m => m.Init(args));

        protected Type GetViewType(GalleyBaseViewModel model)
        {
            var modelType = model.GetType();
            if (_bindings.ContainsKey(modelType))
            {
                return _bindings[modelType];
            }
            return GetViewType(modelType);
        }
    }
}