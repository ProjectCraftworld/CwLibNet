using static CwLibNet.IO.ValueEnum<int>;

namespace CwLibNet.Enums.ValueEnum
{
    public enum CellGcmEnumForGtf : int
    {
        // B8(0x81)
        B8,
        // A1R5G5B5(0x82)
        A1R5G5B5,
        // A4R4G4B4(0x83)
        A4R4G4B4,
        // R5G6B5(0x84)
        R5G6B5,
        // A8R8G8B8(0x85)
        A8R8G8B8,
        // DXT1(0x86)
        DXT1,
        // DXT3(0x87)
        DXT3,
        // DXT5(0x88)
        DXT5,
        // G8B8(0x8b)
        G8B8,
        // R5G5B5(0x8f)
        R5G5B5 

        // --------------------
        // TODO enum body members
    }
    public sealed class CellGcmBodyMembers
    {

        private readonly CellGcmEnumForGtf value;
        CellGcmBodyMembers(int value)
        {
            this.value = (CellGcmEnumForGtf)value;
        }

        public int getValue()
        {
            return (int)this.value;
        }




        public static CellGcmBodyMembers fromValue(int value)
        {
            if (Enum.IsDefined(typeof(CellGcmEnumForGtf), value))
            {
                return new CellGcmBodyMembers(value);
            }
            return default(CellGcmBodyMembers);
        }
    }
}