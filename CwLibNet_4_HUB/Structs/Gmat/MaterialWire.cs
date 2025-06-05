using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Gmat;

public class MaterialWire: ISerializable
{
    public const int BaseAllocationSize = 0x10;
    public const int SwizzleElementCount = 5;

    public int BoxFrom, BoxTo;
    public byte PortFrom, PortTo;
    public byte[] Swizzle = new byte[SwizzleElementCount];

    /**
     * Empty constructor for serialization.
     */
    public MaterialWire() { }

    /**
     * Creates a connection between two boxes.
     */
    public MaterialWire(int boxFrom, int boxTo, int portFrom, int portTo)
    {
        BoxFrom = boxFrom;
        BoxTo = boxTo;
        PortFrom = (byte) portFrom;
        PortTo = (byte) portTo;
    }

    
    public void Serialize(Serializer serializer)
    {
        BoxFrom = serializer.S32(BoxFrom);
        BoxTo = serializer.S32(BoxTo);
        PortFrom = serializer.I8(PortFrom);
        PortTo = serializer.I8(PortTo);
        for (var i = 0; i < SwizzleElementCount; ++i)
            Swizzle[i] = serializer.I8(Swizzle[i]);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}