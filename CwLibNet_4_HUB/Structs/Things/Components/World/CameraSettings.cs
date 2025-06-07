using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Components.World;

public class CameraSettings: ISerializable
{
    public const int BaseAllocationSize = 0x10;
    public float DefaultZoomMultiplier = 1, MaximumZoomMultiplier = 1, ZoomDelayMultiplier = 1, ZoomSpeedMultiplier = 1;
    public float DefaultPitchMultiplier = 0.254098f, MaximumPitchMultiplier = 0.254098f;
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref DefaultZoomMultiplier);
        Serializer.Serialize(ref MaximumZoomMultiplier);
        Serializer.Serialize(ref ZoomDelayMultiplier);
        Serializer.Serialize(ref ZoomSpeedMultiplier);

        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();
        if (subVersion > 0x36) {
            Serializer.Serialize(ref DefaultPitchMultiplier);
            Serializer.Serialize(ref MaximumPitchMultiplier);
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}