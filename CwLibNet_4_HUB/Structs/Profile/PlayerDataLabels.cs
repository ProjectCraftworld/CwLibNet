using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class PlayerDataLabels : ISerializable
    {
        public const int BaseAllocationSize = 0x10;

        public DataLabelValue[]? Values;
        public NetworkOnlineId[]? ProtectedIDs;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            var revision = Serializer.GetCurrentSerializer().GetRevision();
            var head = revision.GetVersion();

            Serializer.Serialize(ref Values);
            if (revision.Has(Branch.Double11, (int)Revisions.D1_PROTECTED_LABELS) || head >= (int)Revisions.DATALABELS)
                Serializer.Serialize(ref ProtectedIDs);
        }

        public int GetAllocatedSize() 
        {
            var size = BaseAllocationSize;
            if (Values != null) size += Values.Sum(value => value.GetAllocatedSize());
            if (ProtectedIDs != null)
                size += ProtectedIDs.Length * NetworkOnlineId.BaseAllocationSize;
            return size;
        }
    }
}