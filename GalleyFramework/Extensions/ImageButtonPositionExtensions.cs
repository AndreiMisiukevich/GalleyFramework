using GalleyFramework.Views.Controls;

namespace GalleyFramework.Extensions
{
    internal static class ImageButtonPositionExtensions
    {
        internal static double ToX(this GalleyImageButtonPosition pos)
        => pos == GalleyImageButtonPosition.Left
                                           ? 0
                                               : pos == GalleyImageButtonPosition.Right
                                           ? 1
                                               : 0.5;

        internal static double ToY(this GalleyImageButtonPosition pos)
        => pos == GalleyImageButtonPosition.Top
                                           ? 0
                                               : pos == GalleyImageButtonPosition.Bottom
                                           ? 1
                                               : 0.5;
    }
}
