using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Inventory;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Profile;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components;

public class EggLink: ISerializable
{
    public const int BaseAllocationSize = 0x100;
    public GlobalThingDescriptor? Item;
    
    public InventoryItemDetails? Details;

    public ResourceDescriptor? Plan;
    public bool Shareable = true;
    public ResourceDescriptor? Painting;
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();


        // The game technically removes this field at 0x197,
        // but it's essentially the same as plan at 0x160
        if (version < 0x160) 
            Serializer.Serialize(ref Item);
        else
            Serializer.Serialize(ref Plan, ResourceType.Plan, false, false, false);

        switch (version)
        {
            case > 0x15f and < 0x197:
                Serializer.Serialize(ref Details);
                break;
            case > 0x207 and < 0x22a:
                Serializer.Serialize(ref temp_bool_false); // Unknown value
                break;
            case > 0x23b:
                Serializer.Serialize(ref Shareable);
                break;
        }

        var hasPainting = false;
        if (version > 0x3e0) {
            if (Serializer.IsWriting()) hasPainting = Painting != null;
            Serializer.Serialize(ref hasPainting);
        }
        if (hasPainting)
            Serializer.Serialize(ref Painting, ResourceType.Painting, true, true, true);

    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Details != null) size += Details.GetAllocatedSize();
        return size;
    }
}