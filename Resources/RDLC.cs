using System.CodeDom;
using System.Diagnostics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Resources;
using CwLibNet.Structs;
using CwLibNet.Types;

namespace CwLibNet.Resources
{
    public class RDLC : Resource
    {
        public readonly int BASE_ALLOCATION_SIZE = 0x10;

        public List<DLCGUID> GUIDs = new List<DLCGUID>();
        public List<DLCFile> Files = new List<DLCFile>();

        public override void Serialize(Serializer serializer)
        {
            GUIDs = serializer.List<DLCGUID>(GUIDs);
            Files = serializer.List<DLCFile>(Files);
        }


        public override int GetAllocatedSize()
        {
            int size = RDLC.BASE_ALLOCATION_SIZE;
            if (this.GUIDs != null) size += (this.GUIDs.size() * DLCGUID.BASE_ALLOCATION_SIZE);
            if (this.Files != null)
            {
                foreach (DLCFile file in this.Files)
                    size += file.GetAllocatedSize();
            }
            return size;
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            Serializer serializer = new Serializer(this.GetAllocatedSize(), revision, compressionFlags);
            serializer.Struct<RDLC>(this);
            return new SerializationData(
                serializer.getBuffer(),
                revision,
                compressionFlags,
                ResourceType.DOWNLOADABLE_CONTENT,
                SerializationType.BINARY,
                serializer.getDependencies()
            );
        }
    }

    public class DLCFile
    {
        public const int BASE_ALLOCATION_SIZE = 0x20; // Example size, adjust as needed

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }

    public class DLCGUID
    {
    }
}