using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace Cwlib.Structs.Mesh
{
    /// <summary>
    /// Building blocks of a mesh that controls
    /// how it gets rendered.
    /// </summary>
    public class Primitive : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x60;
        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        public ResourceDescriptor material;
        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        public ResourceDescriptor textureAlternatives;
        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        public int minVert;
        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        public int maxVert;
        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        public int firstIndex;
        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        public int numIndices;
        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public int region;
        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public Primitive()
        {
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public Primitive(int minVert, int maxVert, int firstIndex, int numIndices)
        {
            this.minVert = minVert;
            this.maxVert = maxVert;
            this.firstIndex = firstIndex;
            this.numIndices = numIndices;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public Primitive(ResourceDescriptor material, int minVert, int maxVert, int firstIndex, int numIndices) : this(minVert, maxVert, firstIndex, numIndices)
        {
            this.material = material;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public override void Serialize(Serializer serializer)
        {
            int version = serializer.GetRevision().GetVersion();
            material = serializer.Serialize(GetMaterial(), ResourceType.GFX_MATERIAL);
            if (version < 0x149)
                serializer.Serialize(null, ResourceType.GFX_MATERIAL);
            if (version >= Revisions.MESH_TEXTURE_ALTERNATIVES)
                textureAlternatives = serializer.Serialize(textureAlternatives, ResourceType.TEXTURE_LIST);
            minVert = serializer.Serialize(minVert);
            maxVert = serializer.Serialize(maxVert);
            firstIndex = serializer.Serialize(firstIndex);
            numIndices = serializer.Serialize(numIndices);
            region = serializer.Serialize(region);
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public virtual ResourceDescriptor GetMaterial()
        {
            return this.material;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public virtual int GetMinVert()
        {
            return this.minVert;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public virtual int GetMaxVert()
        {
            return this.maxVert;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public virtual int GetFirstIndex()
        {
            return this.firstIndex;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public virtual int GetNumIndices()
        {
            return this.numIndices;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        public virtual int GetRegion()
        {
            return this.region;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        /// <summary>
        /// In PS4 versions of models, the min/max vertices are all over the place,
        /// as I'm not sure what's the reason for it, the MeshExporter has to fix up primitives.
        /// </summary>
        /// <param name="minVert">Fixed up minimum vertex in primitive</param>
        /// <param name="maxVert">Fixed up maximum vertex in primitive</param>
        public virtual void SetMinMax(int minVert, int maxVert)
        {
            this.minVert = minVert;
            this.maxVert = maxVert;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        /// <summary>
        /// In PS4 versions of models, the min/max vertices are all over the place,
        /// as I'm not sure what's the reason for it, the MeshExporter has to fix up primitives.
        /// </summary>
        /// <param name="minVert">Fixed up minimum vertex in primitive</param>
        /// <param name="maxVert">Fixed up maximum vertex in primitive</param>
        public virtual void SetMaterial(ResourceDescriptor material)
        {
            this.material = material;
        }

        /// <summary>
        /// RGfxMaterial used to texture this primitive.
        /// </summary>
        /// <summary>
        /// Presumably for mapping alternative textures to
        /// the material, however functionality appears to be scrapped.
        /// </summary>
        /// <summary>
        /// Minimum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// Maximum vertex painted by this primitive.
        /// </summary>
        /// <summary>
        /// What index is first painted by this primitive.
        /// </summary>
        /// <summary>
        /// Number of indices painted with this material
        /// after the first index.
        /// </summary>
        /// <summary>
        /// The region ID of this mesh, it essentially defines a "submesh",
        /// that can be hidden by costume pieces.
        /// </summary>
        /// <summary>
        /// In PS4 versions of models, the min/max vertices are all over the place,
        /// as I'm not sure what's the reason for it, the MeshExporter has to fix up primitives.
        /// </summary>
        /// <param name="minVert">Fixed up minimum vertex in primitive</param>
        /// <param name="maxVert">Fixed up maximum vertex in primitive</param>
        public virtual void SetRegion(int region)
        {
            this.region = region;
        }
    }
}