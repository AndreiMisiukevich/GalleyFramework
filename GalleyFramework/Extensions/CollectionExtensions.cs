using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleyFramework.Extensions.Flow;
using Xamarin.Forms;

namespace GalleyFramework.Extensions
{
    public static class CollectionExtensions
    {
		public static IEnumerable<TItem> Each<TItem>(this IEnumerable<TItem> items, Action<TItem> action, ExecuteStrategy strategy)
		{
			switch (strategy)
			{
				case ExecuteStrategy.SeparateTask:
                    action = (i) => Task.Run(() => action(i));
					break;
				case ExecuteStrategy.MainThread:
                    action = (i) => Device.BeginInvokeOnMainThread(() => action(i));
					break;
			}

            return items.Each(action);
		}

        public static IEnumerable<TItem> Each<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
            return items;
        }

        public async static Task<IEnumerable<TItem>> EachAsync<TItem>(this IEnumerable<TItem> items, Func<TItem, Task> action, bool inCourse = true)
		{
			if (items != null)
			{
                if (inCourse)
                {
                    foreach (var item in items)
                    {
                        await action(item);
                    }
                }
                else
                {
                    await Task.WhenAll(items.Select(item => action(item)));
                }
			}
			return items;
		}

        public static void AddRange<TItem>(this ICollection<TItem> collection, params TItem[] arr)
        => arr.NotNull().Then(() => arr.Each(collection.Add));

        public static bool RemoveOfType<TItem, TRemove>(this ICollection<TItem> collection) where TRemove : TItem
        => collection.RemoveCollection(collection.Where(i => i is TRemove).ToArray());

        public static void Remove<TItem>(this ICollection<TItem> collection, Func<TItem, bool> selector)
        => collection.RemoveCollection(collection.Where(i => selector.Invoke(i)).ToArray());

        public static bool RemoveCollection<TItem>(this ICollection<TItem> collection, ICollection<TItem> collectionToRemove)
        {
            var result = false;
            collectionToRemove.Each(i => result = collection.Remove(i) || result);
            return result;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> collection, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var item in collection)
            {
                if (seenKeys.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }
    }
}