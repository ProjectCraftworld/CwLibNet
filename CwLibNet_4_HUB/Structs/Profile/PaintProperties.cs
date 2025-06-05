using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile 
{
    public class PaintProperties : ISerializable
    {
        public const int BaseAllocationSize = 0x40;
        
        public int TriggerOverride, StickerSaveSize;
        public bool AngleOverride, UiHidden;
        public ResourceDescriptor LastAutoSave;

        public bool UseDefaultBackground;

        public void Serialize(Serializer serializer)
        {
            TriggerOverride = serializer.I32(TriggerOverride);
            AngleOverride = serializer.Bool(AngleOverride);
            StickerSaveSize = serializer.I32(StickerSaveSize);
            UiHidden = serializer.Bool(UiHidden);
            LastAutoSave = serializer.Resource(LastAutoSave, ResourceType.Painting);
            if (serializer.GetRevision().GetVersion() >= (int)Revisions.PTG_USE_DEFAULT_BACKGROUND)
                UseDefaultBackground = serializer.Bool(UseDefaultBackground);
        }

        public int GetAllocatedSize()
        {
            return BaseAllocationSize;
        }
    }
}