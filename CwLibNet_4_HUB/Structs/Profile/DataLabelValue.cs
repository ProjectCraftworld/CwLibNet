using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile;

public class DataLabelValue: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public NetworkOnlineId? CreatorId;
    public string? LabelName;
    public int LabelIndex;
    public float[]? Analogue;
    public byte[]? Ternary;

    
    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var head = revision.GetVersion();

        CreatorId = serializer.Struct(CreatorId);
        LabelIndex = serializer.I32(LabelIndex);

        if (revision.IsVita())
        {

            if (revision.Has(Branch.Double11, (int)Revisions.D1_LABEL_ANALOGUE_ARRAY))
                Analogue = serializer.Floatarray(Analogue);
            else if (revision.Has(Branch.Double11, (int)Revisions.D_1DATALABELS))
            {
                if (serializer.IsWriting())
                {
                    var value = Analogue != null && Analogue.Length != 0 ? Analogue[0] : 0.0f;
                    serializer.GetOutput().F32(value);
                }
                else
                    Analogue = [serializer.GetInput().F32()];
            }

            if (revision.Has(Branch.Double11, (int)Revisions.D1_LABEL_TERNARY))
                Ternary = serializer.Bytearray(Ternary);
        }
        else if (head >= (int)Revisions.DATALABELS)
        {
            Analogue = serializer.Floatarray(Analogue);
            Ternary = serializer.Bytearray(Ternary);
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