using System;
using System.Collections.Generic;

using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile 
{
    public class PaintProperties : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x40;
        
        public int triggerOverride, stickerSaveSize;
        public bool angleOverride, uiHidden;
        public ResourceDescriptor lastAutoSave;

        public bool useDefaultBackground;

        public void Serialize(Serializer serializer)
        {
            triggerOverride = serializer.I32(triggerOverride);
            angleOverride = serializer.Bool(angleOverride);
            stickerSaveSize = serializer.I32(stickerSaveSize);
            uiHidden = serializer.Bool(uiHidden);
            lastAutoSave = serializer.Resource(lastAutoSave, ResourceType.Painting);
            if (serializer.GetRevision().GetVersion() >= (int)Revisions.PtgUseDefaultBackground)
                useDefaultBackground = serializer.Bool(useDefaultBackground);
        }

        public int GetAllocatedSize()
        {
            return PaintProperties.BASE_ALLOCATION_SIZE;
        }
    }
}