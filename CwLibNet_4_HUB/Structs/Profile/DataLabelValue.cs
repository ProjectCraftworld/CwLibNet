using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Profile;

public class DataLabelValue: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public NetworkOnlineId? CreatorId;
    public string? LabelName;
    public int LabelIndex;
    public float[]? Analogue;
    public byte[]? Ternary;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var head = revision.GetVersion();

        Serializer.Serialize(ref CreatorId);
        Serializer.Serialize(ref LabelIndex);

        if (revision.IsVita())
        {

            if (revision.Has(Branch.Double11, (int)Revisions.D1_LABEL_ANALOGUE_ARRAY))
                Serializer.Serialize(ref Analogue);
            else if (revision.Has(Branch.Double11, (int)Revisions.D_1DATALABELS))
            {
                if (Serializer.IsWriting())
                {
                    var value = Analogue != null && Analogue.Length != 0 ? Analogue[0] : 0.0f;
                    Serializer.GetCurrentSerializer().GetOutput().F32(value);
                }
                else
                    Analogue = [Serializer.GetCurrentSerializer().GetInput().F32()];
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D1_LABEL_TERNARY))
                Serializer.Serialize(ref Ternary);
        }
        else if (head >= (int)Revisions.DATALABELS)
        {
            Serializer.Serialize(ref Analogue);
            Serializer.Serialize(ref Ternary);
        }
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Analogue != null)
            size += Analogue.Length * 4;
        if (Ternary != null)
            size += Ternary.Length;
        return size;
    }


}