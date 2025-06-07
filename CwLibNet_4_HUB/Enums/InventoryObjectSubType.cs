using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public sealed class InventoryObjectSubType
{
    public const int None = 0x0;
    public const int Earth = 0x0;
    public const int Moon = 0x1;
    public const int Adventure = 0x2;
    public const int External = 0x4;
    public const int CreatureMaskGiant = 0x00200000;
    public const int CreatureMaskDwarf = 0x00400000;
    public const int CreatureMaskBird = 0x00600000;
    public const int CreatureMaskQuad = 0x00800000;
    public const int CreatureMask = 0x03E00000;
    public const int Painting = 0x02000000;
    public const int SpecialCostume = 0x04000000;
    public const int PlayerAvatar = 0x08000000;
    public const int EarthDecoration = 0x10000000;
    public const int MadeByMe = 0x20000000;
    public const int MadeByOthers = 0x40000000;
    public const uint FullCostume = 0x80000000;
    public const int MadeByAnyone = MadeByMe | MadeByOthers;
    public static string GetTypeString(HashSet<InventoryObjectType> type, int subType)
    {
        if (type.Count == 0)
            return "NONE";
        if (type.Contains(InventoryObjectType.PLAYER_COLOUR))
            return $"PLAYER_COLOUR_{subType}";
        if (type.Contains(InventoryObjectType.COSTUME) || type.Contains(InventoryObjectType.USER_COSTUME))
        {
            if ((subType & FullCostume) != 0)
                return "OUTFIT";
            if ((subType & SpecialCostume) != 0)
                return "SPECIAL";
            var categories = (CostumePieceCategory[])Enum.GetValues(typeof(CostumePieceCategory));
            for (var i = 0; i < categories.Length; ++i)
                if ((subType & (1 << i)) != 0)
                    return categories[i].ToString();
            return "NONE";
        }

        if (type.Contains(InventoryObjectType.STICKER) && (subType & Painting) != 0)
            return "PAINTING";
        if (!type.Contains(InventoryObjectType.USER_PLANET)) return "NONE";
        return subType switch
        {
            Earth => "EARTH",
            Moon => "MOON",
            Adventure => "ADVENTURE",
            _ => subType == External ? "EXTERNAL" : "NONE"
        };
    }
}