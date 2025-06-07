using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Profile 
{
    public class PaintProperties : ISerializable
    {
        public const int BaseAllocationSize = 0x40;
        
        public int TriggerOverride, StickerSaveSize;
        public bool AngleOverride, UiHidden;
        public ResourceDescriptor LastAutoSave;

        public bool UseDefaultBackground;

        public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
        {
            Serializer.Serialize(ref TriggerOverride);
            Serializer.Serialize(ref AngleOverride);
            Serializer.Serialize(ref StickerSaveSize);
            Serializer.Serialize(ref UiHidden);
            Serializer.Serialize(ref LastAutoSave, LastAutoSave, ResourceType.Painting);
            if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() >= (int)Revisions.PTG_USE_DEFAULT_BACKGROUND)
                Serializer.Serialize(ref UseDefaultBackground);
        }

        public int GetAllocatedSize()
        {
            return BaseAllocationSize;
        }
    }
}