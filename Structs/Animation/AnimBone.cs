using System
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Animation
{
    /// <summary>
    /// Stores information about bone relations for animation.
    /// </summary>
    public class AnimBone : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;
        /// <summary>
        /// Unique identifier for the bone, generated from the name.
        /// </summary>
        public int animHash;
        /// <summary>
        /// Unique identifier for the bone, generated from the name.
        /// </summary>
        /// <summary>
        /// Index of the parent of this bone.
        /// </summary>
        public int parent = -1;
        /// <summary>
        /// Unique identifier for the bone, generated from the name.
        /// </summary>
        /// <summary>
        /// Index of the parent of this bone.
        /// </summary>
        /// <summary>
        /// Index of the first child of this bone.
        /// </summary>
        public int firstChild = -1;
        /// <summary>
        /// Unique identifier for the bone, generated from the name.
        /// </summary>
        /// <summary>
        /// Index of the parent of this bone.
        /// </summary>
        /// <summary>
        /// Index of the first child of this bone.
        /// </summary>
        /// <summary>
        /// Index of the next sibling of this bone.
        /// </summary>
        public int nextSibling = -1;
        /// <summary>
        /// Unique identifier for the bone, generated from the name.
        /// </summary>
        /// <summary>
        /// Index of the parent of this bone.
        /// </summary>
        /// <summary>
        /// Index of the first child of this bone.
        /// </summary>
        /// <summary>
        /// Index of the next sibling of this bone.
        /// </summary>
        public AnimBone()
        {
        }

        /// <summary>
        /// Unique identifier for the bone, generated from the name.
        /// </summary>
        /// <summary>
        /// Index of the parent of this bone.
        /// </summary>
        /// <summary>
        /// Index of the first child of this bone.
        /// </summary>
        /// <summary>
        /// Index of the next sibling of this bone.
        /// </summary>
        public AnimBone(int animHash, int parent, int firstChild, int nextSibling)
        {
            this.animHash = animHash;
            this.parent = parent;
            this.firstChild = firstChild;
            this.nextSibling = nextSibling;
        }

        /// <summary>
        /// Unique identifier for the bone, generated from the name.
        /// </summary>
        /// <summary>
        /// Index of the parent of this bone.
        /// </summary>
        /// <summary>
        /// Index of the first child of this bone.
        /// </summary>
        /// <summary>
        /// Index of the next sibling of this bone.
        public override void Serialize(Serializer serializer)
        {
            animHash = serializer.Serialize(animHash);
            parent = serializer.Serialize(parent);
            firstChild = serializer.Serialize(firstChild);
            nextSibling = serializer.Serialize(nextSibling);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(animHash), animHash);
            info.AddValue(nameof(parent), parent);
            info.AddValue(nameof(firstChild), firstChild);
            info.AddValue(nameof(nextSibling), nextSibling);
        }
        public override void Serialize(Serializer serializer)
        {
            animHash = serializer.Serialize(animHash);
            parent = serializer.Serialize(parent);
            firstChild = serializer.Serialize(firstChild);
            nextSibling = serializer.Serialize(nextSibling);
        }

        /// <summary>
        /// Unique identifier for the bone, generated from the name.
        /// </summary>
        /// <summary>
        /// Index of the parent of this bone.
        /// </summary>
        /// <summary>
        /// Index of the first child of this bone.
        /// </summary>
        /// <summary>
        /// Index of the next sibling of this bone.
        /// </summary>
        public virtual int GetAllocatedSize()
        {
            return AnimBone.BASE_ALLOCATION_SIZE;
        }
    }
}