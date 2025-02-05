namespace CwLibNet.Enums
{
    public class InventoryItemFlags
    {
        public static readonly int NONE = 0x0;
        public static readonly int HEARTED = 0x1;
        public static readonly int UPLOADED = 0x2;
        public static readonly int CHEAT = 0x4;
        public static readonly int UNSAVED = 0x8;
        public static readonly int ERRORED = 0x10;
        public static readonly int HIDDEN_PLAN = 0x20;
        public static readonly int AUTOSAVED = 0x40;
        public static readonly int ALLOW_EMIT = 0x1;
        public static readonly int COPYRIGHT = 0x2;
        public static readonly int USED = 0x4;
        public static readonly int HIDDEN_ITEM = 0x8;
        public static readonly int RESTRICTED_LEVEL = 0x10;
        public static readonly int RESTRICTED_POD = 0x20;
        public static readonly int DISABLE_LOOP_PREVIEW = 0x40;
    }
}