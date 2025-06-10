using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Mesh;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Custom;

public class Skeleton: ISerializable
{
    public const int BaseAllocationSize = 0x14;

    public Bone[]? Bones;
    public short[] Mirror;
    public FlipType[] MirrorType;
    public CullBone[]? CullBones;

    
    
    private SkeletonType type;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Bones = serializer.Array(Bones);
        Mirror = serializer.Shortarray(Mirror);
        // MirrorType needs special handling for FlipType[]
        CullBones = serializer.Array(CullBones);
        if (serializer.GetRevision().Before(Branch.Mizuki, (int)Revisions.MZ_BST_REMOVE_SK))
            type = serializer.Enum32(type);
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