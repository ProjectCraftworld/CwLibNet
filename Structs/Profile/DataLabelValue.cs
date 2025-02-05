using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class DataLabelValue : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x30;
        public NetworkOnlineID creatorID;
        public string labelName;
        public int labelIndex;
        public float[] analogue;
        public byte[] ternary;
        public override void Serialize(Serializer serializer)
        {
            Revision revision = serializer.Serialize();
            int head = revision.GetVersion();
            creatorID = serializer.Serialize(creatorID, typeof(NetworkOnlineID));
            labelIndex = serializer.Serialize(labelIndex);
            if (revision.IsVita())
            {
                if (revision.Has(Branch.DOUBLE11, Revisions.D1_LABEL_ANALOGUE_ARRAY))
                    analogue = serializer.Serialize(analogue);
                else if (revision.Has(Branch.DOUBLE11, Revisions.D1_DATALABELS))
                {
                    if (serializer.IsWriting())
                    {
                        float value = analogue != null && analogue.length != 0 ? analogue[0] : 0F;
                        serializer.GetOutput().f32(value);
                    }
                    else
                        analogue = new float[]
                        {
                            serializer.GetInput().f32()
                        };
                }

                if (revision.Has(Branch.DOUBLE11, Revisions.D1_LABEL_TERNARY))
                    ternary = serializer.Serialize(ternary);
            }
            else if (head >= Revisions.DATALABELS)
            {
                analogue = serializer.Serialize(analogue);
                ternary = serializer.Serialize(ternary);
            }
        }

        public virtual int GetAllocatedSize()
        {
            int size = DataLabelValue.BASE_ALLOCATION_SIZE;
            if (this.analogue != null)
                size += (this.analogue.length * 4);
            if (this.ternary != null)
                size += (this.ternary.length);
            return size;
        }
    }
}