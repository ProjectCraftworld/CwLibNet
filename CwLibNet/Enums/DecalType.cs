using static CwLibNet.IO.ValueEnum<int>;

namespace CwLibNet.Enums
{
    public enum DecalType : int
    {
        // STICKER(0)
        STICKER,
        // PAINT(1)
        PAINT,
        // EYETOY(2)
        EYETOY 
    }

    public sealed class DecalBody
    {
        private readonly int value;

        DecalBody(int value)
        {
            this.value = value & 0xFF;
        }

        public DecalType getValue()
        {
            return (DecalType)this.value;
        }

        public static DecalBody fromValue(byte value)
        {
            if (Enum.IsDefined(typeof(DecalType), value))
                return new DecalBody(value);
            return default(DecalBody);
        }
    }
}