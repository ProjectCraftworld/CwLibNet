using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref ThingOfWhichIAmABone);
        Serializer.Serialize(ref AnimHash);

        if (version < 0x341)
            Serializer.Serialize(ref LocalPosition);
        Serializer.Serialize(ref WorldPosition);

        LocalPosition ??= WorldPosition;

        // Unknown value, depreciated very early
        if (version < 0x155)
            Serializer.Serialize(ref 0);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}