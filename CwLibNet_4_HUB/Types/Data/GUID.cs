using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
﻿namespace CwLibNet4Hub.Types.Data;

public readonly struct GUID : IEquatable<GUID>
{

    public GUID(long value)
    {
        Value = value;
    }
        
    public readonly long Value;

    public override string ToString()
    {
        return "g" + Value;
    }

    public override bool Equals(object? obj)
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

    public bool Equals(GUID other)
    {
        return Value == other.Value;
    }
}