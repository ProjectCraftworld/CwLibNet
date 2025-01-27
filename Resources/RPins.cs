using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types;
using CwLibNet.Types.Profile;

namespace CwLibNet.Resources;

public class RPins : IResource
{
    public const int BaseAllocationSize = 0x10;

    public List<Pin> Pins = [];

    public void Serialize(Serializer serializer)
    {
        Pins = serializer.List<Pin>(Pins);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize + Pins.Count * Pin.BaseAllocationSize;
    }

    public SerializationData Build(Revision revision, byte compressionFlags)
    {
        Serializer serializer = new Serializer(this.GetAllocatedSize(), revision,
            compressionFlags);
        serializer.Struct<RPins>(this);
        return new SerializationData(
            serializer.getBuffer(),
            revision,
            compressionFlags,
            ResourceType.Pins,
            SerializationType.BINARY,
            serializer.getDependencies());
    }
}