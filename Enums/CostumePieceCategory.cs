using static CwLibNet.IO.ValueEnum<int>;

namespace CwLibNet.Enums
{
    public enum CostumePieceCategory : int
    {
        // BEARD(0)
        BEARD,
        // FEET(1)
        FEET,
        // EYES(2)
        EYES,
        // GLASSES(3)
        GLASSES,
        // MOUTH(4)
        MOUTH,
        // MOUSTACHE(5)
        MOUSTACHE,
        // NOSE(6)
        NOSE,
        // HAIR(7)
        HAIR,
        // HEAD(8)
        HEAD,
        // NECK(9)
        NECK,
        // TORSO(10)
        TORSO,
        // LEGS(11)
        LEGS,
        // HANDS(12)
        HANDS,
        // WAIST(13)
        WAIST 
    }

    public sealed class CostumePieceBodyMembers
    {
        private readonly CostumePieceCategory index;
        private readonly CostumePieceCategory flag;

        public CostumePieceBodyMembers(CostumePieceCategory index)
        {
            this.index = (CostumePieceCategory)index;
        }

        public CostumePieceCategory getIndex()
        {
            return this.index;
        }

        public CostumePieceCategory getFlag()
        {
            return this.flag;
        }

        public static CostumePieceCategory GetFlags(CostumePieceBodyMembers set)
        {
            int flags = 0;
            CostumePieceCategory[] categories = Values();
            for (int i = 0; i < categories.Length; ++i)
            {
                CostumePieceCategory category = categories[i];
                if (set.Contains(category))
                    flags |= 1 << i;
            }
            return (CostumePieceCategory)flags;
        }

        public static HashSet<CostumePieceCategory> FromFlags(CostumePieceBodyMembers flags)
        {
            HashSet<CostumePieceCategory> set = new HashSet<CostumePieceCategory>();
            CostumePieceCategory[] categories = Values();
            for (int i = 0; i < categories.Length; ++i)
            {
                CostumePieceCategory category = categories[i];
                if (((int)flags.getIndex() & (1 << i)) == 0) continue;
                set.Add(categories[i]);
            }
            return set;
        }

        public static CostumePieceCategory[] Values()
        {
            return (CostumePieceCategory[])Enum.GetValues(typeof(CostumePieceCategory));
        }

        public bool Contains(CostumePieceCategory category)
        {
            return (index & category) == category;
        }

        public static String getNameFromIndex(int index)
        {
            CostumePieceCategory[] categories = (CostumePieceCategory[])Enum.GetValues(typeof(CostumePieceCategory));
            for (int i = 0; i < categories.Length; ++i)
            {
                if ((int)categories[i] == index)
                    return categories[i].ToString().ToLower();
            }
            return "" + index;
        }

        public static String getPrimaryName(HashSet<CostumePieceCategory> set)
        {
            if (set == null || set.Count == 0) return CostumePieceCategory.BEARD.ToString().ToLower(); // or any default value
            return set.First().ToString().ToLower();
        }
    }
}