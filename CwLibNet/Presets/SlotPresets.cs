using System;
using System.Numerics;
using CwLibNet.Enums;
using static CwLibNet.Enums.Crater;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Presets
{
    public class SlotPresets
    {
        public static Slot Crater(ResourceDescriptor level, int ID)
        {
            Slot slot = new Slot();
            SlotID slotID = new SlotID(SlotType.USER_CREATED_STORED_LOCAL, ID);

            string enumName = $"SLOT_{ID}_LBP1";
            if (Enum.TryParse(enumName, out Crater craterEnum))
            {
                slot.location = craterEnum.GetValue();
            }
            else
            {
                slot.location = Vector4.Zero; // Fallback value if ID is invalid
            }

            slot.root = level;
            return slot;
        }
    }
}