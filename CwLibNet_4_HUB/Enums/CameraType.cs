using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.Enums;

public enum CameraType
{
    // CAMERA_ZONE(0)
    CAMERA_ZONE,
    // PHOTO_BOOTH(1)
    PHOTO_BOOTH,
    // SPEECH_BUBBLE(2)
    SPEECH_BUBBLE,
    // CUTSCENE(3)
    CUTSCENE
}

public sealed class CameraBodyMembers(int part)
{
    private readonly CameraType part = (CameraType)part;

    public int GetPart()
    {
        return (int)part;
    }

    public static CameraBodyMembers? FromValue(int part)
    {
        return Enum.IsDefined(typeof(CameraType), part) ? new CameraBodyMembers(part) : null;
    }
        
}