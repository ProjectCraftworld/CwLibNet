using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Inventory;

public class ColorCorrection: ISerializable
{
    public const int BASE_ALLOCATION_SIZE = 0x18;

    public float saturation = 1.0f;
    public float hueShift;
    public float brightness = 0.5f;
    public float contrast = 0.5f;
    public float tintHue, tintAmount;

    
    public void Serialize(Serializer serializer)
    {
        saturation = serializer.F32(saturation);
        hueShift = serializer.F32(hueShift);
        brightness = serializer.F32(brightness);
        contrast = serializer.F32(contrast);
        tintHue = serializer.F32(tintHue);
        tintAmount = serializer.F32(tintAmount);
    }

    
    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE;
    }


}