using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.Types.Profile;
using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;

namespace CwLibNet4Hub.Resources;

public class RPins : Resource
{
    public const int BaseAllocationSize = 0x10;

    public List<Pin>? Pins = [];

    public override void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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