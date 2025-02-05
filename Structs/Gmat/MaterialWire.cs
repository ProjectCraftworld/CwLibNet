using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Gmat
{
    /// <summary>
    /// A wire that connects two boxes on the shader graph,
    /// used by Media Molecule in their shader editor.
    /// </summary>
    public class MaterialWire : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x10;
        public static readonly int SWIZZLE_ELEMENT_COUNT = 5;
        public int boxFrom, boxTo;
        public byte portFrom, portTo;
        public byte[] swizzle = new byte[SWIZZLE_ELEMENT_COUNT];
        /// <summary>
        /// Empty constructor for serialization.
        /// </summary>
        public MaterialWire()
        {
        }

        /// <summary>
        /// Empty constructor for serialization.
        /// </summary>
        /// <summary>
        /// Creates a connection between two boxes.
        /// </summary>
        public MaterialWire(int boxFrom, int boxTo, int portFrom, int portTo)
        {
            this.boxFrom = boxFrom;
            this.boxTo = boxTo;
            this.portFrom = (byte)portFrom;
            this.portTo = (byte)portTo;
        }

        /// <summary>
        /// Empty constructor for serialization.
        /// </summary>
        /// <summary>
        /// Creates a connection between two boxes.
        /// </summary>
        public override void Serialize(Serializer serializer)
        {
            boxFrom = serializer.Serialize(boxFrom);
            boxTo = serializer.Serialize(boxTo);
            portFrom = serializer.Serialize(portFrom);
            portTo = serializer.Serialize(portTo);
            for (int i = 0; i < SWIZZLE_ELEMENT_COUNT; ++i)
                swizzle[i] = serializer.Serialize(swizzle[i]);
        }

        /// <summary>
        /// Empty constructor for serialization.
        /// </summary>
        /// <summary>
        /// Creates a connection between two boxes.
        /// </summary>
        public virtual int GetAllocatedSize()
        {
            return MaterialWire.BASE_ALLOCATION_SIZE;
        }
    }
}