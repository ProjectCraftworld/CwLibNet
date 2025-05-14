using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
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
        var revision = serializer.GetRevision();

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
                var output = new int[NonPlanGuids.Count];
                for (var i = 0; i < output.Length; ++i)
                {
                    var guid = NonPlanGuids[i];
                    output[i] = guid == null ? 0 : (int) guid?.Value!;
                }
                serializer.Intvector(output);
            }
            else
            {
                var guids = serializer.Intvector(null);
                if (guids != null)
                {
                    NonPlanGuids = new List<GUID?>(guids.Length);
                    foreach (var t in guids)
                        NonPlanGuids.Add(new GUID(t));
                }
            }
        }

        if (serializer.IsWriting())
        {
            var output = new int[Guids.Count];
            for (var i = 0; i < output.Length; ++i)
            {
                var guid = Guids[i];
                output[i] = guid == null ? 0 : (int) guid?.Value!;
            }
            serializer.Intvector(output);
        }
        else
        {
            var guids = serializer.Intvector(null);
            if (guids != null)
            {
                Guids = new List<GUID?>(guids.Length);
                foreach (var t in guids)
                    Guids.Add(new GUID(t));
            }
        }

        if (revision.Has(Branch.Double11, 0x85))
            TypeMask = serializer.I32(TypeMask);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Directory != null) size += Directory.Length;
        if (File != null) size += File.Length;
        if (ContentId != null) size += ContentId.Length;
        if (InGameCommerceId != null) size += InGameCommerceId.Length;
        if (CategoryId != null) size += CategoryId.Length;
        if (Guids != null)
            size += Guids.Count * 0x4;
        if (NonPlanGuids != null)
            size += NonPlanGuids.Count * 0x4;
        return size;
    }
}