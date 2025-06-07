using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Inventory;

public class EyetoyData: ISerializable
{
    public const int BaseAllocationSize = 0x100 + Inventory.ColorCorrection.BaseAllocationSize;

    public ResourceDescriptor? Frame;
    public ResourceDescriptor? AlphaMask;
    public Matrix4x4? ColorCorrection = Matrix4x4.Identity;
    public ColorCorrection ColorCorrectionSrc = new();

    
    public ResourceDescriptor? Outline;

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() < 0x15e)
            return;

        Serializer.Serialize(ref Frame, ResourceType.Texture, true, false, false);
        Serializer.Serialize(ref AlphaMask, ResourceType.Texture, true, false, false);
        Serializer.Serialize(ref ColorCorrection);
        Serializer.Serialize(ref ColorCorrectionSrc);
        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() > 0x39f)
            Serializer.Serialize(ref Outline, ResourceType.Texture, true, false, false);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}