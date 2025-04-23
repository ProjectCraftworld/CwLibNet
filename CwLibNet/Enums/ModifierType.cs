using static CwLibNet.IO.ValueEnum<int>;

namespace CwLibNet.Enums
{
    public enum ModifierType : int
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
    }

    public sealed class ModifierBody
    {
        private readonly ModifierType value;

        ModifierBody(int value)
        {
            this.value = (ModifierType)value;
        }

        public ModifierType getValue()
        {
            return this.value;
        }

        public static short GetFlags(HashSet<ModifierType>? set)
        {
            short flags = 0;
            if (set == null) return flags;
            return set.Aggregate(flags, (current, value) => (short)((ushort)current | (1 << (short)value)));
        }

        public static HashSet<ModifierType> fromValue(int value)
        {
            HashSet<ModifierType> bitset = new HashSet<ModifierType>();
            foreach (ModifierType type in Enum.GetValues(typeof(ModifierType)))
            {
                if ((value & (1 << (int)type)) != 0)
                {
                    bitset.Add(type);
                }
            }
            return bitset;
        }

        public static String ToModifierString(HashSet<ModifierType> set)
        {
            String[] modifiers = new String[set.Count];
            int i = 0;
            foreach (ModifierType value in set)
            modifiers[i++] = value.ToString().ToLower();
            return string.Join(" ", modifiers);
        }
    }
}