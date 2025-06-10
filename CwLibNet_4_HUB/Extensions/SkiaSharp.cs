using SkiaSharp;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Extensions;

public static class SkiaSharp
{
    public static uint ToColor(this SKColor color)
    {
        return (uint)((color.Alpha << 24) | (color.Red << 16) | (color.Green << 8) | color.Blue);
    }
}