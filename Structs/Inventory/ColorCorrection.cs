using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Inventory
{
    public class ColorCorrection : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x18;
        public float saturation = 1F;
        public float hueShift;
        public float brightness = 0.5F;
        public float contrast = 0.5F;
        public float tintHue, tintAmount;
        public override void Serialize(Serializer serializer)
        {
            saturation = serializer.Serialize(saturation);
            hueShift = serializer.Serialize(hueShift);
            brightness = serializer.Serialize(brightness);
            contrast = serializer.Serialize(contrast);
            tintHue = serializer.Serialize(tintHue);
            tintAmount = serializer.Serialize(tintAmount);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}