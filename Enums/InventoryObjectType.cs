using CwLibNet.Types.Data;
namespace CwLibNet.Enums
{
    public enum InventoryObjectType
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

        // --------------------
        // TODO enum body members
        // private final int value, flags;
        // InventoryObjectType(int value) {
        //     this.value = value;
        //     this.flags = GameVersion.LBP1 | GameVersion.LBP2 | GameVersion.LBP3;
        // }
        // InventoryObjectType(int value, int flags) {
        //     this.value = value;
        //     this.flags = flags;
        // }
        // public static int getFlags(EnumSet<InventoryObjectType> set) {
        //     int flags = 0;
        //     for (InventoryObjectType type : set) flags |= type.value;
        //     return flags;
        // }
        // public static EnumSet<InventoryObjectType> fromFlags(int flags, Revision revision) {
        //     int version = GameVersion.getFlag(revision);
        //     EnumSet<InventoryObjectType> set = EnumSet.noneOf(InventoryObjectType.class);
        //     for (InventoryObjectType type : InventoryObjectType.values()) {
        //         if ((type.flags & version) == 0)
        //             continue;
        //         if ((type.value & flags) != 0)
        //             set.add(type);
        //     }
        //     return set;
        // }
        // public static boolean has(int flags, InventoryObjectType type) {
        //     return (flags & type.value) != 0;
        // }
        // public boolean has(int flags) {
        //     return (flags & this.value) != 0;
        // }
        // public static String getPrimaryName(EnumSet<InventoryObjectType> set) {
        //     if (set == null || set.isEmpty())
        //         return "none";
        //     return set.iterator().next().name().toLowerCase();
        // }
        // --------------------
    }
}