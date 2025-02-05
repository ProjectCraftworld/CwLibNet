using Cwlib.Enums;
using CwLibNet.IO
using CwLibNet.IO.Serialization;
using CwLibNet.Types;

namespace Cwlib.Structs.Dlc
{
    public class DLCGUID : Serializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public GUID guid;
        public int flags = DLCFileFlags.NONE;
        public virtual void Serialize(Serializer serializer)
        {
            guid = serializer.Guid(guid);
            flags = serializer.Serialize(flags);
        }

        public virtual int GetAllocatedSize()
        {
            return DLCGUID.BASE_ALLOCATION_SIZE;
        }
    }
}