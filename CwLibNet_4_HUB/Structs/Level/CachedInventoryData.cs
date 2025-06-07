using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Level;

public class CachedInventoryData: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public long Category, Location;
    public int CachedToolType;
    // CachedUVs
    public byte U0, U1, V0, V1;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref Category);
        Serializer.Serialize(ref Location);
        if (version <= 0x39d) return;
        Serializer.Serialize(ref CachedToolType);

        Serializer.Serialize(ref U0);
        Serializer.Serialize(ref U1);
        Serializer.Serialize(ref V0);
        Serializer.Serialize(ref V1);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}