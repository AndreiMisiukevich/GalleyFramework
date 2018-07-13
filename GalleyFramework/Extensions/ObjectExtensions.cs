
namespace GalleyFramework.Extensions
{
    public static class ObjectExtensions
    {
        public static bool NotNull<TItem>(this TItem obj) where TItem : class
        => obj != null;

        public static bool IsNull<TItem>(this TItem obj) where TItem : class
        => obj == null;

		public static TItem As<TItem>(this object obj) where TItem : class
        => obj as TItem;

        public static TItem As<TSource, TItem>(this TSource obj) where TSource: class where TItem : class
        => obj as TItem;

        public static bool Is<TItem>(this object obj) => obj is TItem;

        public static bool EqualsOrBothNull<TITem>(this TITem obj, TITem other) where TITem: class
        {
            return (obj ?? other).IsNull() || obj == other;
        }
    }
}
