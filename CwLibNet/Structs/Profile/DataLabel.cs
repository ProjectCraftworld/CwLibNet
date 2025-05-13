using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class DataLabel : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x8;

        public int labelIndex;
        public string name;

        public void Serialize(Serializer serializer)
        {
            labelIndex = serializer.I32(labelIndex);
            name = serializer.Wstr(name);
        }

        public int GetAllocatedSize() 
        {
            int size = DataLabel.BASE_ALLOCATION_SIZE;
            if (this.name != null)
            {
                size += (this.name.Length * 2);
            }
            return size;
        }
    }
}