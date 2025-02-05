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

        // --------------------
        // TODO enum body members
        // private final int value;
        // SlotType(int value) {
        //     this.value = value;
        // }
        // public Integer getValue() {
        //     return this.value;
        // }
        // /**
        //  * Attempts to get a SlotType from value.
        //  *
        //  * @param value Slot type value
        //  * @return SlotType
        //  */
        // public static SlotType fromValue(int value) {
        //     for (SlotType type : SlotType.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return SlotType.DEVELOPER;
        // }
        // public boolean isGroup() {
        //     return this.equals(SlotType.DEVELOPER_GROUP) || this.equals(SlotType.LOCAL_GROUP) || this.equals(SlotType.DLC_PACK) || this.equals(SlotType.DEVELOPER_ADVENTURE_AREA) || this.equals(SlotType.ADVENTURE_AREA_LEVEL);
        // }
        // public boolean isLink() {
        //     return this.equals(SlotType.DEVELOPER) || this.equals(SlotType.DLC_LEVEL) || this.equals(SlotType.ADVENTURE_LEVEL_LOCAL) || this.equals(SlotType.DEVELOPER_ADVENTURE);
        // }
        // --------------------
    }
}