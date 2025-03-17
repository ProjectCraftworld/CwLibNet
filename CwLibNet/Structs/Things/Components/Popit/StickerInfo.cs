using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Inventory;
using CwLibNet.Types.Data;

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

    public EyetoyData EyetoyData = new EyetoyData();
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        Up = serializer.V4(Up);
        Across = serializer.V4(Across);
        Texture = serializer.Resource(Texture, ResourceType.Texture);
        Height = serializer.F32(Height);
        Width = serializer.F32(Width);
        Angle = serializer.F32(Angle);
        Scale = serializer.F32(Scale);
        Reversed = serializer.Bool(Reversed);
        Offset = serializer.V4(Offset);
        Stamping = serializer.Bool(Stamping);
        if (version > 0x15f)
            Plan = serializer.Resource(Plan, ResourceType.Plan, true);
        if (version > 0x24b)
            EyetoyData = serializer.Struct(EyetoyData);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}