using System.Diagnostics.CodeAnalysis;
using CwLibNet.Enums;

namespace CwLibNet.Types;

public readonly struct Branch(int head, int id, int revision) : IEquatable<Branch>
{
    public static readonly Branch 
        None = new(0x0, 0x0, 0x0),
        Leerdammer = new((int)Revisions.LdHead, 0x4c44, (int)Revisions.LdMax),
        Double11 = new((int)Revisions.D1Head, 0x4431, (int)Revisions.D1Max),
        Mizuki = new((int)Revisions.MzHead, 0x4d5a, (int)Revisions.MzMax);
    
    public int Head { get; } = head;
    public short Id { get; } = (short) id;
    public short Revision { get; } = (short) revision;
    
    public static Branch? FromId(short id) => id switch
    {
        0x0 => None,
        0x4c44 => Leerdammer,
        0x4431 => Double11,
        0x4d5a => Mizuki,
        _ => null
    };
    
    public bool Equals(Branch other)
    {
        return Head == other.Head && Id == other.Id && Revision == other.Revision;
    }

    public override bool Equals(object? obj)
    {
        return obj is Branch other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Head, Id, Revision);
    }

    public static bool operator ==(Branch a, Branch b)
    {
        return a.Equals(b);
    }
    
    public static bool operator !=(Branch a, Branch b)
    {
        return !a.Equals(b);
    }
}