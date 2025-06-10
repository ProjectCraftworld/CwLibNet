using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.Enums;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.World;

public class CutsceneCameraManager: ISerializable
{
    public const int BaseAllocationSize = 0x14;

    public int State;
    public Thing? CurrentCutSceneCamera;
    public int TimeInCurrentCamera, EndCountdown;
    public float TransitionStage;
    
    public bool CurrentCameraTweaking;
    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref State);
        Serializer.Serialize(ref CurrentCutSceneCamera);
        Serializer.Serialize(ref TimeInCurrentCamera);
        Serializer.Serialize(ref EndCountdown);
        Serializer.Serialize(ref TransitionStage);

        switch (version)
        {
            case > 0x2ef and < 0x36e:
                Serializer.Serialize(ref temp_bool_false);
                ResourceDescriptor? tempNull1 = null;
                Serializer.Serialize(ref tempNull1, ResourceType.Texture, true, true, false);
                Serializer.Serialize(ref temp_int);
                ResourceDescriptor? tempNull2 = null;
                Serializer.Serialize(ref tempNull2, ResourceType.Texture, true, true, false);
                Serializer.Serialize(ref temp_bool_false);
                ResourceDescriptor? tempNull3 = null;
                Serializer.Serialize(ref tempNull3, ResourceType.Texture, true, true, false);
                Serializer.Serialize(ref temp_int);
                ResourceDescriptor? tempNull4 = null;
                Serializer.Serialize(ref tempNull4, ResourceType.Texture, true, true, false);
                break;
            case >= 0x3a0:
                Serializer.Serialize(ref CurrentCameraTweaking);
                break;
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}