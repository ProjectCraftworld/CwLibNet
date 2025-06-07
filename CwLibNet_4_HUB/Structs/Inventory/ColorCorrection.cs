using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Inventory;

public class ColorCorrection: ISerializable
{
    public const int BaseAllocationSize = 0x18;

    public float Saturation = 1.0f;
    public float HueShift;
    public float Brightness = 0.5f;
    public float Contrast = 0.5f;
    public float TintHue, TintAmount;

    
    public void Serialize()
    {
        Serializer.Serialize(ref Saturation);
        Serializer.Serialize(ref HueShift);
        Serializer.Serialize(ref Brightness);
        Serializer.Serialize(ref Contrast);
        Serializer.Serialize(ref TintHue);
        Serializer.Serialize(ref TintAmount);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}