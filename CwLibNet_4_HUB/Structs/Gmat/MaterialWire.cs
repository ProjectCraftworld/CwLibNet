using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Gmat;
using static CwLibNet.IO.Serializer.Serializer;
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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref BoxFrom);
        Serializer.Serialize(ref BoxTo);
        Serializer.Serialize(ref PortFrom);
        Serializer.Serialize(ref PortTo);
        for (var i = 0; i < SwizzleElementCount; ++i)
            Serializer.Serialize(ref Swizzle[i]);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}