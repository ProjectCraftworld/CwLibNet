using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Animation;

public class AnimBone: ISerializable
{
    public const int BASE_ALLOCATION_SIZE = 0x20;

    /**
     * Unique identifier for the bone, generated from the name.
     */
    public int animHash;

    /**
     * Index of the parent of this bone.
     */
    public int parent = -1;

    /**
     * Index of the first child of this bone.
     */
    public int firstChild = -1;

    /**
     * Index of the next sibling of this bone.
     */
    public int nextSibling = -1;

    public AnimBone() { }

    public AnimBone(int animHash, int parent, int firstChild, int nextSibling)
    {
        this.animHash = animHash;
        this.parent = parent;
        this.firstChild = firstChild;
        this.nextSibling = nextSibling;
    }

    
    public virtual void Serialize(Serializer serializer)
    {
        animHash = serializer.I32(animHash);
        parent = serializer.S32(parent);
        firstChild = serializer.S32(firstChild);
        nextSibling = serializer.S32(nextSibling);
    }

    
    public virtual int GetAllocatedSize()
    {
        return AnimBone.BASE_ALLOCATION_SIZE;
    }


}