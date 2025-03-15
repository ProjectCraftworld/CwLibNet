using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Types.Things.Components.World;

public class CameraSettings: ISerializable
{
    public const int BaseAllocationSize = 0x10;
    public float DefaultZoomMultiplier = 1, MaximumZoomMultiplier = 1, ZoomDelayMultiplier = 1, ZoomSpeedMultiplier = 1;
    public float DefaultPitchMultiplier = 0.254098f, MaximumPitchMultiplier = 0.254098f;
    public void Serialize(Serializer serializer)
    {
        DefaultZoomMultiplier = serializer.F32(DefaultZoomMultiplier);
        MaximumZoomMultiplier = serializer.F32(MaximumZoomMultiplier);
        ZoomDelayMultiplier = serializer.F32(ZoomDelayMultiplier);
        ZoomSpeedMultiplier = serializer.F32(ZoomSpeedMultiplier);

        int subVersion = serializer.GetRevision().GetSubVersion();
        if (subVersion > 0x36) {
            DefaultPitchMultiplier = serializer.F32(DefaultPitchMultiplier);
            MaximumPitchMultiplier = serializer.F32(MaximumPitchMultiplier);
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}