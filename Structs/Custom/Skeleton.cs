using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;
using Cwlib.Structs.Mesh;

namespace Cwlib.Structs.Custom
{
    public class Skeleton : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x14;
        public Bone[] bones;
        public short[] mirror;
        public FlipType[] mirrorType;
        public CullBone[] cullBones;
        private SkeletonType type;
        public override void Serialize(Serializer serializer)
        {
            bones = serializer.Array(bones, typeof(Bone));
            mirror = serializer.Shortarray(mirror);
            mirrorType = serializer.Enumarray(mirrorType, typeof(FlipType));
            cullBones = serializer.Array(cullBones, typeof(CullBone));
            if (serializer.GetRevision().Before(Branch.MIZUKI, Revisions.MZ_BST_REMOVE_SK))
                type = serializer.Enum8(type);
        }

        public virtual int GetAllocatedSize()
        {
            int size = Skeleton.BASE_ALLOCATION_SIZE;
            if (this.bones != null)
                foreach (Bone bone in bones)
                    size += bone.GetAllocatedSize();
            if (this.mirror != null)
                size += (this.mirror.length * 0x2);
            if (this.mirrorType != null)
                size += (this.mirrorType.length * 0x2);
            if (this.cullBones != null)
                size += (this.cullBones.length * CullBone.BASE_ALLOCATION_SIZE);
            return size;
        }
    }
}