using System;
using CwLibNet.Types.Data;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.DLC
{
    public class DLCGUID : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10; // for the GUID
        public GUID guid;
        public int flags = DLCFileFlags.NONE;

        public virtual void Serialize(Serializer serializer)
        {
            guid = (GUID)serializer.Guid(guid);
            flags = serializer.I32(flags);
        }

        public virtual int GetAllocatedSize()
        {
            return DLCGUID.BASE_ALLOCATION_SIZE;
        }
    }
}