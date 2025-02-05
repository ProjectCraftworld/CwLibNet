using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Inventory
{
    public class PhotoUser : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x30;
        public string PSID;
        public string user;
        public Vector4 bounds = new Vector4().Zero();
        public PhotoUser()
        {
        }

        public PhotoUser(string psid)
        {
            if (psid == null)
                return;
            if (psid.Length() > 0x14)
                psid = psid.Substring(0, 0x14);
            this.PSID = psid;
            this.user = psid;
        }

        public override void Serialize(Serializer serializer)
        {
            PSID = serializer.Serialize(PSID, 0x14);
            user = serializer.Serialize(user);
            bounds = serializer.Serialize(bounds);
        }

        public virtual int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.user != null)
                size += (user.Length() * 2);
            return size;
        }
    }
}