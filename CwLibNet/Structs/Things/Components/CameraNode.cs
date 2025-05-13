using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components;

public class CameraNode: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Vector4? TargetBox;
    public Vector3? PitchAngle;
    public float ZoomDistance;
    public bool LocalSpaceRoll;

    
    public void Serialize(Serializer serializer)
    {
        TargetBox = serializer.V4(TargetBox);
        PitchAngle = serializer.V3(PitchAngle);
        ZoomDistance = serializer.F32(ZoomDistance);
        LocalSpaceRoll = serializer.Bool(LocalSpaceRoll);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}