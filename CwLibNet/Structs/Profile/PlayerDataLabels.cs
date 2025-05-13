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
        public const int BASE_ALLOCATION_SIZE = 0x10;

        public DataLabelValue[] values;
        public NetworkOnlineID[] protectedIDs;

        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int head = revision.GetVersion();

            values = serializer.Array<DataLabelValue>(values);
            if (revision.Has(Branch.Double11, (int)Revisions.D1ProtectedLabels) || head >= (int)Revisions.Datalabels)
                protectedIDs = serializer.Array<NetworkOnlineID>(protectedIDs);
        }

        public int GetAllocatedSize() 
        {
            int size = BASE_ALLOCATION_SIZE;
            if (values != null) 
                foreach (DataLabelValue value in values)
                    size += value.GetAllocatedSize();
            if (protectedIDs != null)
                size += protectedIDs.Length * NetworkOnlineID.BASE_ALLOCATION_SIZE;
            return size;
        }
    }
}