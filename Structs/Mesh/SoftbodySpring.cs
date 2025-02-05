using CwLibNet.IO;
using CwLibNet.IO.Serialization;
namespace CwLibNet.Structs.Mesh
{
    public class SoftbodySpring : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public short A, B;
        public float restLengthSq;
        public SoftbodySpring()
        {
        }

        public SoftbodySpring(int a, int b, float restLengthSq)
        {
            this.A = (short)a;
            this.B = (short)b;
            this.restLengthSq = restLengthSq;
        }

        public override void Serialize(Serializer serializer)
        {
            A = serializer.Serialize(A);
            B = serializer.Serialize(B);
            restLengthSq = serializer.Serialize(restLengthSq);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}