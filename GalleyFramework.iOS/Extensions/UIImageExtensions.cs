using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace GalleyFramework.iOS.Extensions
{
    public static class UIImageExtensions
    {
        public static UIImage WithOpacity(this UIImage image, double opacity)
		{
			if (opacity >= 1.0)
			{
				return image;
			}

            try
            {
                UIGraphics.BeginImageContext(image.Size);
                image.Draw(new RectangleF(0, 0, (float)image.Size.Width, (float)image.Size.Height), CGBlendMode.Normal, (nfloat)opacity);
                var newImage = UIGraphics.GetImageFromCurrentImageContext();
                image.Dispose();
                return newImage;
            }
            catch
            {
                return image;
            }
			finally
			{
				UIGraphics.EndImageContext();
			}
		}
    }
}
