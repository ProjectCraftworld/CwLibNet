using Cwlib.Io.Serializer;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types;
using CwLibNet.Types.Profile;

namespace CwLibNet.Resources
{
    public class RPins : Resource
    {
        public const int BaseAllocationSize = 0x10;

        public List<Pin> Pins = new();

        public override void Serialize(Serializer serializer)
        {
            Pins = serializer.Array<Pin>(Pins);
        }

        public override int GetAllocatedSize()
        {
            return BaseAllocationSize + Pins.Count * Pin.BaseAllocationSize;
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
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
}