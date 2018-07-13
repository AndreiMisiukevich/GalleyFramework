using System;
using System.Collections.Generic;
using GalleyFramework.ViewModels;
using GalleyFramework.Views.Interfaces;
using GalleyFramework.Extensions;
using GalleyFramework.Views;

namespace GalleyFramework.Helpers.Flow
{
    public sealed class GalleyViewItem
    {
		private readonly Dictionary<Type, Func<GalleyBaseViewModel, IGalleyBaseView<GalleyBaseViewModel>>> _viewCreators;
		private GalleyBaseView _view;

        public GalleyViewItem(Type viewType,
                              GalleyBaseViewModel model,
                              Dictionary<Type, Func<GalleyBaseViewModel, IGalleyBaseView<GalleyBaseViewModel>>> viewCreators)
        {
            ViewType = viewType;
            Model = model;
            _viewCreators = viewCreators;
            if (model.CreateViewType == GalleyCreateType.LayerFirstAppearing)
            {
                _view = CreateView();
            }
        }

        public Type ViewType { get; }
        public GalleyBaseViewModel Model { get; }
        public bool HasCreatedView => _view.NotNull();

        public GalleyBaseView View => _view ?? GetView();

        private GalleyBaseView GetView()
        {
            var view = CreateView();
            return Model.CreateViewType == GalleyCreateType.ViewEveryAppearing
                       ? view
                       : _view = view;
        }

        private GalleyBaseView CreateView()
        => _viewCreators.ContainsKey(ViewType)
                            ? _viewCreators[ViewType]?.Invoke(Model).As<GalleyBaseView>()
                            : Activator.CreateInstance(ViewType, Model).As<GalleyBaseView>();
        
    }
}
