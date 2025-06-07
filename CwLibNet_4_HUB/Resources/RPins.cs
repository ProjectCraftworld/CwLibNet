using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.Types.Profile;
using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Resources;

public class RPins : Resource
{
    public const int BaseAllocationSize = 0x10;

    public List<Pin>? Pins = [];

    public override void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
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
        Serializer.SetCurrentSerializer(serializer);
        Serialize(serializer);
        return new SerializationData(
            Serializer.GetCurrentSerializer().GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Pins,
            SerializationType.BINARY,
            Serializer.GetCurrentSerializer().GetDependencies());
    }
}