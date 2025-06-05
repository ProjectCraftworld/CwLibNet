using SkiaSharp;

namespace CwLibNet.Extensions;

public static class SkBitmapEx
{
    public static SKBitmap ScaleFit(this SKBitmap originalBitmap, int targetWidth, int targetHeight)
    {
        var scaleWidth = (float)targetWidth / originalBitmap.Width;
        var scaleHeight = (float)targetHeight / originalBitmap.Height;
        var scale = Math.Max(scaleWidth, scaleHeight);
        var newWidth = (int)(originalBitmap.Width * scale);
        var newHeight = (int)(originalBitmap.Height * scale);
        var resizedInfo = new SKImageInfo(newWidth, newHeight);
        var resizedBitmap = new SKBitmap(resizedInfo);
        originalBitmap.ScalePixels(resizedBitmap, SKSamplingOptions.Default);

        // Calcola le coordinate per il ritaglio centrale
        var cropX = (newWidth - targetWidth) / 2;
        var cropY = (newHeight - targetHeight) / 2;

        // Crea un'area di ritaglio
        var cropRect = new SKRectI(cropX, cropY, cropX + targetWidth, cropY + targetHeight);

        // Ritaglia l'immagine per ottenere le dimensioni esatte
        var finalBitmap = new SKBitmap(targetWidth, targetHeight);
        using var canvas = new SKCanvas(finalBitmap);
        canvas.DrawBitmap(resizedBitmap, cropRect, new SKRect(0, 0, targetWidth, targetHeight));
        return finalBitmap;
    }
}