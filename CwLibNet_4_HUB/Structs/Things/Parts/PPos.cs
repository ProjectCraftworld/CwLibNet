using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Parts;

/**
 * This part represents a Thing's
 * position in the world, as well
 * as information about a bone.
 */
public class PPos: ISerializable
{
    public const int BaseAllocationSize = 0x100;

    public Thing? ThingOfWhichIAmABone;
    public int AnimHash;

    public Matrix4x4? LocalPosition = Matrix4x4.Identity;

    public Matrix4x4? WorldPosition = Matrix4x4.Identity;

    public PPos() { }

    public PPos(Matrix4x4 wpos) : this(wpos, wpos) {}

    public PPos(Thing? root, int animHash)
    {
        ThingOfWhichIAmABone = root;
        AnimHash = animHash;
    }

    public PPos(Matrix4x4 wpos, Matrix4x4 pos)
    {
        WorldPosition = wpos;
        LocalPosition = pos;
    }

    public PPos(Thing? root, int animHash, Matrix4x4 wpos)
    {
        ThingOfWhichIAmABone = root;
        AnimHash = animHash;
        WorldPosition = wpos;
        LocalPosition = wpos;
    }

    public PPos(Thing? root, int animHash, Matrix4x4 wpos, Matrix4x4 pos)
    {
        ThingOfWhichIAmABone = root;
        AnimHash = animHash;
        WorldPosition = wpos;
        LocalPosition = pos;
    }

    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();

        ThingOfWhichIAmABone = serializer.Reference(ThingOfWhichIAmABone);
        AnimHash = serializer.I32(AnimHash);

        if (version < 0x341)
            LocalPosition = serializer.M44(LocalPosition);
        WorldPosition = serializer.M44(WorldPosition);

        LocalPosition ??= WorldPosition;

        // Unknown value, depreciated very early
        if (version < 0x155)
            serializer.I32(0);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}