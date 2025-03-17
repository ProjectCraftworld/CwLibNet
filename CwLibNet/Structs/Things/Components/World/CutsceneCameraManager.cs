using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Things;

namespace CwLibNet.Structs.Things.Components.World;

public class CutsceneCameraManager: ISerializable
{
    public const int BaseAllocationSize = 0x14;

    public int State;
    public Thing? CurrentCutSceneCamera;
    public int TimeInCurrentCamera, EndCountdown;
    public float TransitionStage;
    
    public bool CurrentCameraTweaking;
    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        State = serializer.I32(State);
        CurrentCutSceneCamera = serializer.Reference(CurrentCutSceneCamera);
        TimeInCurrentCamera = serializer.S32(TimeInCurrentCamera);
        EndCountdown = serializer.S32(EndCountdown);
        TransitionStage = serializer.F32(TransitionStage);

        if (0x2ef < version && version < 0x36e) {
            serializer.Bool(false);
            serializer.V3(null);
            serializer.F32(0);
            serializer.V3(null);
            serializer.Bool(false);
            serializer.V3(null);
            serializer.F32(0);
            serializer.V3(null);
        }

        if (version >= 0x3a0)
            CurrentCameraTweaking = serializer.Bool(CurrentCameraTweaking);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}