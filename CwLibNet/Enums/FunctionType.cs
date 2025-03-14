using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum FunctionType : int
    {
        // NORMAL(0)
        NORMAL,
        // GETTER(1)
        GETTER,
        // SETTER(2)
        SETTER 
    }

    public sealed class FunctionBody
    {
        private readonly FunctionType value;

        FunctionBody(int value)
        {
            this.value = (FunctionType)(value & 0xFF);
        }

        public FunctionType getValue()
        {
            return this.value;
        }

        public static FunctionBody fromValue(int value)
        {
            if (Enum.IsDefined(typeof(FunctionType), value))
                return new FunctionBody(value);
            return default(FunctionBody);
        }
    }
}