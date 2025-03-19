using SkiaSharp;

namespace CwLibNet.Extensions;

public static class SKBitmapEx
{
    public static SKBitmap ScaleFit(this SKBitmap originalBitmap, int targetWidth, int targetHeight)
    {
        float scaleWidth = (float)targetWidth / originalBitmap.Width;
        float scaleHeight = (float)targetHeight / originalBitmap.Height;
        float scale = Math.Max(scaleWidth, scaleHeight);
        int newWidth = (int)(originalBitmap.Width * scale);
        int newHeight = (int)(originalBitmap.Height * scale);
        SKImageInfo resizedInfo = new SKImageInfo(newWidth, newHeight);
        SKBitmap resizedBitmap = new SKBitmap(resizedInfo);
        originalBitmap.ScalePixels(resizedBitmap, SKSamplingOptions.Default);

        // Calcola le coordinate per il ritaglio centrale
        int cropX = (newWidth - targetWidth) / 2;
        int cropY = (newHeight - targetHeight) / 2;

        // Crea un'area di ritaglio
        SKRectI cropRect = new SKRectI(cropX, cropY, cropX + targetWidth, cropY + targetHeight);

        // Ritaglia l'immagine per ottenere le dimensioni esatte
        SKBitmap finalBitmap = new SKBitmap(targetWidth, targetHeight);
        using SKCanvas canvas = new SKCanvas(finalBitmap);
        canvas.DrawBitmap(resizedBitmap, cropRect, new SKRect(0, 0, targetWidth, targetHeight));
        return finalBitmap;
    }
}