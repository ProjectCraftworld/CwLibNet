using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Inventory;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components.Popit;

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
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref Up);
        Serializer.Serialize(ref Across);
        Serializer.Serialize(ref Texture, ResourceType.Texture, false, true, false);
        Serializer.Serialize(ref Height);
        Serializer.Serialize(ref Width);
        Serializer.Serialize(ref Angle);
        Serializer.Serialize(ref Scale);
        Serializer.Serialize(ref Reversed);
        Serializer.Serialize(ref Offset);
        Serializer.Serialize(ref Stamping);
        if (version > 0x15f)
            Serializer.Serialize(ref Plan, ResourceType.Plan, true, true, false);
        if (version > 0x24b)
            Serializer.Serialize(ref EyetoyData);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}