using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using GalleyFramework.Extensions;

namespace GalleyFramework.ViewModels.Flow
{
    public class GalleyObservableCollection<TEntity> : ObservableCollection<TEntity>
	{
        public GalleyObservableCollection() { }

        public GalleyObservableCollection(IEnumerable<TEntity> collection) : base(collection) { }

		public void AddRange(IEnumerable<TEntity> collection)
		{
            collection.NotNull().Then(() => {
                collection.Each(i => Items.Add(i));
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            });
		}

		public void RemoveRange(IEnumerable<TEntity> collection)
		{
			collection.NotNull().Then(() => {
				collection.Each(i => Items.Remove(i));
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			});
		}

		public void Replace(TEntity item)
		{
			ReplaceRange(new TEntity[] { item });
		}

		public void ReplaceRange(IEnumerable<TEntity> collection)
		{
			collection.NotNull().Then(() => {
                Items.Clear();
                collection.Each(Items.Add);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			});
		}
	}
}
