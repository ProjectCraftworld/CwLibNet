using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.World;

public class CameraSettings: ISerializable
{
    public const int BaseAllocationSize = 0x10;
    public float DefaultZoomMultiplier = 1, MaximumZoomMultiplier = 1, ZoomDelayMultiplier = 1, ZoomSpeedMultiplier = 1;
    public float DefaultPitchMultiplier = 0.254098f, MaximumPitchMultiplier = 0.254098f;
    public void Serialize()
    {
        Serializer.Serialize(ref DefaultZoomMultiplier);
        Serializer.Serialize(ref MaximumZoomMultiplier);
        Serializer.Serialize(ref ZoomDelayMultiplier);
        Serializer.Serialize(ref ZoomSpeedMultiplier);

        var subVersion = Serializer.GetRevision().GetSubVersion();
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