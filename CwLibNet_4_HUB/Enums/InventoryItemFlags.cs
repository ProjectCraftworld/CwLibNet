using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Profile;
namespace CwLibNet4Hub.Enums;

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
    // this item can be emitted
    public static readonly int COPYRIGHT = 0x2;
    // this item was shared from a community level or
    // not created by the player
    public static readonly int USED = 0x4;
    // indicates that the item has been used at least once
    public static readonly int HIDDEN_ITEM = 0x8;
    public static readonly int RESTRICTED_LEVEL = 0x10;
    // this flag is used for items other than backgrounds,
    // but the most notable use is for limiting a background
    // to be used in levels only
    public static readonly int RESTRICTED_POD = 0x20;
    // this flag is used for items other than backgrounds,
    // but the most notable use is for limiting a background
    // to be used as a pod background only
    public static readonly int DISABLE_LOOP_PREVIEW = 0x40;
    // disables highlight sound from repeating
}