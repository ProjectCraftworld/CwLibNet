using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public enum FunctionType
{
    // NORMAL(0)
    NORMAL,
    // GETTER(1)
    GETTER,
    // SETTER(2)
    SETTER 
}

public sealed class FunctionBody(int value)
{
    private readonly FunctionType value = (FunctionType)(value & 0xFF);

    public FunctionType GetValue()
    {
        return value;
    }

    public static FunctionBody? FromValue(int value)
    {
        return Enum.IsDefined(typeof(FunctionType), value) ? new FunctionBody(value) : null;
    }
}