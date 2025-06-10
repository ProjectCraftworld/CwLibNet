using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Profile 
{
    public class PaintProperties : ISerializable
    {
        public const int BaseAllocationSize = 0x40;
        
        public int TriggerOverride, StickerSaveSize;
        public bool AngleOverride, UiHidden;
        public ResourceDescriptor LastAutoSave;

        public bool UseDefaultBackground;

        public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref TriggerOverride);
            Serializer.Serialize(ref AngleOverride);
            Serializer.Serialize(ref StickerSaveSize);
            Serializer.Serialize(ref UiHidden);
            Serializer.Serialize(ref LastAutoSave, ResourceType.Painting);
            if (serializer.GetRevision().GetVersion() >= (int)Revisions.PTG_USE_DEFAULT_BACKGROUND)
                Serializer.Serialize(ref UseDefaultBackground);
        }

        public int GetAllocatedSize()
        {
            return BaseAllocationSize;
        }
    }
}