using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.Resources;
using CwLibNet.Structs.Animation;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Mesh;

public class Bone: AnimBone
{
    public const int MaxBoneNameLength = 32;
    public new const int BaseAllocationSize = AnimBone.BaseAllocationSize + 0x120;

    private string? name;
    public int Flags = BoneFlag.None;

    
    public Matrix4x4? SkinPoseMatrix = Matrix4x4.Identity;

    public Matrix4x4? InvSkinPoseMatrix = Matrix4x4.Identity.Invert();

    public Vector4? ObbMin;
    public Vector4? ObbMax;
    public MeshShapeVertex[]? ShapeVerts;
    public MeshShapeInfo[]? ShapeInfos;
    public float ShapeMinZ, ShapeMaxZ;
    public Vector4? BoundBoxMin;
    public Vector4? BoundBoxMax;
    public Vector4? BoundSphere;

    /**
     * Creates an empty bone
     */
    public Bone() { }

    /**
     * Creates an empty named bone
     *
     * @param name Name of the bone
     */
    public Bone(string name)
    {
        AnimHash = RAnimation.CalculateAnimationHash(name);
        if (name is { Length: >= MaxBoneNameLength }) // null terminated
            name = name[..MaxBoneNameLength];
        this.name = name;
    }

    
    public override void Serialize()
    {

        Serializer.Serialize(ref name);
        Serializer.Serialize(ref Flags);

        base.Serialize(serializer);

        Serializer.Serialize(ref SkinPoseMatrix);
        Serializer.Serialize(ref InvSkinPoseMatrix);

        Serializer.Serialize(ref ObbMin);
        Serializer.Serialize(ref ObbMax);

        ShapeVerts = Serializer.Serialize(ref ShapeVerts);
        ShapeInfos = Serializer.Serialize(ref ShapeInfos);

        Serializer.Serialize(ref ShapeMinZ);
        Serializer.Serialize(ref ShapeMaxZ);

        Serializer.Serialize(ref BoundBoxMin);
        Serializer.Serialize(ref BoundBoxMax);
        Serializer.Serialize(ref BoundSphere);
    }

    public string? GetName()
    {
        return name;
    }

    public void SetName(string name)
    {
        if (name is { Length: > MaxBoneNameLength })
            throw new ArgumentException("Bone name cannot be longer than 31 " +
                                               "characters!");
        this.name = name;
    }

    public static Bone? GetByHash(Bone?[] skeleton, int animHash)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        return animHash == 0 ? skeleton[0] : skeleton.FirstOrDefault(bone => bone.AnimHash == animHash);
    }

    /**
     * Attempts to get a bone's name from an animation hash.
     *
     * @param skeleton Skeleton to search for animation hash
     * @param animHash Hash to search for
     * @return Bone's name
     */
    public static string? GetNameFromHash(Bone[] skeleton, int animHash)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        return (from bone in skeleton where bone.AnimHash == animHash select bone.name).FirstOrDefault();
    }

    public static int IndexOf(Bone[] skeleton, int animHash)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        if (animHash == 0) return 0;
        for (var i = 0; i < skeleton.Length; ++i)
        {
            var bone = skeleton[i];
            if (bone.AnimHash == animHash)
                return i;
        }
        return -1;
    }

    /**
     * Gets the index of this bone in a skeleton.
     *
     * @param skeleton The skeleton that this bone is from
     * @return The index of this bone if it exists
     */
    public int GetIndex(Bone[] skeleton)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        for (var i = 0; i < skeleton.Length; ++i)
            if (skeleton[i].AnimHash == AnimHash)
                return i;
        return -1;
    }

    /**
     * Gets the children of this bone.
     *
     * @param skeleton The skeleton that this bone is from
     * @return The children of this bone
     */
    public Bone[] GetChildren(Bone[] skeleton)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        List<Bone> bones = new(skeleton.Length);
        var index = GetIndex(skeleton);
        if (index == -1)
            throw new ArgumentException("This bone doesn't exist in the skeleton " +
                                               "provided!");
        bones.AddRange(skeleton.Where(bone => bone != this).Where(bone => bone.Parent == index));
        return bones.ToArray();
    }

    public Matrix4x4? GetLocalTransform(Bone[] bones)
    {
        if (Parent == -1) return SkinPoseMatrix;
        var bone = bones[Parent];
        return bone.InvSkinPoseMatrix * SkinPoseMatrix;
    }

    
    public override int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (ShapeInfos != null)
            size += ShapeInfos.Length * MeshShapeInfo.BaseAllocationSize;
        if (ShapeVerts != null)
            size += ShapeVerts.Length * MeshShapeVertex.BaseAllocationSize;
        return size;
    }


}