using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Things.Components.Decals;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class PStickers
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x60;

    /**
     * Decals that are on the primary thing.
     */
    public Decal[] decals;

    /**
     * Decals on specific costume pieces.
     */
    public Decal[][] costumeDecals = new Decal[14][];

    /**
     * Paint control data, depreciated.
     */
    
    public PaintControlPoint[] paintControl;

    /**
     * Eyetoy related information for decal placed.
     */
    
    public EyetoyData[] eyetoyData;

    public PStickers() { }

    public PStickers(ResourceDescriptor sticker)
    {
        this.decals = new Decal[] { new Decal(sticker) };
    }

    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        decals = serializer.Array(decals);

        if (!serializer.IsWriting())
            costumeDecals = new Decal[serializer.GetInput().I32()][];
        else serializer.GetOutput().I32(costumeDecals.Length);
        for (int i = 0; i < costumeDecals.Length; ++i)
            costumeDecals[i] = serializer.Array(costumeDecals[i]);

        if (version >= 0x158 && version <= 0x3ba)
            paintControl = serializer.Array(paintControl);

        if (version >= 0x15d)
            eyetoyData = serializer.Array(eyetoyData);
    }

    
    public int GetAllocatedSize()
    {
        int size = BASE_ALLOCATION_SIZE;
        if (this.decals != null)
            size += (this.decals.Length * Decal.BaseAllocationSize);
        foreach (Decal[] decals in this.costumeDecals)
        if (decals != null)
            size += (decals.Length * Decal.BaseAllocationSize);
        if (this.paintControl != null)
            size += (this.paintControl.Length * PaintControlPoint.BASE_ALLOCATION_SIZE);
        if (this.eyetoyData != null)
            size += (this.eyetoyData.Length * EyetoyData.BaseAllocationSize);
        return size;
    }


}