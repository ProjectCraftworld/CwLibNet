using SkiaSharp;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Extensions;

public static class SkiaSharp
{
    public static uint ToColor(this SKColor color)
    {
        return (uint)((color.Alpha << 24) | (color.Red << 16) | (color.Green << 8) | color.Blue);
    }
}