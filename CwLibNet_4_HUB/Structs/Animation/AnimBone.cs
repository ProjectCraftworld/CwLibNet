using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Animation;

public class AnimBone: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    /**
     * Unique identifier for the bone, generated from the name.
     */
    public int AnimHash;

    /**
     * Index of the parent of this bone.
     */
    public int Parent = -1;

    /**
     * Index of the first child of this bone.
     */
    public int FirstChild = -1;

    /**
     * Index of the next sibling of this bone.
     */
    public int NextSibling = -1;

    public AnimBone() { }

    public AnimBone(int animHash, int parent, int firstChild, int nextSibling)
    {
        AnimHash = animHash;
        Parent = parent;
        FirstChild = firstChild;
        NextSibling = nextSibling;
    }

    
    public virtual void Serialize(Serializer serializer)
    {
        AnimHash = serializer.I32(AnimHash);
        Parent = serializer.S32(Parent);
        FirstChild = serializer.S32(FirstChild);
        NextSibling = serializer.S32(NextSibling);
    }

    
    public virtual int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}