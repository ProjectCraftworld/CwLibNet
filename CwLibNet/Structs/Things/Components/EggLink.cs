using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Inventory;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Components;

public class EggLink: ISerializable
{
    public const int BaseAllocationSize = 0x100;
    public GlobalThingDescriptor? Item;
    
    public InventoryItemDetails? Details;

    public ResourceDescriptor? Plan;
    public bool Shareable = true;
    public ResourceDescriptor? Painting;
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();


        // The game technically removes this field at 0x197,
        // but it's essentially the same as plan at 0x160
        if (version < 0x160) 
            Item = serializer.Struct(Item);
        else
        Plan = serializer.Resource(Plan, ResourceType.Plan, false, false, false);

        if (version > 0x15f && version < 0x197)
            Details = serializer.Struct(Details);

        if (version > 0x207 && version < 0x22a)
            serializer.Bool(false); // Unknown value
        
        if (version > 0x23b)
            Shareable = serializer.Bool(Shareable);

        bool hasPainting = false;
        if (version > 0x3e0) {
            if (serializer.IsWriting()) hasPainting = Painting != null;
            hasPainting = serializer.Bool(hasPainting);
        }
        if (hasPainting)
            Painting = serializer.Resource(Painting, ResourceType.Painting, true);

    }

    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (Details != null) size += Details.GetAllocatedSize();
        return size;
    }
}