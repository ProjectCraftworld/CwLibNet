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
        if (serializer.GetRevision().Before(Branch.Mizuki, (int)Revisions.MZ_BST_REMOVE_SK))
            type = serializer.Enum8(type);
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