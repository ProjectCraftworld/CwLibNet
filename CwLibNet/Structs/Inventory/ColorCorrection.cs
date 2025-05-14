using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Inventory;

public class ColorCorrection: ISerializable
{
    public const int BaseAllocationSize = 0x18;

    public float Saturation = 1.0f;
    public float HueShift;
    public float Brightness = 0.5f;
    public float Contrast = 0.5f;
    public float TintHue, TintAmount;

    
    public void Serialize(Serializer serializer)
    {
        Saturation = serializer.F32(Saturation);
        HueShift = serializer.F32(HueShift);
        Brightness = serializer.F32(Brightness);
        Contrast = serializer.F32(Contrast);
        TintHue = serializer.F32(TintHue);
        TintAmount = serializer.F32(TintAmount);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}