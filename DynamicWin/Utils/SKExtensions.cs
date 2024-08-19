using SkiaSharp;

namespace DynamicWin.Utils;

public static class SKExtensions {
    public static SKBitmap ToSKBitmap(this System.Drawing.Bitmap bitmap)
		{
			var info = new SKImageInfo(bitmap.Width, bitmap.Height);
			var skiaBitmap = new SKBitmap(info);
			using (var pixmap = skiaBitmap.PeekPixels())
			{
				bitmap.ToSKPixmap(pixmap);
			}
			return skiaBitmap;
		}

		public static SKImage ToSKImage(this System.Drawing.Bitmap bitmap)
		{
			var info = new SKImageInfo(bitmap.Width, bitmap.Height);
			var image = SKImage.Create(info);
			using (var pixmap = image.PeekPixels())
			{
				bitmap.ToSKPixmap(pixmap);
			}
			return image;
		}

		public static void ToSKPixmap(this System.Drawing.Bitmap bitmap, SKPixmap pixmap)
		{
			if (pixmap.ColorType == SKImageInfo.PlatformColorType)
			{
				var info = pixmap.Info;
				using (var tempBitmap = new System.Drawing.Bitmap(info.Width, info.Height, info.RowBytes, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, pixmap.GetPixels()))
				using (var gr = System.Drawing.Graphics.FromImage(tempBitmap))
				{
					// Clear graphic to prevent display artifacts with transparent bitmaps					
					gr.Clear(System.Drawing.Color.Transparent);
					
					gr.DrawImageUnscaled(bitmap, 0, 0);
				}
			}
			else
			{
				// we have to copy the pixels into a format that we understand
				// and then into a desired format
				// TODO: we can still do a bit more for other cases where the color types are the same
				using (var tempImage = bitmap.ToSKImage())
				{
					tempImage.ReadPixels(pixmap, 0, 0);
				}
			}
		}
}