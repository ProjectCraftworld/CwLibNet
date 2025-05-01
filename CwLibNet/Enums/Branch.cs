using CwLibNet.Enums;

namespace CwLibNet.Types;

public readonly struct Branch : IEquatable<Branch>
{
    public static readonly Branch 
        None = new(0x0, 0x0, 0x0),
        Leerdammer = new((int)Revisions.LD_HEAD, 0x4c44, (int)Revisions.LD_MAX),
        Double11 = new((int)Revisions.D1_HEAD, 0x4431, (int)Revisions.D1_MAX),
        Mizuki = new((int)Revisions.MZ_HEAD, 0x4d5a, (int)Revisions.MZ_MAX);

    public readonly int Head;
    public readonly short Id;
    public readonly short Revision;
        
    public Branch(int head, int id, int revision)
    {
        Head = head;
        Id = (short) id;
        Revision = (short) revision;
    }
    
    public static Branch? FromId(short id) => id switch
    {
        0x0 => None,
        0x4c44 => Leerdammer,
        0x4431 => Double11,
        0x4d5a => Mizuki,
        _ => null
    };

    public override bool Equals(object? obj)
    {
        return obj is Branch other && Equals(other);
    }
    
    public bool Equals(Branch other)
    {
        if (GetHashCode() != other.GetHashCode()) return false;
        return Head == other.Head && Id == other.Id && Revision == other.Revision;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Head.GetHashCode();
            hash = hash * 23 + Id.GetHashCode();
            hash = hash * 23 + Revision.GetHashCode();
            return hash;
        }
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