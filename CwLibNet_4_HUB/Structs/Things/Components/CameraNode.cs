using System.Numerics;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components;

public class CameraNode: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public Vector4? TargetBox;
    public Vector3? PitchAngle;
    public float ZoomDistance;
    public bool LocalSpaceRoll;

    
    public void Serialize()
    {
        Serializer.Serialize(ref TargetBox);
        PitchAngle = Serializer.Serialize(ref PitchAngle);
        Serializer.Serialize(ref ZoomDistance);
        Serializer.Serialize(ref LocalSpaceRoll);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}