using CwLibNet.IO;
using CwLibNet.Types.Data;

namespace CwLibNet.Enums
{
    public enum ContentsType
    {
        // GROUP(0, CommonMeshes.POLAROID_GUID)
        GROUP,
        // PLANS(1, CommonMeshes.BUBBLE_GUID)
        PLANS,
        // LEVEL(2, CommonMeshes.LEVEL_BADGE_GUID)
        LEVEL,
        // COSTUME(3, CommonMeshes.BUBBLE_GUID)
        COSTUME,
        // ADVENTURE(5, CommonMeshes.ADVENTURE_BADGE_GUID)
        ADVENTURE 

        // --------------------
        // TODO enum body members
        // private final int value;
        // /**
        //  * The default associated mesh associated
        //  * with this content type.
        //  */
        // private final GUID badgeMeshGUID;
        // ContentsType(int value, GUID mesh) {
        //     this.value = value;
        //     this.badgeMeshGUID = mesh;
        // }
        // public Integer getValue() {
        //     return this.value;
        // }
        // public ResourceDescriptor getBadgeMesh() {
        //     return new ResourceDescriptor(this.badgeMeshGUID, ResourceType.MESH);
        // }
        // public static ContentsType fromValue(int value) {
        //     for (ContentsType type : ContentsType.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return ContentsType.GROUP;
        // }
        // --------------------
    }
}