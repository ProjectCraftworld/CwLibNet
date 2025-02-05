using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Joint
{
    public class FCurve : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x40;
        public Vector4 x = Vector4.Zero;
        public Vector4 y = Vector4.Zero;
        public Vector4 p = Vector4.Zero;
        public CurveType type = CurveType.CONSTANT;
        public override void Serialize(Serializer serializer)
        {
            x = serializer.V4(x);
            y = serializer.V4(y);
            p = serializer.V4(p);
            type = serializer.Enum32(type);
        }

        public virtual int GetAllocatedSize()
        {
            return FCurve.BASE_ALLOCATION_SIZE;
        }
    }
}