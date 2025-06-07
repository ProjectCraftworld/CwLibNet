using CwLibNet.Enums;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Types.Data;

public readonly struct Revision
{
    public const int Lbp1FinalRevision = 0x272;

    public readonly int Head;
    public readonly short BranchId;
    public readonly short BranchRevision;

    public short GetBranchRevision()
    {
        return BranchRevision;
    }

    /**
     * Forms revision with branch data.
     *
     * @param revision          Head revision
     * @param branchDescription Revision branch descriptor
     */
    public Revision(int revision, int branchDescription)
    {
        Head = revision;
        BranchId = (short) (branchDescription >> 0x10);
        BranchRevision = (short) (branchDescription & 0xFFFF);
    }

    /**
     * Forms revision with no branch data.
     *
     * @param revision Head revision
     */
    public Revision(int revision)
    {
        Head = revision;
        BranchId = 0;
        BranchRevision = 0;
    }

    /**
     * Forms revision with branch ID and revision.
     *
     * @param revision       Head revision
     * @param branchID       Branch ID of revision
     * @param branchRevision Revision of branch
     */
    public Revision(int revision, int branchId, int branchRevision)
    {
        Head = revision;
        BranchId = (short) branchId;
        BranchRevision = (short) branchRevision;
    }

    public bool IsLbp1()
    {
        return Head <= Lbp1FinalRevision;
    }

    public bool IsLbp2()
    {
        return !IsLbp1() && !IsLbp3() && !IsVita();
    }

    public bool IsLbp3()
    {
        return Head >> 0x10 != 0;
    }

    public bool IsLeerdammer()
    {
        return Is(Branch.Leerdammer);
    }

    public bool IsVita()
    {
        var version = GetVersion();
        return BranchId == Branch.Double11.Id && version is >= 0x3c1 and <= 0x3e2;
    }

    public bool IsToolkit()
    {
        return Is(Branch.Mizuki);
    }

    public bool Is(Branch branch)
    {
        if (branch == Branch.Double11) return IsVita();
        return BranchId == branch.Id && Head == branch.Head;
    }

    public bool Has(Branch branch, int revision)
    {
        if (!Is(branch)) return false;
        return BranchRevision >= revision;
    }

    public bool After(Branch branch, int revision)
    {
        if (!Is(branch)) return false;
        return BranchRevision > revision;
    }

    public bool Before(Branch branch, int revision)
    {
        if (!Is(branch)) return false;
        return BranchRevision < revision;
    }

    /**
     * Gets the LBP3 specific revision of the head revision.
     *
     * @return LBP3 head revision
     */
    public int GetSubVersion()
    {
        return (Head >>> 16) & 0xFFFF;
    }

    /**
     * Gets the LBP1/LBP2/V revision of the head revision.
     *
     * @return LBP1/2/V head revision
     */
    public int GetVersion()
    {
        return Head & 0xFFFF;
    }
    
    public override string ToString()
    {
        if (BranchId != 0) return $"Revision: (r{Head:D}, b{BranchId:X4}:{BranchRevision:X4})";
        return $"Revision: (r{Head:D})";
    }

    public static explicit operator int(Revision v)
    {
        throw new NotImplementedException();
    }
}