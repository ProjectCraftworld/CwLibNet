using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Inventory;

public class EyetoyData: ISerializable
{
    public const int BaseAllocationSize = 0x100 + Inventory.ColorCorrection.BaseAllocationSize;

    public ResourceDescriptor? Frame;
    public ResourceDescriptor? AlphaMask;
    public Matrix4x4? ColorCorrection = Matrix4x4.Identity;
    public ColorCorrection ColorCorrectionSrc = new();

    
    public ResourceDescriptor? Outline;

    
    public void Serialize()
    {
        if (Serializer.GetRevision().GetVersion() < 0x15e)
            return;

        Serializer.Serialize(ref Frame, Frame, ResourceType.Texture);
        Serializer.Serialize(ref AlphaMask, AlphaMask, ResourceType.Texture);
        Serializer.Serialize(ref ColorCorrection);
        Serializer.Serialize(ref ColorCorrectionSrc);
        if (Serializer.GetRevision().GetVersion() > 0x39f)
            Serializer.Serialize(ref Outline, Outline, ResourceType.Texture);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}