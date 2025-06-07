using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Inventory;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Components.Popit;

public class StickerInfo: ISerializable
{
    public const int BaseAllocationSize = 0x150;

    public Vector4? Up, Across;
    public ResourceDescriptor? Texture;
    public float Height, Width;
    public float Angle, Scale;
    public bool Reversed;
    public Vector4? Offset;
    public bool Stamping;
    
    public ResourceDescriptor? Plan;

    public EyetoyData EyetoyData = new();
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref Up);
        Serializer.Serialize(ref Across);
        Serializer.Serialize(ref Texture, Texture, ResourceType.Texture);
        Serializer.Serialize(ref Height);
        Serializer.Serialize(ref Width);
        Serializer.Serialize(ref Angle);
        Serializer.Serialize(ref Scale);
        Serializer.Serialize(ref Reversed);
        Serializer.Serialize(ref Offset);
        Serializer.Serialize(ref Stamping);
        if (version > 0x15f)
            Serializer.Serialize(ref Plan, Plan, ResourceType.Plan, true);
        if (version > 0x24b)
            Serializer.Serialize(ref EyetoyData);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}