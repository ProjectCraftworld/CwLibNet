using System.CodeDom;
using System.Diagnostics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using Branch = CwLibNet.Types.Branch;

namespace CwLibNet.Resources
{
    public class RDLC : Resource
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;

        public List<DLCGUID>? GUIDs = [];
        public List<DLCFile>? Files = [];

        public override void Serialize(Serializer serializer)
        {
            GUIDs = serializer.Arraylist<DLCGUID>(GUIDs);
            Files = serializer.Arraylist<DLCFile>(Files);
        }

        public override int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.GUIDs != null) size += (this.GUIDs.Count * DLCGUID.BaseAllocationSize);
            if (this.Files == null) return size;
            size += this.Files.Sum(file => file.GetAllocatedSize());
            return size;
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            Serializer serializer = new Serializer(this.GetAllocatedSize(), revision, compressionFlags);
            serializer.Struct<RDLC>(this);
            return new SerializationData(
                serializer.GetBuffer(),
                revision,
                compressionFlags,
                ResourceType.DownloadableContent,
                SerializationType.BINARY,
                serializer.GetDependencies()
            );
        }
    }

    public class DLCFile: ISerializable
    {
        public const int BaseAllocationSize = 0x30; // Example size, adjust as needed

        public string? Directory;
        public string? File;
        public string? ContentId;
        public string? InGameCommerceId;
        public string? CategoryId;
        public List<GUID?> Guids = [];
        public List<GUID?> NonPlanGuids = [];
        public int Flags = DLCFileFlags.NONE;
        public int TypeMask;
        
        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();

            Directory = serializer.Str(Directory);
            File = serializer.Str(File);
            ContentId = serializer.Str(ContentId);
            InGameCommerceId = serializer.Str(InGameCommerceId);
            if (revision.GetVersion() >= 0x264)
                CategoryId = serializer.Str(CategoryId);
            Flags = serializer.I32(Flags);

            if (revision.GetSubVersion() > 0x20c)
            {
                if (serializer.IsWriting())
                {
                    int[] output = new int[NonPlanGuids.Count];
                    for (int i = 0; i < output.Length; ++i)
                    {
                        GUID? guid = NonPlanGuids[i];
                        output[i] = guid == null ? 0 : (int) guid?.Value!;
                    }
                    serializer.Intvector(output);
                }
                else
                {
                    int[]? guids = serializer.Intvector(null);
                    if (guids != null)
                    {
                        NonPlanGuids = new List<GUID?>(guids.Length);
                        for (int i = 0; i < guids.Length; ++i)
                            NonPlanGuids.Add(new GUID(guids[i]));
                    }
                }
            }

            if (serializer.IsWriting())
            {
                int[] output = new int[Guids.Count];
                for (int i = 0; i < output.Length; ++i)
                {
                    GUID? guid = Guids[i];
                    output[i] = guid == null ? 0 : (int) guid?.Value!;
                }
                serializer.Intvector(output);
            }
            else
            {
                int[]? guids = serializer.Intvector(null);
                if (guids != null)
                {
                    this.Guids = new List<GUID?>(guids.Length);
                    foreach (var t in guids)
                        this.Guids.Add(new GUID(t));
                }
            }

            if (revision.Has(Branch.Double11, 0x85))
                TypeMask = serializer.I32(TypeMask);
        }

        public int GetAllocatedSize()
        {
            int size = DLCFile.BaseAllocationSize;
            if (this.Directory != null) size += this.Directory.Length;
            if (this.File != null) size += this.File.Length;
            if (this.ContentId != null) size += this.ContentId.Length;
            if (this.InGameCommerceId != null) size += this.InGameCommerceId.Length;
            if (this.CategoryId != null) size += this.CategoryId.Length;
            if (this.Guids != null)
                size += this.Guids.Count * 0x4;
            if (this.NonPlanGuids != null)
                size += this.NonPlanGuids.Count * 0x4;
            return size;
        }
    }

    public class DLCGUID: ISerializable
    {
        public const int BaseAllocationSize = 0x10;
        
        public GUID? GUID;
        public int Flags = DLCFileFlags.NONE;

        public void Serialize(Serializer serializer)
        {
            GUID = serializer.Guid(GUID);
            Flags = serializer.I32(Flags);
        }

        public int GetAllocatedSize()
        {
            return BaseAllocationSize;
        }
    }
}