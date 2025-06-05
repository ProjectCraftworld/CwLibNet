using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Inventory;

public class EyetoyData: ISerializable
{
    public const int BaseAllocationSize = 0x100 + Inventory.ColorCorrection.BaseAllocationSize;

    public ResourceDescriptor? Frame;
    public ResourceDescriptor? AlphaMask;
    public Matrix4x4? ColorCorrection = Matrix4x4.Identity;
    public ColorCorrection ColorCorrectionSrc = new();

    
    public ResourceDescriptor? Outline;

    
    public void Serialize(Serializer serializer)
    {
        if (serializer.GetRevision().GetVersion() < 0x15e)
            return;

        Frame = serializer.Resource(Frame, ResourceType.Texture);
        AlphaMask = serializer.Resource(AlphaMask, ResourceType.Texture);
        ColorCorrection = serializer.M44(ColorCorrection);
        ColorCorrectionSrc = serializer.Struct(ColorCorrectionSrc);
        if (serializer.GetRevision().GetVersion() > 0x39f)
            Outline = serializer.Resource(Outline, ResourceType.Texture);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}