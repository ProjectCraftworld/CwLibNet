using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.DLC;

public class DLCGUID: ISerializable
{
    public const int BaseAllocationSize = 0x10;
        
    public GUID? GUID;
    public int Flags = DLCFileFlags.NONE;

    public void Serialize()
    {
        GUID = Serializer.Serialize(ref GUID);
        Serializer.Serialize(ref Flags);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}