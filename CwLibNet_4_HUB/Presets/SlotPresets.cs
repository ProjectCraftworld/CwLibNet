using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Presets;

public class SlotPresets
{
    public static Slot Crater(ResourceDescriptor level, int id)
    {
        var slot = new Slot();
        // SlotID slotId = new SlotID(SlotType.USER_CREATED_STORED_LOCAL, id);

        var enumName = $"SLOT_{id}_LBP1";
        slot.Location = Enum.TryParse(enumName, out Crater craterEnum) ? craterEnum.GetValue() : Vector4.Zero; // Fallback value if ID is invalid

        slot.Root = level;
        return slot;
    }
}