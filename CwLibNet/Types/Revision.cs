namespace CwLibNet.Types
{
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
            this.Head = revision;
            this.BranchId = (short) (branchDescription >> 0x10);
            this.BranchRevision = (short) (branchDescription & 0xFFFF);
        }

        /**
         * Forms revision with no branch data.
         *
         * @param revision Head revision
         */
        public Revision(int revision)
        {
            this.Head = revision;
            this.BranchId = 0;
            this.BranchRevision = 0;
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
            this.Head = revision;
            this.BranchId = (short) branchId;
            this.BranchRevision = (short) branchRevision;
        }

        public bool IsLbp1()
        {
            return this.Head <= Lbp1FinalRevision;
        }

        public bool IsLbp2()
        {
            return !this.IsLbp1() && !this.IsLbp3() && !this.IsVita();
        }

        public bool IsLbp3()
        {
            return this.Head >> 0x10 != 0;
        }

        public bool IsLeerdammer()
        {
            return this.Is(Branch.Leerdammer);
        }

        public bool IsVita()
        {
            int version = this.GetVersion();
            return this.BranchId == Branch.Double11.Id && version is >= 0x3c1 and <= 0x3e2;
        }

        public bool IsToolkit()
        {
            return this.Is(Branch.Mizuki);
        }

        public bool Is(Branch branch)
        {
            if (branch == Branch.Double11) return this.IsVita();
            return this.BranchId == branch.Id && this.Head == branch.Head;
        }

        public bool Has(Branch branch, int revision)
        {
            if (!this.Is(branch)) return false;
            return this.BranchRevision >= revision;
        }

        public bool After(Branch branch, int revision)
        {
            if (!this.Is(branch)) return false;
            return this.BranchRevision > revision;
        }

        public bool Before(Branch branch, int revision)
        {
            if (!this.Is(branch)) return false;
            return this.BranchRevision < revision;
        }

        /**
         * Gets the LBP3 specific revision of the head revision.
         *
         * @return LBP3 head revision
         */
        public int GetSubVersion()
        {
            return (this.Head >>> 16) & 0xFFFF;
        }

        /**
         * Gets the LBP1/LBP2/V revision of the head revision.
         *
         * @return LBP1/2/V head revision
         */
        public int GetVersion()
        {
            return this.Head & 0xFFFF;
        }
    
        public override string ToString()
        {
            if (this.BranchId != 0) return $"Revision: (r{this.Head:D}, b{this.BranchId:X4}:{this.BranchRevision:X4})";
            return $"Revision: (r{this.Head:D})";
        }
    }
}