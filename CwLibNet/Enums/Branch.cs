namespace CwLibNet.Enums
{
    public class Branch
    {
        public static readonly Branch? NONE = new Branch(0x0, 0x0, 0x0);
        // dummy branch
        public static readonly Branch? LEERDAMMER = new Branch((int)Revisions.LdHead, 0x4c44, (int)Revisions.LdMax);
        // branched revision for Leerdammer update in LBP1
        // Tag: LD
        public static readonly Branch? DOUBLE11 = new Branch((int)Revisions.D1Head, 0x4431, (int)Revisions.D1Max);
        // Vita branched revision; final branch revision is 0x3e2, but it can go
        // as early as 0x3c1 in earlier branch revisions
        // Tag: D1
        public static readonly Branch? MIZUKI = new Branch((int)Revisions.MzHead, 0x4d5a, (int)Revisions.MzMax);
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

        public int GetHead()
        {
            return Head;
        }

        public short GetId()
        {
            return Id;
        }

        public short GetRevision()
        {
            return Revision;
        }

        public static Branch? FromID(short id)
        {
            return new[] { NONE, LEERDAMMER, DOUBLE11, MIZUKI }.FirstOrDefault(branch => branch!.Id == id);
        }
    }
}