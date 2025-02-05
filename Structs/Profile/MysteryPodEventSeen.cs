using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Profile
{
    /// <summary>
    /// I don't think either of these variable
    /// names or correct, or if this is actually some
    /// other structure, but only place I've seen it used,
    /// so whatever.
    /// </summary>
    public class MysteryPodEventSeen : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public int id;
        public int type;
        public override void Serialize(Serializer serializer)
        {
            id = serializer.Serialize(id);
            type = serializer.Serialize(type);
        }

        public virtual int GetAllocatedSize()
        {
            return MysteryPodEventSeen.BASE_ALLOCATION_SIZE;
        }
    }
}