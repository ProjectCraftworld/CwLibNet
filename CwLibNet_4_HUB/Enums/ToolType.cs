using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum ToolType
{
    // NONE(0)
    NONE = 0,
    // CURSOR(1)
    CURSOR = 1,
    // VERTEX_EDIT(2)
    VERTEX_EDIT = 2,
    // GLUE(3)
    GLUE = 3,
    // FILL(4)
    FILL = 4,
    // STICKER(5)
    STICKER = 5,
    // PHOTO(6)
    PHOTO = 6,
    // STICKER_CUTTER(7)
    STICKER_CUTTER = 7,
    // SHAPE_STICKER_CUTTER(8)
    SHAPE_STICKER_CUTTER = 8,
    // RESET_PLAYER_AVATAR(9)
    RESET_PLAYER_AVATAR = 9,
    // UV_EDIT(10)
    UV_EDIT = 10,
    // CAPTURE_TOOL(11)
    CAPTURE_TOOL = 11,
    // ELECTRIC(12)
    ELECTRIC = 12,
    // FIRE(13)
    FIRE = 13,
    // PLASMA(14)
    PLASMA = 14,
    // POISON(15)
    POISON = 15,
    // UNLETHAL(16)
    UNLETHAL = 16,
    // PHOTO_FROM_XMB(17)
    PHOTO_FROM_XMB = 17,
    // RESET_COSTUME(18)
    RESET_COSTUME = 18,
    // RANDOM_COSTUME(19)
    RANDOM_COSTUME = 19,
    // SAVE_COSTUME(20)
    SAVE_COSTUME = 20,
    // WASH_COSTUME(21)
    WASH_COSTUME = 21,
    // DELETE_COMMUNITY_PHOTOS(22)
    DELETE_COMMUNITY_PHOTOS = 22,
    // DELETE_COMMUNITY_OBJECTS(23)
    DELETE_COMMUNITY_OBJECTS = 23,
    // POD_RESET(24)
    POD_RESET = 24,
    // POD_SAVE(25)
    POD_SAVE = 25,
    // PLANET_RESET(26)
    PLANET_RESET = 26,
    // PLANET_SAVE(27)
    PLANET_SAVE = 27,
    // PLANET_RESET_MATERIAL(28)
    PLANET_RESET_MATERIAL = 28,
    // PICK_EMITTER_OBJECT(29)
    PICK_EMITTER_OBJECT = 29,
    // PICK_GUN_OBJECT(30)
    PICK_GUN_OBJECT = 30,
    // PLANET_RESET_ALL(31)
    PLANET_RESET_ALL = 31,
    // STICKER_WASH(32)
    STICKER_WASH = 32,
    // SLICE_N_DICE(33)
    SLICE_N_DICE = 33,
    // UNPHYSICS(34)
    UNPHYSICS = 34,
    // PAINT(35)
    PAINT = 35,
    // PICK_UP_POWER_UP_OBJECT(36)
    PICK_UP_POWER_UP_OBJECT = 36,
    // GROUPING(37)
    GROUPING = 37,
    // WASH(38)
    WASH = 38,
    // SET_LAUNCHER_POSITION(39)
    SET_LAUNCHER_POSITION = 39,
    // LEVEL_IMPORTER(40)
    LEVEL_IMPORTER = 40,
    // DISK_CAPTURE(41)
    DISK_CAPTURE = 41,
    // PICK_ZONE(42)
    PICK_ZONE = 42,
    // RAIL_EDIT(43)
    RAIL_EDIT = 43,
    // ADD_LIGHT(44)
    ADD_LIGHT = 44,
    // ADVENTURE_RESET(45)
    ADVENTURE_RESET = 45,
    // ADVENTURE_SAVE(46)
    ADVENTURE_SAVE = 46
}

public static class ToolTypeBody
{
    public static byte GetValue(this ToolType toolType) => (byte)toolType;

    public static ToolType FromValue(byte toolType) => Enum.GetValues(typeof(ToolType)).Cast<ToolType>().FirstOrDefault(type => (byte)type == toolType);
}