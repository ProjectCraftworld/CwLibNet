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
    }

        public sealed class ContentsBodyMembers
    {
        private readonly ContentsType type;

        public ContentsBodyMembers(int type)
        {
            this.type = (ContentsType)type;
        }

        public ContentsType getType()
        {
            return this.type;
        }

        public static ContentsBodyMembers fromValue(int type)
        {
            if (Enum.IsDefined(typeof(BuiltinType), type))
        {
            return new ContentsBodyMembers(type);
        }
            return default(ContentsBodyMembers);
        }
    }
}