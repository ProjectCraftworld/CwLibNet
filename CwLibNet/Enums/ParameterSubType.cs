using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum ParameterSubType : int
    {
        // NONE(0x0)
        NONE,
        // XY(0x1)
        XY,
        // ZW(0x2)
        ZW,
        // Z(0x3)
        Z 
    }

    public sealed class ParameterSubBody

    {
        private readonly ParameterSubType value;

        public ParameterSubBody
        (int value)
        {
            this.value = (ParameterSubType)value;
        }

        public ParameterSubType getValue()
        {
            return value;
        }

        public static ParameterSubBody
         fromValue(int value)
        {
            if (Enum.IsDefined(typeof(ParameterSubType), value))
            {
                return new ParameterSubBody
                (value);
            }
            return default(ParameterSubBody); 
        }
    }
}