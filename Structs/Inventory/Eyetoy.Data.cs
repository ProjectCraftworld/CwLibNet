using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Inventory
{
    public class EyetoyData : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x100 + ColorCorrection.BASE_ALLOCATION_SIZE;
        public ResourceDescriptor frame, alphaMask;
        public Matrix4f colorCorrection = new Matrix4f().Identity();
        public ColorCorrection colorCorrectionSrc = new ColorCorrection();
        public ResourceDescriptor outline;
        public override void Serialize(Serializer serializer)
        {
            if (serializer.GetRevision().GetVersion() < 0x15e)
                return;
            frame = serializer.Serilialize(frame, ResourceType.TEXTURE);
            alphaMask = serializer.Serialize(alphaMask, ResourceType.TEXTURE);
            colorCorrection = serializer.Serialize(colorCorrection);
            colorCorrectionSrc = serializer.Serialize(colorCorrectionSrc, typeof(ColorCorrection));
            if (serializer.GetRevision().GetVersion() > 0x39f)
                outline = serializer.Serialize(outline, ResourceType.TEXTURE);
        }

        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}