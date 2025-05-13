using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile 
{
    public class DataLabelValue : ISerializable 
    {
        public const int BASE_ALLOCATION_SIZE = 0x30;

        public NetworkOnlineID creatorID;
        public string labelName;
        public int labelIndex;
        public float[] analogue;
        public byte[] ternary;

        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            int head = revision.GetVersion();

            creatorID = serializer.Struct<NetworkOnlineID>(creatorID);
            labelIndex = serializer.I32(labelIndex);

            if (revision.IsVita())
            {
                if (revision.Has(Branch.Double11, (int)Revisions.D1LabelAnalogueArray))
                    analogue = serializer.Floatarray(analogue);
                else if (revision.Has(Branch.Double11, (int)Revisions.D1Datalabels))
                {
                    if (serializer.IsWriting()) 
                    {
                        float value = analogue != null &&
                            analogue.Length != 0 ? analogue[0] : 0f;
                        serializer.GetOutput().F32(value);
                    }
                    else
                        analogue = new float[] { serializer.GetInput().F32() };
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1LabelTernary))
                    ternary = serializer.Bytearray(ternary);
            }
            else if (head >= (int)Revisions.Datalabels)
            {
                analogue = serializer.Floatarray(analogue);
                ternary = serializer.Bytearray(ternary);
            }
        }

        public int GetAllocatedSize() 
        {
            int size = DataLabelValue.BASE_ALLOCATION_SIZE;
            if (this.analogue != null)
            {
                size += (this.analogue.Length * 4);
            }
            if (this.ternary != null)
            {
                size += this.ternary.Length;
            }
            return size;
        }
    }
}