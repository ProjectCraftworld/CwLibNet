using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types;

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

    
    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (subVersion >= 0xef && version > 0x2d0)
            GameplayType = serializer.Enum32(GameplayType);

        FluffCost = serializer.S32(FluffCost);
        EggLink = serializer.Reference(EggLink);
        KeyLink = serializer.Reference(KeyLink);

        if (version >= 0x2d1 && subVersion < 0xef)
            GameplayType = serializer.Enum32(GameplayType);

        if (subVersion < 0xf3) return;
        TreasureType = serializer.I32(TreasureType);
        TreasureCount = serializer.I16(TreasureCount);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (EggLink != null) size += EggLink.GetAllocatedSize();
        if (KeyLink != null) size += KeyLink.GetAllocatedSize();
        return size;
    }


}