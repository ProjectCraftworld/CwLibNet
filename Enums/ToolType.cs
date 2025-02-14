using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum ToolType
    {
        // NONE(0)
        NONE,
        // CURSOR(1)
        CURSOR,
        // VERTEX_EDIT(2)
        VERTEX_EDIT,
        // GLUE(3)
        GLUE,
        // FILL(4)
        FILL,
        // STICKER(5)
        STICKER,
        // PHOTO(6)
        PHOTO,
        // STICKER_CUTTER(7)
        STICKER_CUTTER,
        // SHAPE_STICKER_CUTTER(8)
        SHAPE_STICKER_CUTTER,
        // RESET_PLAYER_AVATAR(9)
        RESET_PLAYER_AVATAR,
        // UV_EDIT(10)
        UV_EDIT,
        // CAPTURE_TOOL(11)
        CAPTURE_TOOL,
        // ELECTRIC(12)
        ELECTRIC,
        // FIRE(13)
        FIRE,
        // PLASMA(14)
        PLASMA,
        // POISON(15)
        POISON,
        // UNLETHAL(16)
        UNLETHAL,
        // PHOTO_FROM_XMB(17)
        PHOTO_FROM_XMB,
        // RESET_COSTUME(18)
        RESET_COSTUME,
        // RANDOM_COSTUME(19)
        RANDOM_COSTUME,
        // SAVE_COSTUME(20)
        SAVE_COSTUME,
        // WASH_COSTUME(21)
        WASH_COSTUME,
        // DELETE_COMMUNITY_PHOTOS(22)
        DELETE_COMMUNITY_PHOTOS,
        // DELETE_COMMUNITY_OBJECTS(23)
        DELETE_COMMUNITY_OBJECTS,
        // POD_RESET(24)
        POD_RESET,
        // POD_SAVE(25)
        POD_SAVE,
        // PLANET_RESET(26)
        PLANET_RESET,
        // PLANET_SAVE(27)
        PLANET_SAVE,
        // PLANET_RESET_MATERIAL(28)
        PLANET_RESET_MATERIAL,
        // PICK_EMITTER_OBJECT(29)
        PICK_EMITTER_OBJECT,
        // PICK_GUN_OBJECT(30)
        PICK_GUN_OBJECT,
        // PLANET_RESET_ALL(31)
        PLANET_RESET_ALL,
        // STICKER_WASH(32)
        STICKER_WASH,
        // SLICE_N_DICE(33)
        SLICE_N_DICE,
        // UNPHYSICS(34)
        UNPHYSICS,
        // PAINT(35)
        PAINT,
        // PICK_UP_POWER_UP_OBJECT(36)
        PICK_UP_POWER_UP_OBJECT,
        // GROUPING(37)
        GROUPING,
        // WASH(38)
        WASH,
        // SET_LAUNCHER_POSITION(39)
        SET_LAUNCHER_POSITION,
        // LEVEL_IMPORTER(40)
        LEVEL_IMPORTER,
        // DISK_CAPTURE(41)
        DISK_CAPTURE,
        // PICK_ZONE(42)
        PICK_ZONE,
        // RAIL_EDIT(43)
        RAIL_EDIT,
        // ADD_LIGHT(44)
        ADD_LIGHT,
        // ADVENTURE_RESET(45)
        ADVENTURE_RESET,
        // ADVENTURE_SAVE(46)
        ADVENTURE_SAVE 

        // --------------------
        // TODO enum body members
        // private final byte value;
        // ToolType(int value) {
        //     this.value = (byte) value;
        // }
        // public Byte getValue() {
        //     return this.value;
        // }
        // /**
        //  * Attempts to get a ToolType from value.
        //  *
        //  * @param value Tool type value
        //  * @return ToolType
        //  */
        // public static ToolType fromValue(byte value) {
        //     for (ToolType type : ToolType.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return ToolType.NONE;
        // }
        // --------------------
    }
}