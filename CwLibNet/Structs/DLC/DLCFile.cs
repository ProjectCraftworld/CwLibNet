using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.DLC;

public class DLCFile: ISerializable
{
    public const int BaseAllocationSize = 0x30; // Example size, adjust as needed

    public string? Directory;
    public string? File;
    public string? ContentId;
    public string? InGameCommerceId;
    public string? CategoryId;
    public List<GUID?>? Guids = [];
    public List<GUID?>? NonPlanGuids = [];
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