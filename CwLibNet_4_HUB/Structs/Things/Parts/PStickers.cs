using CwLibNet.IO;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Things.Components.Decals;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PStickers: ISerializable
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x60;

    /**
     * Decals that are on the primary thing.
     */
    public Decal[]? decals;

    /**
     * Decals on specific costume pieces.
     */
    public Decal[]?[] costumeDecals = new Decal[14][];

    /**
     * Paint control data, depreciated.
     */
    
    public PaintControlPoint[]? paintControl;

    /**
     * Eyetoy related information for decal placed.
     */
    
    public EyetoyData[]? eyetoyData;

    public PStickers() { }

    public PStickers(ResourceDescriptor sticker)
    {
        decals = [new Decal(sticker)];
    }

    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        decals = Serializer.Serialize(ref decals);

        if (!Serializer.IsWriting())
            costumeDecals = new Decal[Serializer.GetInput().I32()][];
        else Serializer.GetOutput().I32(costumeDecals.Length);
        for (var i = 0; i < costumeDecals.Length; ++i)
            costumeDecals[i] = Serializer.Serialize(ref costumeDecals[i]);

        if (version is >= 0x158 and <= 0x3ba)
            paintControl = Serializer.Serialize(ref paintControl);

        if (version >= 0x15d)
            eyetoyData = Serializer.Serialize(ref eyetoyData);
    }

    
    public int GetAllocatedSize()
    {
        var size = BASE_ALLOCATION_SIZE;
        if (this.decals != null)
            size += this.decals.Length * Decal.BaseAllocationSize;
        size += costumeDecals.Sum(decals => decals.Length * Decal.BaseAllocationSize);
        if (paintControl != null)
            size += paintControl.Length * PaintControlPoint.BASE_ALLOCATION_SIZE;
        if (eyetoyData != null)
            size += eyetoyData.Length * EyetoyData.BaseAllocationSize;
        return size;
    }


}