using System;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Slot 
{
    public class Pack
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x40 + Slot.BaseAllocationSize;
        public ContentsType ContentsType = ContentsType.LEVEL;
        public ResourceDescriptor? Mesh;
        public Slot Slot = new();
        public string? ContentId;
        public long TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // seconds since epoch
        public bool CrossBuyCompatible = false;

        public void Serialize(Serializer serializer) 
        {
            ContentsType = serializer.Enum32(ContentsType);
            Mesh = serializer.Resource(Mesh, ResourceType.Mesh, true);
            Slot = serializer.Struct(Slot);
            ContentId = serializer.Str(ContentId);
            TimeStamp = serializer.S64(TimeStamp);
            if (serializer.GetRevision().IsVita()) CrossBuyCompatible = serializer.Bool(CrossBuyCompatible); 
        }
    }
}