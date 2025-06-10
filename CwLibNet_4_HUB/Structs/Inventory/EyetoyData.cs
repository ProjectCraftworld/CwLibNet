using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Inventory;

public class EyetoyData: ISerializable
{
    public const int BaseAllocationSize = 0x100 + Inventory.ColorCorrection.BaseAllocationSize;

    public ResourceDescriptor? Frame;
    public ResourceDescriptor? AlphaMask;
    public Matrix4x4? ColorCorrection = Matrix4x4.Identity;
    public ColorCorrection ColorCorrectionSrc = new();

    
    public ResourceDescriptor? Outline;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
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