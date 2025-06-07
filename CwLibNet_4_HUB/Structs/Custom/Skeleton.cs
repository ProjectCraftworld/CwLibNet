using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Mesh;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Custom;

public class Skeleton: ISerializable
{
    public const int BaseAllocationSize = 0x14;

    public Bone[]? Bones;
    public short[] Mirror;
    public FlipType[] MirrorType;
    public CullBone[]? CullBones;

    
    
    private SkeletonType type;

    
    public void Serialize()
    {
        Bones = Serializer.Serialize(ref Bones);
        Serializer.Serialize(ref Mirror);
        Serializer.Serialize(ref MirrorType);
        CullBones = Serializer.Serialize(ref CullBones);
        if (Serializer.GetRevision().Before(Branch.Mizuki, (int)Revisions.MZ_BST_REMOVE_SK))
            Serializer.Serialize(ref type);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Bones != null) size += Bones.Sum(bone => bone.GetAllocatedSize());
        if (Mirror != null) size += Mirror.Length * 0x2;
        if (MirrorType != null) size += MirrorType.Length * 0x2;
        if (CullBones != null)
            size += CullBones.Length * CullBone.BaseAllocationSize;
        return size;
    }


}