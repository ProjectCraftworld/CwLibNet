using System;
using System.Collections.Generic;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile 
{
    public class PlayerDataLabels : ISerializable
    {
        public const int BaseAllocationSize = 0x10;

        public DataLabelValue[]? Values;
        public NetworkOnlineId[]? ProtectedIDs;

        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int head = revision.GetVersion();

            Values = serializer.Array(Values);
            if (revision.Has(Branch.Double11, (int)Revisions.D1_PROTECTED_LABELS) || head >= (int)Revisions.DATALABELS)
                ProtectedIDs = serializer.Array(ProtectedIDs);
        }

        public int GetAllocatedSize() 
        {
            int size = BaseAllocationSize;
            if (Values != null) size += Values.Sum(value => value.GetAllocatedSize());
            if (ProtectedIDs != null)
                size += ProtectedIDs.Length * NetworkOnlineId.BaseAllocationSize;
            return size;
        }
    }
}