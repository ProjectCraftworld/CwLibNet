using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.DLC;

public class DLCGUID: ISerializable
{
    public const int BaseAllocationSize = 0x10;
        
    public GUID? GUID;
    public int Flags = DLCFileFlags.NONE;

    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref GUID);
        Serializer.Serialize(ref Flags);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}