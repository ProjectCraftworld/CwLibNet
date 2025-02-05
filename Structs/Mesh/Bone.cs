using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;
using CwLibNet.Resources;
using CwLibNet.Structs.Animation;

namespace CwLibNet.Structs.Mesh
{
    /// <summary>
    /// A joint of an armature for skinned meshes.
    /// </summary>
    public class Bone : AnimBone
    {
        public static readonly int MAX_BONE_NAME_LENGTH = 32;
        public static readonly int BASE_ALLOCATION_SIZE = AnimBone.BASE_ALLOCATION_SIZE + 0x120;
        private string name;
        public int flags = BoneFlag.NONE;
        public Matrix4x4 skinPoseMatrix = Matrix4x4.Identity;
        public Matrix4x4 invSkinPoseMatrix = Matrix4x4.Identity;
        public Vector4 obbMin, obbMax;
        public MeshShapeVertex[] shapeVerts;
        public MeshShapeInfo[] shapeInfos;
        public float shapeMinZ, shapeMaxZ;
        public Vector4 boundBoxMin, boundBoxMax, boundSphere;
        /// <summary>
        /// Creates an empty bone
        /// </summary>
        public Bone()
        {
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        public Bone(string name)
        {
            this.animHash = RAnimation.CalculateAnimationHash(name);
            if (name != null && name.Length() >= MAX_BONE_NAME_LENGTH)
                name = name.Substring(0, MAX_BONE_NAME_LENGTH);
            this.name = name;
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        public override void Serialize(Serializer serializer)
        {
            name = serializer.Serialize(name, MAX_BONE_NAME_LENGTH);
            flags = serializer.Serialize(flags);
            base.Serialize(serializer);
            skinPoseMatrix = serializer.Serialize(skinPoseMatrix);
            invSkinPoseMatrix = serializer.Serialize(invSkinPoseMatrix);
            obbMin = serializer.Serialize(obbMin);
            obbMax = serializer.Serialize(obbMax);
            shapeVerts = serializer.Serialize(shapeVerts, typeof(MeshShapeVertex));
            shapeInfos = serializer.Serialize(shapeInfos, typeof(MeshShapeInfo));
            shapeMinZ = serializer.Serialize(shapeMinZ);
            shapeMaxZ = serializer.Serialize(shapeMaxZ);
            boundBoxMin = serializer.Serialize(boundBoxMin);
            boundBoxMax = serializer.Serialize(boundBoxMax);
            boundSphere = serializer.Serialize(boundSphere);
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        public virtual string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        public virtual void SetName(string name)
        {
            if (name != null && name.Length() > MAX_BONE_NAME_LENGTH)
                throw new ArgumentException("Bone name cannot be longer than 31 " + "characters!");
            this.name = name;
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        public static Bone GetByHash(Bone[] skeleton, int animHash)
        {
            if (skeleton == null)
                throw new NullReferenceException("Can't get bones from null skeleton!");
            if (animHash == 0)
                return skeleton[0];
            foreach (Bone bone in skeleton)
                if (bone.animHash == animHash)
                    return bone;
            return null;
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        /// <summary>
        /// Attempts to get a bone's name from an animation hash.
        /// </summary>
        /// <param name="skeleton">Skeleton to search for animation hash</param>
        /// <param name="animHash">Hash to search for</param>
        /// <returns>Bone's name</returns>
        public static string GetNameFromHash(Bone[] skeleton, int animHash)
        {
            if (skeleton == null)
                throw new NullReferenceException("Can't get bones from null skeleton!");
            foreach (Bone bone in skeleton)
                if (bone.animHash == animHash)
                    return bone.name;
            return null;
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        /// <summary>
        /// Attempts to get a bone's name from an animation hash.
        /// </summary>
        /// <param name="skeleton">Skeleton to search for animation hash</param>
        /// <param name="animHash">Hash to search for</param>
        /// <returns>Bone's name</returns>
        public static int IndexOf(Bone[] skeleton, int animHash)
        {
            if (skeleton == null)
                throw new NullReferenceException("Can't get bones from null skeleton!");
            if (animHash == 0)
                return 0;
            for (int i = 0; i < skeleton.length; ++i)
            {
                Bone bone = skeleton[i];
                if (bone.animHash == animHash)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        /// <summary>
        /// Attempts to get a bone's name from an animation hash.
        /// </summary>
        /// <param name="skeleton">Skeleton to search for animation hash</param>
        /// <param name="animHash">Hash to search for</param>
        /// <returns>Bone's name</returns>
        /// <summary>
        /// Gets the index of this bone in a skeleton.
        /// </summary>
        /// <param name="skeleton">The skeleton that this bone is from</param>
        /// <returns>The index of this bone if it exists</returns>
        public virtual int GetIndex(Bone[] skeleton)
        {
            if (skeleton == null)
                throw new NullReferenceException("Can't get bones from null skeleton!");
            for (int i = 0; i < skeleton.length; ++i)
                if (skeleton[i].animHash == this.animHash)
                    return i;
            return -1;
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        /// <summary>
        /// Attempts to get a bone's name from an animation hash.
        /// </summary>
        /// <param name="skeleton">Skeleton to search for animation hash</param>
        /// <param name="animHash">Hash to search for</param>
        /// <returns>Bone's name</returns>
        /// <summary>
        /// Gets the index of this bone in a skeleton.
        /// </summary>
        /// <param name="skeleton">The skeleton that this bone is from</param>
        /// <returns>The index of this bone if it exists</returns>
        /// <summary>
        /// Gets the children of this bone.
        /// </summary>
        /// <param name="skeleton">The skeleton that this bone is from</param>
        /// <returns>The children of this bone</returns>
        public virtual Bone[] GetChildren(Bone[] skeleton)
        {
            if (skeleton == null)
                throw new NullReferenceException("Can't get bones from null skeleton!");
            List<Bone> bones = new List(skeleton.length);
            int index = this.GetIndex(skeleton);
            if (index == -1)
                throw new ArgumentException("This bone doesn't exist in the skeleton " + "provided!");
            foreach (Bone bone in skeleton)
            {
                if (bone == this)
                    continue;
                if (bone.parent == index)
                    bones.Add(bone);
            }

            return bones.ToArray(Bone[].New());
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        /// <summary>
        /// Attempts to get a bone's name from an animation hash.
        /// </summary>
        /// <param name="skeleton">Skeleton to search for animation hash</param>
        /// <param name="animHash">Hash to search for</param>
        /// <returns>Bone's name</returns>
        /// <summary>
        /// Gets the index of this bone in a skeleton.
        /// </summary>
        /// <param name="skeleton">The skeleton that this bone is from</param>
        /// <returns>The index of this bone if it exists</returns>
        /// <summary>
        /// Gets the children of this bone.
        /// </summary>
        /// <param name="skeleton">The skeleton that this bone is from</param>
        /// <returns>The children of this bone</returns>
        public virtual Matrix4 GetLocalTransform(Bone[] bones)
        {
            if (this.parent == -1)
                return this.skinPoseMatrix;
            Bone bone = bones[this.parent];
            return bone.invSkinPoseMatrix.Mul(this.skinPoseMatrix, new Matrix4());
        }

        /// <summary>
        /// Creates an empty bone
        /// </summary>
        /// <summary>
        /// Creates an empty named bone
        /// </summary>
        /// <param name="name">Name of the bone</param>
        // null terminated
        /// <summary>
        /// Attempts to get a bone's name from an animation hash.
        /// </summary>
        /// <param name="skeleton">Skeleton to search for animation hash</param>
        /// <param name="animHash">Hash to search for</param>
        /// <returns>Bone's name</returns>
        /// <summary>
        /// Gets the index of this bone in a skeleton.
        /// </summary>
        /// <param name="skeleton">The skeleton that this bone is from</param>
        /// <returns>The index of this bone if it exists</returns>
        /// <summary>
        /// Gets the children of this bone.
        /// </summary>
        /// <param name="skeleton">The skeleton that this bone is from</param>
        /// <returns>The children of this bone</returns>
        public override int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.shapeInfos != null)
                size += this.shapeInfos.length * MeshShapeInfo.BASE_ALLOCATION_SIZE;
            if (this.shapeVerts != null)
                size += this.shapeVerts.length * MeshShapeVertex.BASE_ALLOCATION_SIZE;
            return size;
        }
    }
}