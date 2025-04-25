using static CwLibNet.IO.IValueEnum<int>;

namespace CwLibNet.Enums
{
    public enum CameraType : int
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

    public sealed class CameraBodyMembers
    {
        private readonly CameraType part;

        public CameraBodyMembers(int part)
        {
            this.part = (CameraType)part;
        }

        public int getPart()
        {
            return (int)part;
        }

        public static CameraBodyMembers fromValue(int part)
        {
            if(Enum.IsDefined(typeof(CameraType), part))
            {
                return new CameraBodyMembers(part);
            }
            return default(CameraBodyMembers);
        }
        
    }
}