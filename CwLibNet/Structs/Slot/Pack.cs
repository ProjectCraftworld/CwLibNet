using System;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Slot 
{
    public class Pack
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x40 + Slot.BASE_ALLOCATION_SIZE;
        public ContentsType contentsType = ContentsType.LEVEL;
        public ResourceDescriptor mesh = ContentsType.LEVEL.getBadgeMesh();
        public Slot slot = new Slot();
        public string contentID;
        public long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // seconds since epoch
        public bool crossBuyCompatible = false; // TODO: Set to true if cross-buy compatible

        public void Serialize(Serializer serializer) 
        {
            contentsType = serializer.Enum32(contentsType);
            mesh = serializer.Resource(mesh, ResourceType.MESH, true);
            slot = serializer.Struct(slot);
            contentID = serializer.Str(contentID);
            timeStamp = serializer.S64(timeStamp);
            if (serializer.GetRevision().IsVita()) crossBuyCompatible = serializer.Serialize(crossBuyCompatible); 
        }
    }
}