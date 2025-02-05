namespace CwLibNet.Enums
{
    public class Branch
    {
        public static readonly Branch NONE = new Branch(0x0, 0x0, 0x0);
        // dummy branch
        public static readonly Branch LEERDAMMER = new Branch(Revisions.LD_HEAD, 0x4c44, Revisions.LD_MAX);
        // branched revision for Leerdammer update in LBP1
        // Tag: LD
        public static readonly Branch DOUBLE11 = new Branch(Revisions.D1_HEAD, 0x4431, Revisions.D1_MAX);
        // Vita branched revision; final branch revision is 0x3e2, but it can go
        // as early as 0x3c1 in earlier branch revisions
        // Tag: D1
        public static readonly Branch MIZUKI = new Branch(Revisions.MZ_HEAD, 0x4d5a, Revisions.MZ_MAX);
        // Custom branched revision for Toolkit custom resources
        // Tag: MZ

        public int Head {get;}
        public short Id {get;}
        public short Revision {get;}

        private Branch(int head, int id, int revision)
        {
            Head = head;
            Id = (short)id;
            Revision = (short)revision;
        }

        public int getHead()
        {
            return Head;
        }

        public short getID()
        {
            return Id;
        }

        public short getRevision()
        {
            return Revision;
        }

        public static Branch FromID(short id)
        {
            foreach (var branch in new[] {NONE, LEERDAMMER, DOUBLE11, MIZUKI})
            {
                if (branch.Id == id)
                {
                    return branch;
                }
            }
            return null;
        }
    }
}