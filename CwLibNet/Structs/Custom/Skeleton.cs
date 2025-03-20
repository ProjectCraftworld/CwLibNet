using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Mesh;
using CwLibNet.Types;

namespace CwLibNet.Structs.Custom;

public class Skeleton: ISerializable
{
    public const int BaseAllocationSize = 0x14;

    public Bone[] Bones;
    public short[] Mirror;
    public FlipType[] MirrorType;
    public CullBone[] CullBones;

    
    
    private SkeletonType type;

    
    public void Serialize(Serializer serializer)
    {
        Bones = serializer.Array(Bones);
        Mirror = serializer.Shortarray(Mirror);
        MirrorType = serializer.Enumarray(MirrorType);
        CullBones = serializer.Array(CullBones);
        if (serializer.GetRevision().Before(Branch.Mizuki, (int)Revisions.MzBstRemoveSk))
            type = serializer.Enum8(type);
    }

    public int GetAllocatedSize()
    {
        int size = Skeleton.BaseAllocationSize;
        if (this.Bones != null) size += Bones.Sum(bone => bone.GetAllocatedSize());
        if (this.Mirror != null) size += (this.Mirror.Length * 0x2);
        if (this.MirrorType != null) size += (this.MirrorType.Length * 0x2);
        if (this.CullBones != null)
            size += (this.CullBones.Length * CullBone.BaseAllocationSize);
        return size;
    }


}