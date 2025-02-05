using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.IO.Streams;

namespace CwLibNet.Structs.Instrument
{
    public class Note : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x4;
        // Each type is actually a byte, but that's pretty inconvenient, since
        // there's no unsigned type
        public int x = 0, y = 0, volume = 0x60, timbre = 0x40;
        // Each type is actually a byte, but that's pretty inconvenient, since
        // there's no unsigned type
        public bool triplet, end;
        // Each type is actually a byte, but that's pretty inconvenient, since
        // there's no unsigned type
        public Note()
        {
        }

        // Each type is actually a byte, but that's pretty inconvenient, since
        // there's no unsigned type
        public Note(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // Each type is actually a byte, but that's pretty inconvenient, since
        // there's no unsigned type
        public override void Serialize(Serializer serializer)
        {
            if (serializer.IsWriting())
            {
                MemoryOutputStream stream = serializer.GetOutput();
                stream.u8((x & 0x7f) | ((triplet) ? 0x80 : 0x0));
                stream.u8((y & 0x7f) | ((end) ? 0x80 : 0x0));
                stream.u8(volume & 0xff);
                stream.u8(timbre & 0xff);
                return;
            }

            byte[] struct = serializer.GetInput().Bytes(4);
            triplet = (struct[0] >> 0x7) != 0;
            x = (struct[0] & 0x7f);
            end = (struct[1] >> 0x7) != 0;
            y = (struct[1] & 0x7f);
            volume = struct[0x2];
            timbre = struct[0x3];
        }

        // Each type is actually a byte, but that's pretty inconvenient, since
        // there's no unsigned type
        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}