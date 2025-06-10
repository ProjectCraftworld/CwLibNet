using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.DLC;

public class DLCGUID: ISerializable
{
    public const int BaseAllocationSize = 0x10;
        
    public GUID? GUID;
    public int Flags = DLCFileFlags.NONE;

    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref GUID);
        Serializer.Serialize(ref Flags);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}