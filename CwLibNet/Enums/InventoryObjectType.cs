using CwLibNet.Types.Data;

namespace CwLibNet.Enums;

public enum InventoryObjectType
{
    // NONE(0)
    NONE = 0,
    // PRIMITIVE_MATERIAL(1 << 0)
    PRIMITIVE_MATERIAL = 1 << 0,
    // READYMADE(1 << 1)
    READYMADE = 1 << 1,
    // DECORATION(1 << 2)
    DECORATION = 1 << 2,
    // STICKER(1 << 3)
    STICKER = 1 << 3,
    // COSTUME(1 << 4)
    COSTUME = 1 << 4,
    // COSTUME_MATERIAL(1 << 5)
    COSTUME_MATERIAL = 1 << 5,
    // JOINT(1 << 6)
    JOINT = 1 << 6,
    // USER_OBJECT(1 << 7)
    USER_OBJECT = 1 << 7,
    // BACKGROUND(1 << 8)
    BACKGROUND = 1 << 8,
    // GAMEPLAY_KIT(1 << 9)
    GAMEPLAY_KIT = 1 << 9,
    // USER_STICKER(1 << 10)
    USER_STICKER = 1 << 10,
    // PRIMITIVE_SHAPE(1 << 11)
    PRIMITIVE_SHAPE = 1 << 11,
    // SEQUENCER(1 << 12, GameVersion.LBP2 | GameVersion.LBP3)
    SEQUENCER = 1 << 12,
    // DANGER(1 << 13)
    DANGER = 1 << 13,
    // EYETOY(1 << 14)
    EYETOY = 1 << 14,
    // GADGET(1 << 15)
    GADGET = 1 << 15,
    // TOOL(1 << 16)
    TOOL = 1 << 16,
    // SACKBOT_MESH(1 << 17, GameVersion.LBP3)
    SACKBOT_MESH = 1 << 17,
    // CREATURES_CHARACTERS(1 << 18, GameVersion.LBP3)
    CREATURES_CHARACTERS = 1 << 18,
    // PLAYER_COLOUR(1 << 19)
    PLAYER_COLOUR = 1 << 19,
    // USER_COSTUME(1 << 20)
    USER_COSTUME = 1 << 20,
    // MUSIC(1 << 21)
    MUSIC = 1 << 21,
    // SOUND(1 << 22)
    SOUND = 1 << 22,
    // PHOTOBOOTH(1 << 23)
    PHOTOBOOTH = 1 << 23,
    // USER_PLANET(1 << 24, GameVersion.LBP2 | GameVersion.LBP3)
    USER_PLANET = 1 << 24,
    // LEVEL_KEY(1 << 25, GameVersion.LBP2 | GameVersion.LBP3)
    LEVEL_KEY = 1 << 25,
    // EMITTED_ITEM(1 << 26, GameVersion.LBP2 | GameVersion.LBP3)
    EMITTED_ITEM = 1 << 26,
    // GUN_ITEM(1 << 27, GameVersion.LBP2 | GameVersion.LBP3)
    GUN_ITEM = 1 << 27,
    // NPC_COSTUME(1 << 28, GameVersion.LBP2 | GameVersion.LBP3)
    NPC_COSTUME = 1 << 28,
    // INSTRUMENT(1 << 29, GameVersion.LBP2 | GameVersion.LBP3)
    INSTRUMENT = 1 << 29,
    // USER_POD(1 << 30, GameVersion.LBP2 | GameVersion.LBP3)
    USER_POD = 1 << 30,
    // COSTUME_TWEAKER_TOOL(1 << 31, GameVersion.LBP3)
    COSTUME_TWEAKER_TOOL = 1 << 31,
    // /* These are all exclusive to LBP1, and are replaced with
    //        different types in LBP2 onward. */
    // PAINT(1 << 12, GameVersion.LBP1)
    PAINT = 1 << 12,
    // FLOOD_FILL(1 << 17, GameVersion.LBP1)
    FLOOD_FILL = 1 << 17,
    // STICKER_TOOL(1 << 18, GameVersion.LBP1)
    STICKER_TOOL = 1 << 18,
    // COSTUME_TOOL(1 << 24, GameVersion.LBP1)
    COSTUME_TOOL = 1 << 24,
    // PLAN_TOOL(1 << 25, GameVersion.LBP1)
    PLAN_TOOL = 1 << 25,
    // PHOTO_TOOL(1 << 26, GameVersion.LBP1)
    PHOTO_TOOL = 1 << 26,
    // PICTURE_TOOLS(1 << 27, GameVersion.LBP1)
    PICTURE_TOOLS = 1 << 27,
    // COM_PHOTO_TOOLS(1 << 28, GameVersion.LBP1)
    COM_PHOTO_TOOLS = 1 << 28,
    // COM_OBJECT_TOOLS(1 << 29, GameVersion.LBP1)
    COM_OBJECT_TOOLS = 1 << 29,
    // USER_POD_LBP1(1 << 30, GameVersion.LBP1)
    USER_POD_LBP1 = 1 << 30,
    // POD_TOOL_LBP1(1 << 31, GameVersion.LBP1)
    POD_TOOL_LBP1 = 1 << 31,
    // /* These are exclusive to LBP2 */
    // EDIT_MODE_TOOL(1 << 17, GameVersion.LBP2)
    EDIT_MODE_TOOL = 1 << 17,
    // POD_TOOL_LBP2(1 << 18, GameVersion.LBP2)
    POD_TOOL_LBP2 = 1 << 18,
    // EARTH_TOOL(1 << 31, GameVersion.LBP2)
    EARTH_TOOL = 1 << 31,
}
    
public static class InvObjectExtensions {
    public static int GetFlags(this HashSet<InventoryObjectType> set)
    {
        return set.Aggregate(0, (current, type) => current | (int)type);
    }
}

public sealed class InvObjectBody(InventoryObjectType value, int flags)
{
        
    public static HashSet<InventoryObjectType> FromFlags(int flags, Revision revision)
    {
        var set = new HashSet<InventoryObjectType>();
        foreach (InventoryObjectType type in Enum.GetValues(typeof(InventoryObjectType)))
        {
            if ((flags & (int)type) != 0)
                set.Add(type);
        }
        return set;
    }

    private readonly int flags = flags;

    public InvObjectBody(InventoryObjectType value) : this(value, GameVersion.LBP1 | GameVersion.LBP2 | GameVersion.LBP3)
    {
    }


    public static bool Has(int flags, InventoryObjectType type)
    {
        return (flags & (int)type) != 0;
    }

    public bool Has(int flags)
    {
        return (flags & (int)value) != 0;
    }

    public static string GetPrimaryName(HashSet<InventoryObjectType>? set)
    {
        if (set == null || set.Count == 0)
            return "none";
        return set.First().ToString().ToLower();
    }
}