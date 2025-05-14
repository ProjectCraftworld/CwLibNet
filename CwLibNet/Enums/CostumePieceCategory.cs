namespace CwLibNet.Enums;

[Flags]
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
}

public sealed class CostumePieceBodyMembers
{
    private readonly CostumePieceCategory index;

    public CostumePieceBodyMembers(CostumePieceCategory index)
    {
        this.index = index;
    }

    public CostumePieceCategory GetIndex()
    {
        return index;
    }

    public static CostumePieceCategory GetFlags(CostumePieceBodyMembers set)
    {
        var flags = 0;
        var categories = Values();
        for (var i = 0; i < categories.Length; ++i)
        {
            var category = categories[i];
            if (set.Contains(category))
                flags |= 1 << i;
        }
        return (CostumePieceCategory)flags;
    }

    public static HashSet<CostumePieceCategory> FromFlags(CostumePieceBodyMembers flags)
    {
        var set = new HashSet<CostumePieceCategory>();
        var categories = Values();
        for (var i = 0; i < categories.Length; ++i)
        {
            var category = categories[i];
            if (((int)flags.GetIndex() & (1 << i)) == 0) continue;
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

    public static string GetNameFromIndex(int index)
    {
        var categories = (CostumePieceCategory[])Enum.GetValues(typeof(CostumePieceCategory));
        foreach (var t in categories)
        {
            if ((int)t == index)
                return t.ToString().ToLower();
        }
        return "" + index;
    }

    public static string GetPrimaryName(HashSet<CostumePieceCategory> set)
    {
        if (set == null || set.Count == 0) return nameof(CostumePieceCategory.BEARD).ToLower(); // or any default value
        return set.First().ToString().ToLower();
    }
}