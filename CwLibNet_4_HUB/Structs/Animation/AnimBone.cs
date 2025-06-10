using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Animation;

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

    
    public virtual void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref AnimHash);
        Serializer.Serialize(ref Parent);
        Serializer.Serialize(ref FirstChild);
        Serializer.Serialize(ref NextSibling);
    }

    
    public virtual int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}