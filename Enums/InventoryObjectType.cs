using System;
using System.Collections.Generic;
using CwLibNet.Types;
using static CwLibNet.IO.ValueEnum<int>;
namespace CwLibNet.Enums
{
    public enum InventoryObjectType : int
    {
        // NONE(0)
        NONE,
        // PRIMITIVE_MATERIAL(1 << 0)
        PRIMITIVE_MATERIAL,
        // READYMADE(1 << 1)
        READYMADE,
        // DECORATION(1 << 2)
        DECORATION,
        // STICKER(1 << 3)
        STICKER,
        // COSTUME(1 << 4)
        COSTUME,
        // COSTUME_MATERIAL(1 << 5)
        COSTUME_MATERIAL,
        // JOINT(1 << 6)
        JOINT,
        // USER_OBJECT(1 << 7)
        USER_OBJECT,
        // BACKGROUND(1 << 8)
        BACKGROUND,
        // GAMEPLAY_KIT(1 << 9)
        GAMEPLAY_KIT,
        // USER_STICKER(1 << 10)
        USER_STICKER,
        // PRIMITIVE_SHAPE(1 << 11)
        PRIMITIVE_SHAPE,
        // SEQUENCER(1 << 12, GameVersion.LBP2 | GameVersion.LBP3)
        SEQUENCER,
        // DANGER(1 << 13)
        DANGER,
        // EYETOY(1 << 14)
        EYETOY,
        // GADGET(1 << 15)
        GADGET,
        // TOOL(1 << 16)
        TOOL,
        // SACKBOT_MESH(1 << 17, GameVersion.LBP3)
        SACKBOT_MESH,
        // CREATURES_CHARACTERS(1 << 18, GameVersion.LBP3)
        CREATURES_CHARACTERS,
        // PLAYER_COLOUR(1 << 19)
        PLAYER_COLOUR,
        // USER_COSTUME(1 << 20)
        USER_COSTUME,
        // MUSIC(1 << 21)
        MUSIC,
        // SOUND(1 << 22)
        SOUND,
        // PHOTOBOOTH(1 << 23)
        PHOTOBOOTH,
        // USER_PLANET(1 << 24, GameVersion.LBP2 | GameVersion.LBP3)
        USER_PLANET,
        // LEVEL_KEY(1 << 25, GameVersion.LBP2 | GameVersion.LBP3)
        LEVEL_KEY,
        // EMITTED_ITEM(1 << 26, GameVersion.LBP2 | GameVersion.LBP3)
        EMITTED_ITEM,
        // GUN_ITEM(1 << 27, GameVersion.LBP2 | GameVersion.LBP3)
        GUN_ITEM,
        // NPC_COSTUME(1 << 28, GameVersion.LBP2 | GameVersion.LBP3)
        NPC_COSTUME,
        // INSTRUMENT(1 << 29, GameVersion.LBP2 | GameVersion.LBP3)
        INSTRUMENT,
        // USER_POD(1 << 30, GameVersion.LBP2 | GameVersion.LBP3)
        USER_POD,
        // COSTUME_TWEAKER_TOOL(1 << 31, GameVersion.LBP3)
        COSTUME_TWEAKER_TOOL,
        // /* These are all exclusive to LBP1, and are replaced with
        //        different types in LBP2 onward. */
        // PAINT(1 << 12, GameVersion.LBP1)
        PAINT,
        // FLOOD_FILL(1 << 17, GameVersion.LBP1)
        FLOOD_FILL,
        // STICKER_TOOL(1 << 18, GameVersion.LBP1)
        STICKER_TOOL,
        // COSTUME_TOOL(1 << 24, GameVersion.LBP1)
        COSTUME_TOOL,
        // PLAN_TOOL(1 << 25, GameVersion.LBP1)
        PLAN_TOOL,
        // PHOTO_TOOL(1 << 26, GameVersion.LBP1)
        PHOTO_TOOL,
        // PICTURE_TOOLS(1 << 27, GameVersion.LBP1)
        PICTURE_TOOLS,
        // COM_PHOTO_TOOLS(1 << 28, GameVersion.LBP1)
        COM_PHOTO_TOOLS,
        // COM_OBJECT_TOOLS(1 << 29, GameVersion.LBP1)
        COM_OBJECT_TOOLS,
        // USER_POD_LBP1(1 << 30, GameVersion.LBP1)
        USER_POD_LBP1,
        // POD_TOOL_LBP1(1 << 31, GameVersion.LBP1)
        POD_TOOL_LBP1,
        // /* These are exclusive to LBP2 */
        // EDIT_MODE_TOOL(1 << 17, GameVersion.LBP2)
        EDIT_MODE_TOOL,
        // POD_TOOL_LBP2(1 << 18, GameVersion.LBP2)
        POD_TOOL_LBP2,
        // EARTH_TOOL(1 << 31, GameVersion.LBP2)
        EARTH_TOOL
    }

    public sealed class InvObjectBody
    {
        private readonly InventoryObjectType value;
        private readonly int flags;

        public InvObjectBody(InventoryObjectType value)
        {
            this.value = value;
            this.flags = (int)(GameVersion.LBP1 | GameVersion.LBP2 | GameVersion.LBP3);
        }

        public InvObjectBody(InventoryObjectType value, int flags)
        {
            this.value = value;
            this.flags = flags;
        }

        public static int GetFlags(HashSet<InventoryObjectType> set)
        {
            int flags = 0;
            foreach (InventoryObjectType type in set)
            {
                flags |= (int)type;
            }
            return flags;
        }

        public static HashSet<InventoryObjectType> fromFlags(int flags, Revision revision)
        {
            int version = GameVersion.GetFlag(revision);
            HashSet<InventoryObjectType> set = new HashSet<InventoryObjectType>();
            foreach (InventoryObjectType type in Enum.GetValues(typeof(InventoryObjectType)))
            {
                if ((flags & version) == 0)
                    continue;
                if ((flags & (int)type) != 0)
                    set.Add(type);
            }
            return set;
        }

        public static bool Has(int flags, InventoryObjectType type)
        {
            return (flags & (int)type) != 0;
        }

        public bool Has(int flags)
        {
            return (flags & (int)this.value) != 0;
        }

        public static string GetPrimaryName(HashSet<InventoryObjectType> set)
        {
            if (set == null || set.Count == 0)
                return "none";
            return set.First().ToString().ToLower();
        }
    }
}