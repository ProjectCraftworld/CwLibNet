using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components;

public class CameraNode: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Vector4? TargetBox;
    public Vector3? PitchAngle;
    public float ZoomDistance;
    public bool LocalSpaceRoll;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref TargetBox);
        Serializer.Serialize(ref PitchAngle);
        Serializer.Serialize(ref ZoomDistance);
        Serializer.Serialize(ref LocalSpaceRoll);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}