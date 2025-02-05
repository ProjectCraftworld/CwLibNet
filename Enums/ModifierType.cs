namespace CwLibNet.Enums
{
    public enum ModifierType
    {
        // STATIC(0x0)
        STATIC,
        // NATIVE(0x1)
        NATIVE,
        // EPHEMERAL(0x2)
        EPHEMERAL,
        // PINNED(0x3)
        PINNED,
        // CONST(0x4)
        CONST,
        // PUBLIC(0x5)
        PUBLIC,
        // PROTECTED(0x6)
        PROTECTED,
        // PRIVATE(0x7)
        PRIVATE,
        // PROPERTY(0x8)
        PROPERTY,
        // ABSTRACT(0x9)
        ABSTRACT,
        // VIRTUAL(0xa)
        VIRTUAL,
        // OVERRIDE(0xb)
        OVERRIDE,
        // DIVERGENT(0xc)
        DIVERGENT,
        // EXPORT(0xd)
        EXPORT 

        // --------------------
        // TODO enum body members
        // private final int value;
        // ModifierType(int value) {
        //     this.value = value;
        // }
        // public int getValue() {
        //     return this.value;
        // }
        // public static short getFlags(EnumSet<ModifierType> set) {
        //     short flags = 0;
        //     if (set == null)
        //         return flags;
        //     for (ModifierType type : set) flags |= (1 << type.value);
        //     return flags;
        // }
        // public static EnumSet<ModifierType> fromValue(int value) {
        //     EnumSet<ModifierType> bitset = EnumSet.noneOf(ModifierType.class);
        //     for (ModifierType type : ModifierType.values()) if ((value & (1 << type.value)) != 0)
        //         bitset.add(type);
        //     return bitset;
        // }
        // public static String toModifierString(EnumSet<ModifierType> set) {
        //     String[] modifiers = new String[set.size()];
        //     int i = 0;
        //     for (ModifierType type : set) modifiers[i++] = type.toString().toLowerCase();
        //     return String.join(" ", modifiers);
        // }
        // --------------------
    }
}