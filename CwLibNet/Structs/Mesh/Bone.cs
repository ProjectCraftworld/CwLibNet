using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.IO.Serializer;
using CwLibNet.Resources;
using CwLibNet.Structs.Animation;

namespace CwLibNet.Structs.Mesh;

public class Bone: AnimBone
{
    public const int MAX_BONE_NAME_LENGTH = 32;
    public new const int BASE_ALLOCATION_SIZE = AnimBone.BASE_ALLOCATION_SIZE + 0x120;

    private string? name;
    public int flags = BoneFlag.NONE;

    
    public Matrix4x4? skinPoseMatrix = Matrix4x4.Identity;

    public Matrix4x4? invSkinPoseMatrix = Matrix4x4.Identity.Invert();

    public Vector4? obbMin;
    public Vector4? obbMax;
    public MeshShapeVertex[] shapeVerts;
    public MeshShapeInfo[] shapeInfos;
    public float shapeMinZ, shapeMaxZ;
    public Vector4? boundBoxMin;
    public Vector4? boundBoxMax;
    public Vector4? boundSphere;

    /**
     * Creates an empty bone
     */
    public Bone() { }

    /**
     * Creates an empty named bone
     *
     * @param name Name of the bone
     */
    public Bone(String name)
    {
        animHash = RAnimation.CalculateAnimationHash(name);
        if (name is { Length: >= MAX_BONE_NAME_LENGTH }) // null terminated
            name = name[..MAX_BONE_NAME_LENGTH];
        this.name = name;
    }

    
    public override void Serialize(Serializer serializer)
    {

        name = serializer.Str(name, MAX_BONE_NAME_LENGTH);
        flags = serializer.I32(flags);

        base.Serialize(serializer);

        skinPoseMatrix = serializer.M44(skinPoseMatrix);
        invSkinPoseMatrix = serializer.M44(invSkinPoseMatrix);

        obbMin = serializer.V4(obbMin);
        obbMax = serializer.V4(obbMax);

        shapeVerts = serializer.Array(shapeVerts);
        shapeInfos = serializer.Array(shapeInfos);

        shapeMinZ = serializer.F32(shapeMinZ);
        shapeMaxZ = serializer.F32(shapeMaxZ);

        boundBoxMin = serializer.V4(boundBoxMin);
        boundBoxMax = serializer.V4(boundBoxMax);
        boundSphere = serializer.V4(boundSphere);
    }

    public string? getName()
    {
        return name;
    }

    public void setName(String name)
    {
        if (name != null && name.Length > MAX_BONE_NAME_LENGTH)
            throw new ArgumentException("Bone name cannot be longer than 31 " +
                                               "characters!");
        this.name = name;
    }

    public static Bone getByHash(Bone[] skeleton, int animHash)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        if (animHash == 0) return skeleton[0];
        foreach (Bone bone in skeleton)
            if (bone.animHash == animHash)
                return bone;
        return null;
    }

    /**
     * Attempts to get a bone's name from an animation hash.
     *
     * @param skeleton Skeleton to search for animation hash
     * @param animHash Hash to search for
     * @return Bone's name
     */
    public static string? getNameFromHash(Bone[] skeleton, int animHash)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        foreach (Bone bone in skeleton)
            if (bone.animHash == animHash)
                return bone.name;
        return null;
    }

    public static int indexOf(Bone[] skeleton, int animHash)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        if (animHash == 0) return 0;
        for (int i = 0; i < skeleton.Length; ++i)
        {
            Bone bone = skeleton[i];
            if (bone.animHash == animHash)
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
    public int getIndex(Bone[] skeleton)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        for (int i = 0; i < skeleton.Length; ++i)
            if (skeleton[i].animHash == animHash)
                return i;
        return -1;
    }

    /**
     * Gets the children of this bone.
     *
     * @param skeleton The skeleton that this bone is from
     * @return The children of this bone
     */
    public Bone[] getChildren(Bone[] skeleton)
    {
        if (skeleton == null)
            throw new NullReferenceException("Can't get bones from null skeleton!");
        List<Bone> bones = new(skeleton.Length);
        int index = getIndex(skeleton);
        if (index == -1)
            throw new ArgumentException("This bone doesn't exist in the skeleton " +
                                               "provided!");
        foreach (Bone bone in skeleton)
        {
            if (bone == this) continue;
            if (bone.parent == index)
                bones.Add(bone);
        }
        return bones.ToArray();
    }

    public Matrix4x4? getLocalTransform(Bone[] bones)
    {
        if (parent == -1) return skinPoseMatrix;
        Bone bone = bones[parent];
        return bone.invSkinPoseMatrix * (skinPoseMatrix);
    }

    
    public int GetAllocatedSize()
    {
        int size = BASE_ALLOCATION_SIZE;
        if (shapeInfos != null)
            size += shapeInfos.Length * MeshShapeInfo.BaseAllocationSize;
        if (shapeVerts != null)
            size += shapeVerts.Length * MeshShapeVertex.BaseAllocationSize;
        return size;
    }


}