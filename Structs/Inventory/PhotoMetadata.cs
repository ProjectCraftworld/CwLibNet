using System;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace Cwlib.Structs.Inventory
{
    public class PhotoMetadata : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x50;
        public ResourceDescriptor photo;
        public SlotID level = new SlotID();
        public string levelName;
        public SHA1 levelHash = new SHA1();
        public PhotoUser[] users;
        public long timestamp = new Date().GetTime() / 1000;
        public override void Serialize(Serializer serializer)
        {
            photo = serializer.Serialize(photo, ResourceType.TEXTURE, true);
            level = serializer.Serialize(level, typeof(SlotID));
            levelName = serializer.Serialize(levelName);
            levelHash = serializer.Serialize(levelHash);
            timestamp = serializer.Serialize(timestamp);
            users = serializer.Serialize(users, typeof(PhotoUser));
        }

        public virtual int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.levelName != null)
                size += (levelName.Length() * 2);
            if (this.users != null)
                size += (this.users.length * PhotoUser.BASE_ALLOCATION_SIZE);
            return size;
        }
    }
}