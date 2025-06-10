using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

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
}

public sealed class ModifierBody
{
    private readonly ModifierType value;

    private ModifierBody(int value)
    {
        this.value = (ModifierType)value;
    }

    public ModifierType getValue()
    {
        return value;
    }

    public static short GetFlags(HashSet<ModifierType>? set)
    {
        short flags = 0;
        if (set == null) return flags;
        return set.Aggregate(flags, (current, value) => (short)((ushort)current | (1 << (short)value)));
    }

    public static HashSet<ModifierType> fromValue(int value)
    {
        var bitset = new HashSet<ModifierType>();
        foreach (ModifierType type in Enum.GetValues(typeof(ModifierType)))
        {
            if ((value & (1 << (int)type)) != 0)
            {
                bitset.Add(type);
            }
        }
        return bitset;
    }

    public static string ToModifierString(HashSet<ModifierType> set)
    {
        var modifiers = new string[set.Count];
        var i = 0;
        foreach (var value in set)
            modifiers[i++] = value.ToString().ToLower();
        return string.Join(" ", modifiers);
    }
}