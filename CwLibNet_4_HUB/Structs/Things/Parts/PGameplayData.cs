using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Things.Components;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Profile;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (subVersion >= 0xef && version > 0x2d0)
            serializer.Enum32(GameplayType);

        Serializer.Serialize(ref FluffCost);
        Serializer.Serialize(ref EggLink);
        Serializer.Serialize(ref KeyLink);

        if (version >= 0x2d1 && subVersion < 0xef)
            serializer.Enum32(GameplayType);

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