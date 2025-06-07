using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.Types.Profile;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Resources;

public class RPins : Resource
{
    public const int BaseAllocationSize = 0x10;

    public List<Pin>? Pins = [];

    public override void Serialize()
    {
        Serializer.Serialize(ref Pins);
    }

    public override int GetAllocatedSize()
    {
        return BaseAllocationSize + Pins!.Count * Pin.BaseAllocationSize;
    }

    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize(), revision,
            compressionFlags);
        Serializer.Serialize(ref this);
        return new SerializationData(
            Serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Pins,
            SerializationType.BINARY,
            Serializer.GetDependencies());
    }
}