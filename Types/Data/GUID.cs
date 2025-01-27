using System.Diagnostics.CodeAnalysis;

namespace CwLibNet.Types.Data;

public readonly struct GUID(uint value)
{
    public readonly uint Value = value;

    public override string ToString()
    {
        return "g" + this.Value;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return Value.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(GUID a, GUID b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(GUID a, GUID b)
    {
        return !(a == b);
    }
}