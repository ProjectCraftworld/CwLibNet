using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.World;

public class CutsceneCameraManager: ISerializable
{
    public const int BaseAllocationSize = 0x14;

    public int State;
    public Thing? CurrentCutSceneCamera;
    public int TimeInCurrentCamera, EndCountdown;
    public float TransitionStage;
    
    public bool CurrentCameraTweaking;
    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref State);
        Serializer.Serialize(ref CurrentCutSceneCamera);
        Serializer.Serialize(ref TimeInCurrentCamera);
        Serializer.Serialize(ref EndCountdown);
        Serializer.Serialize(ref TransitionStage);

        switch (version)
        {
            case > 0x2ef and < 0x36e:
                Serializer.Serialize(ref false);
                Serializer.Serialize(ref null);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref null);
                Serializer.Serialize(ref false);
                Serializer.Serialize(ref null);
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref null);
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