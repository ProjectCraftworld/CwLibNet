using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Level;

public class CachedInventoryData: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public long Category, Location;
    public int CachedToolType;
    // CachedUVs
    public byte U0, U1, V0, V1;

    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        Category = serializer.U32(Category);
        Location = serializer.U32(Location);
        if (version <= 0x39d) return;
        CachedToolType = serializer.I32(CachedToolType);

        U0 = serializer.I8(U0);
        U1 = serializer.I8(U1);
        V0 = serializer.I8(V0);
        V1 = serializer.I8(V1);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}