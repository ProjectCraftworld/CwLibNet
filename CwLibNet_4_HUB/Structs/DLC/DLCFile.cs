using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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
        
    public void Serialize()
    {
        var revision = Serializer.GetRevision();

        Serializer.Serialize(ref Directory);
        Serializer.Serialize(ref File);
        Serializer.Serialize(ref ContentId);
        Serializer.Serialize(ref InGameCommerceId);
        if (revision.GetVersion() >= 0x264)
            Serializer.Serialize(ref CategoryId);
        Serializer.Serialize(ref Flags);

        if (revision.GetSubVersion() > 0x20c)
        {
            if (Serializer.IsWriting())
            {
                var output = new int[NonPlanGuids.Count];
                for (var i = 0; i < output.Length; ++i)
                {
                    var guid = NonPlanGuids[i];
                    output[i] = guid == null ? 0 : (int) guid?.Value!;
                }
                Serializer.Serialize(ref output);
            }
            else
            {
                var Serializer.Serialize(ref null);
                if (guids != null)
                {
                    NonPlanGuids = new List<GUID?>(guids.Length);
                    foreach (var t in guids)
                        NonPlanGuids.Add(new GUID(t));
                }
            }
        }

        if (Serializer.IsWriting())
        {
            var output = new int[Guids.Count];
            for (var i = 0; i < output.Length; ++i)
            {
                var guid = Guids[i];
                output[i] = guid == null ? 0 : (int) guid?.Value!;
            }
            Serializer.Serialize(ref output);
        }
        else
        {
            var Serializer.Serialize(ref null);
            if (guids != null)
            {
                Guids = new List<GUID?>(guids.Length);
                foreach (var t in guids)
                    Guids.Add(new GUID(t));
            }
        }

        if (revision.Has(Branch.Double11, 0x85))
            Serializer.Serialize(ref TypeMask);
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