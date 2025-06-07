using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Things.Components;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PGameplayData: ISerializable
{
    public const int BaseAllocationSize = 0x60;


    public GameplayPartType GameplayType = GameplayPartType.UNDEFINED;
    public int FluffCost = 100000;
    public EggLink EggLink;
    public SlotID KeyLink;
    
    public int TreasureType;
    
    public short TreasureCount = 1;

    
    public void Serialize()
    {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (subVersion >= 0xef && version > 0x2d0)
            Serializer.Serialize(ref GameplayType);

        Serializer.Serialize(ref FluffCost);
        Serializer.Serialize(ref EggLink);
        Serializer.Serialize(ref KeyLink);

        if (version >= 0x2d1 && subVersion < 0xef)
            Serializer.Serialize(ref GameplayType);

        if (subVersion < 0xf3) return;
        Serializer.Serialize(ref TreasureType);
        Serializer.Serialize(ref TreasureCount);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (EggLink != null) size += EggLink.GetAllocatedSize();
        if (KeyLink != null) size += KeyLink.GetAllocatedSize();
        return size;
    }


}