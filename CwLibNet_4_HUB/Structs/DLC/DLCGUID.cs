using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.DLC;

public class DLCGUID: ISerializable
{
    public const int BaseAllocationSize = 0x10;
        
    public GUID? GUID;
    public int Flags = DLCFileFlags.NONE;

    public void Serialize(Serializer serializer)
    {
        GUID = serializer.Guid(GUID);
        Flags = serializer.I32(Flags);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}