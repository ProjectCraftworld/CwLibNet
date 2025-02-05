using System;
using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Animation
{
    public class Locator : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x12;
        public Vector3 position;
        public string name;
        public byte looping, type;
        public override void Serialize(Serializer serializer)
        {
            position = serializer.V3(position);
            name = serializer.Str(name);
            looping = serializer.I8(looping);
            type = serializer.I8(type);
        }

        public virtual int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.name != null)
                size += (this.name.Length());
            return size;
        }
    }
}