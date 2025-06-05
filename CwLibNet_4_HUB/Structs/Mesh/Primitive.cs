using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Mesh;

public class Primitive: ISerializable
{
    public const int BaseAllocationSize = 0x60;

    /**
     * RGfxMaterial used to texture this primitive.
     */
    public ResourceDescriptor? Material;

    /**
     * Presumably for mapping alternative textures to
     * the material, however functionality appears to be scrapped.
     */
    public ResourceDescriptor? TextureAlternatives;

    /**
     * Minimum vertex painted by this primitive.
     */
    public int MinVert;

    /**
     * Maximum vertex painted by this primitive.
     */
    public int MaxVert;

    /**
     * What index is first painted by this primitive.
     */
    public int FirstIndex;

    /**
     * Number of indices painted with this material
     * after the first index.
     */
    public int NumIndices;

    /**
     * The region ID of this mesh, it essentially defines a "submesh",
     * that can be hidden by costume pieces.
     */
    public int Region;

    public Primitive() { }

    public Primitive(int minVert, int maxVert, int firstIndex, int numIndices)
    {
        MinVert = minVert;
        MaxVert = maxVert;
        FirstIndex = firstIndex;
        NumIndices = numIndices;
    }

    public Primitive(ResourceDescriptor? material, int minVert, int maxVert, int firstIndex,
                     int numIndices)
    : this(minVert, maxVert, firstIndex, numIndices) {
        Material = material;
    }

    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        Material = serializer.Resource(Material,
            ResourceType.GfxMaterial);

        switch (version)
        {
            case < 0x149:
                serializer.Resource(null, ResourceType.GfxMaterial);
                break;
            case >= (int)Revisions.MESH_TEXTURE_ALTERNATIVES:
                TextureAlternatives = serializer.Resource(TextureAlternatives,
                    ResourceType.TextureList);
                break;
        }

        MinVert = serializer.I32(MinVert);
        MaxVert = serializer.I32(MaxVert);
        FirstIndex = serializer.I32(FirstIndex);
        NumIndices = serializer.I32(NumIndices);
        Region = serializer.I32(Region);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }

    /**
     * In PS4 versions of models, the min/max vertices are all over the place,
     * as I'm not sure what's the reason for it, the MeshExporter has to fix up primitives.
     *
     * @param minVert Fixed up minimum vertex in primitive
     * @param maxVert Fixed up maximum vertex in primitive
     */
    public void SetMinMax(int minVert, int maxVert)
    {
        MinVert = minVert;
        MaxVert = maxVert;
    }

}