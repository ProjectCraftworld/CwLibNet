using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Presets;

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