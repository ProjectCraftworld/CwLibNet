using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum SlotType
    {
        // DEVELOPER(0)
        DEVELOPER,
        // USER_CREATED_ON_SERVER(1)
        USER_CREATED_ON_SERVER,
        // USER_CREATED_STORED_LOCAL(2)
        USER_CREATED_STORED_LOCAL,
        // LOCAL_GROUP(3)
        LOCAL_GROUP,
        // DEVELOPER_GROUP(4)
        DEVELOPER_GROUP,
        // POD(5)
        POD,
        // FAKE(6)
        FAKE,
        // REMOTE_MOON(7)
        REMOTE_MOON,
        // DLC_LEVEL(8)
        DLC_LEVEL,
        // DLC_PACK(9)
        DLC_PACK,
        // PLAYLIST(10)
        PLAYLIST,
        // DEVELOPER_ADVENTURE(11)
        DEVELOPER_ADVENTURE,
        // DEVELOPER_ADVENTURE_PLANET(12)
        DEVELOPER_ADVENTURE_PLANET,
        // DEVELOPER_ADVENTURE_AREA(13)
        DEVELOPER_ADVENTURE_AREA,
        // USER_ADVENTURE_PLANET_PUBLISHED(14)
        USER_ADVENTURE_PLANET_PUBLISHED,
        // ADVENTURE_PLANET_LOCAL(15)
        ADVENTURE_PLANET_LOCAL,
        // ADVENTURE_LEVEL_LOCAL(16)
        ADVENTURE_LEVEL_LOCAL,
        // ADVENTURE_AREA_LEVEL(17)
        ADVENTURE_AREA_LEVEL 
    }

    public static class SlotTypeExtensions
    {
        public static SlotType FromValue(int value)
        {
            return Enum.GetValues(typeof(SlotType)).Cast<SlotType>().FirstOrDefault(type => (int)type == value);
        }

        public static bool IsGroup(this SlotType value)
        {
            return value is SlotType.DEVELOPER_GROUP or SlotType.LOCAL_GROUP or SlotType.DLC_PACK or SlotType.DEVELOPER_ADVENTURE_AREA or SlotType.ADVENTURE_AREA_LEVEL;
        }

        public static bool IsLink(this SlotType value)
        {
            return value is SlotType.DEVELOPER or SlotType.DLC_LEVEL or SlotType.ADVENTURE_LEVEL_LOCAL or SlotType.DEVELOPER_ADVENTURE;
        }
    }
}