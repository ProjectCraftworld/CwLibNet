namespace CwLibNet.Enums
{
    public enum CostumePieceCategory
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

        // --------------------
        // TODO enum body members
        // CostumePieceCategory(int index) {
        //     this.index = index;
        //     this.flag = (1 << index);
        // }
        // private final int index;
        // private final int flag;
        // public int getIndex() {
        //     return this.index;
        // }
        // public int getFlag() {
        //     return this.flag;
        // }
        // public static int getFlags(EnumSet<CostumePieceCategory> set) {
        //     int flags = 0;
        //     CostumePieceCategory[] categories = CostumePieceCategory.values();
        //     for (int i = 0; i < categories.length; ++i) {
        //         CostumePieceCategory category = categories[i];
        //         if (set.contains(category))
        //             flags |= (1 << i);
        //     }
        //     return flags;
        // }
        // public static EnumSet<CostumePieceCategory> fromFlags(int flags) {
        //     EnumSet<CostumePieceCategory> set = EnumSet.noneOf(CostumePieceCategory.class);
        //     CostumePieceCategory[] categories = CostumePieceCategory.values();
        //     for (int i = 0; i < categories.length; ++i) {
        //         if ((flags & (1 << i)) == 0)
        //             continue;
        //         set.add(categories[i]);
        //     }
        //     return set;
        // }
        // public static String getNameFromIndex(int index) {
        //     CostumePieceCategory[] categories = CostumePieceCategory.values();
        //     for (int i = 0; i < categories.length; ++i) {
        //         if (categories[i].index == index)
        //             return categories[i].name().toLowerCase();
        //     }
        //     return "" + index;
        // }
        // public static String getPrimaryName(EnumSet<CostumePieceCategory> set) {
        //     if (set == null || set.isEmpty())
        //         return "none";
        //     return set.iterator().next().name().toLowerCase();
        // }
        // --------------------
    }
}