using System;
using System.Collections.Generic;

namespace CwLibNet.Enums
{
    public sealed class InventoryObjectSubType
    {
        public static readonly int NONE = 0x0;
        public static readonly int EARTH = 0x0;
        public static readonly int MOON = 0x1;
        public static readonly int ADVENTURE = 0x2;
        public static readonly int EXTERNAL = 0x4;
        public static readonly int CREATURE_MASK_GIANT = 0x00200000;
        public static readonly int CREATURE_MASK_DWARF = 0x00400000;
        public static readonly int CREATURE_MASK_BIRD = 0x00600000;
        public static readonly int CREATURE_MASK_QUAD = 0x00800000;
        public static readonly int CREATURE_MASK = 0x03E00000;
        public static readonly int PAINTING = 0x02000000;
        public static readonly int SPECIAL_COSTUME = 0x04000000;
        public static readonly int PLAYER_AVATAR = 0x08000000;
        public static readonly int EARTH_DECORATION = 0x10000000;
        public static readonly int MADE_BY_ME = 0x20000000;
        public static readonly int MADE_BY_OTHERS = 0x40000000;
        public static readonly uint FULL_COSTUME = 0x80000000;
        public static readonly int MADE_BY_ANYONE = MADE_BY_ME | MADE_BY_OTHERS;
        public static string GetTypeString(HashSet<InventoryObjectType> type, int subType)
        {
            if (type.Count == 0)
                return "NONE";
            if (type.Contains(InventoryObjectType.PLAYER_COLOUR))
                return String.Format("PLAYER_COLOUR_%2d", subType);
            if (type.Contains(InventoryObjectType.COSTUME) || type.Contains(InventoryObjectType.USER_COSTUME))
            {
                if ((subType & FULL_COSTUME) != 0)
                    return "OUTFIT";
                if ((subType & SPECIAL_COSTUME) != 0)
                    return "SPECIAL";
                CostumePieceCategory[] categories = (CostumePieceCategory[])Enum.GetValues(typeof(CostumePieceCategory));
                for (int i = 0; i < categories.Length; ++i)
                    if ((subType & (1 << i)) != 0)
                        return categories[i].ToString();
                return "NONE";
            }

            if (type.Contains(InventoryObjectType.STICKER) && ((subType & PAINTING) != 0))
                return "PAINTING";
            if (type.Contains(InventoryObjectType.USER_PLANET))
            {
                if (subType == EARTH)
                    return "EARTH";
                else if (subType == MOON)
                    return "MOON";
                else if (subType == ADVENTURE)
                    return "ADVENTURE";
                else if (subType == EXTERNAL)
                    return "EXTERNAL";
            }

            return "NONE";
        }
    }
}