using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Profile;

namespace CwLibNet.Resources;

public class RPins : Resource
{
    public const int BaseAllocationSize = 0x10;

    public List<Pin>? Pins = [];

    public override void Serialize(Serializer serializer)
    {
        Pins = serializer.Arraylist(Pins);
    }

    public override int GetAllocatedSize()
    {
        return BaseAllocationSize + Pins!.Count * Pin.BaseAllocationSize;
    }

    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        var serializer = new Serializer(GetAllocatedSize(), revision,
            compressionFlags);
        serializer.Struct(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Pins,
            SerializationType.BINARY,
            serializer.GetDependencies());
    }
}