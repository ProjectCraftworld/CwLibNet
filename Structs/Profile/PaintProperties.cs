using CwLibNet.Resources;
using CwLibNet.Singleton;
using CwLibNet.Enums;
using CwLibNet.Types.Data;
using CwLibNet.Structs.Slot;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Profile
{
    public class PaintProperties : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x40;
        public int triggerOverride, stickerSaveSize;
        public bool angleOverride, uiHidden;
        public ResourceDescriptor lastAutoSave;
        public bool useDefaultBackground;
        public override void Serialize(Serializer serializer)
        {
            triggerOverride = serializer.Serialize(triggerOverride);
            stickerSaveSize = serializer.Serialize(stickerSaveSize);
            angleOverride = serializer.Serialize(angleOverride);
            uiHidden = serializer.Serialize(uiHidden);
            lastAutoSave = serializer.Serialize(lastAutoSave, ResourceType.PAINTING, true);
            if (serializer.GetRevision().GetVersion() >= Revisions.PTG_USE_DEFAULT_BACKGROUND)
                useDefaultBackground = serializer.Serialize(useDefaultBackground);
        }

        public virtual int GetAllocatedSize()
        {
            return PaintProperties.BASE_ALLOCATION_SIZE;
        }
    }
}