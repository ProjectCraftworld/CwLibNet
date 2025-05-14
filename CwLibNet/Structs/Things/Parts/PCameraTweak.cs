using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components;

namespace CwLibNet.Structs.Things.Parts;

public class PCameraTweak: ISerializable
{
    public const int BaseAllocationSize = 0x80;
    public static bool EnableImproperLoading = false;

    
    
    public Vector3? PitchAngle;
    
    
    public Vector4? TargetBox;

    public Vector4? TriggerBox;

    
    public byte TriggerLayerOffset, TriggerLayerDepth;
    
    public bool IsCameraZRelative;

    
    
    public float ZoomDistance = 1000.0f;

    public float PositionFactor;

    
    public int PhotoBoothTimerLength;
    public int CameraType;

    
    
    public float ActivationLimit = 1.0f;

    
    public bool DisableZoomMode;

    
    public bool RequireAll = true;

    
    public bool MotionControllerZone;

    
    public int Behavior;

    
    public byte CutSceneTransitionType;
    
    public int CutSceneHoldTime;

    
    public bool CutSceneSkippable;

    
    public bool CutSceneUseHoldTime;

    
    public byte CutSceneTimeSinceUsed;
    
    public int CutSceneTransitionTime;

    
    public int CutSceneColour;

    
    public bool CutSceneMovieMode;


    
    public float CutSceneDepthOfField, CutSceneFog;

    
    public float CutSceneFov;
    
    public float CutSceneShake;

    
    public bool FadeAudio;
    
    public bool OldStyleCameraZone;

    
    public bool CutSceneTrackPlayer;

    
    public bool CutSceneSendsSignalOnCancelled;

    
    public bool CutSceneWasActiveLastFrame;

    
    public CameraNode[]? Nodes;

    
    public float FrontDof;

    
    public float SackTrackDof;

    
    public bool AllowSmoothZTransition;

    
    public void Serialize(Serializer serializer)
    {
        var version = serializer.GetRevision().GetVersion();
        var subVersion = serializer.GetRevision().GetSubVersion();

        if (version < 0x37e)
        {
            PitchAngle = serializer.V3(PitchAngle);
            TargetBox = serializer.V4(TargetBox);
        }

        TriggerBox = serializer.V4(TriggerBox);

        if (subVersion >= 0x1b)
        {
            TriggerLayerOffset = serializer.I8(TriggerLayerOffset);
            TriggerLayerDepth = serializer.I8(TriggerLayerDepth);
            if (subVersion >= 0x3d)
                IsCameraZRelative = serializer.Bool(IsCameraZRelative);
        }

        if (version < 0x37e)
            ZoomDistance = serializer.F32(ZoomDistance);

        PositionFactor = serializer.F32(PositionFactor);

        if (version >= 0x1f5)
            PhotoBoothTimerLength = serializer.I32(PhotoBoothTimerLength);

        // some 0x13d and 0x176 levels somehow don't serialize this,
        // if a level at that revision crashes,
        // it might be this
        if (EnableImproperLoading && version is 0x13d or 0x176)
        {
            if (!serializer.IsWriting())
                serializer.Log("ADD CAMERA TYPE HERE", 1);
        }
        else
        {
            CameraType = version < 0x1d7 ? serializer.U8(CameraType) : serializer.S32(CameraType);
        }


        if (version is > 0x196 and < 0x2c4)
            ActivationLimit = serializer.F32(ActivationLimit);

        switch (version)
        {
            case > 0x1b6 and < 0x1d2:
                serializer.F32(0);
                serializer.F32(0);
                break;
            case >= 0x1ff:
                DisableZoomMode = serializer.Bool(DisableZoomMode);
                break;
        }

        if (version >= 0x26a)
            RequireAll = serializer.Bool(RequireAll);

        if (version >= 0x2ba)
            MotionControllerZone = serializer.Bool(MotionControllerZone);

        if (version >= 0x2c4)
            Behavior = serializer.I32(Behavior);

        if (version is >= 0x2f8 and < 0x37e)
            serializer.U8(0);

        if (version >= 0x2eb)
        {
            CutSceneTransitionType = serializer.I8(CutSceneTransitionType);
            CutSceneHoldTime = serializer.I32(CutSceneHoldTime);
        }

        if (version >= 0x2eb)
            CutSceneSkippable = serializer.Bool(CutSceneSkippable);

        if (serializer.GetRevision().GetSubVersion() >= 0x9f)
            CutSceneUseHoldTime = serializer.Bool(CutSceneUseHoldTime);

        if (version is > 0x2ea and < 0x2f1)
        {
            serializer.Wstr(null);
            serializer.S32(0);
        }

        if (version > 0x2ed)
        {
            CutSceneTimeSinceUsed = serializer.I8(CutSceneTimeSinceUsed);
            CutSceneTransitionTime = serializer.I32(CutSceneTransitionTime);
            if (version < 0x35a)
                serializer.Bool(false);
        }

        if (version > 0x359)
            CutSceneColour = serializer.I32(CutSceneColour);

        if (version > 0x2ed)
            CutSceneMovieMode = serializer.Bool(CutSceneMovieMode);

        if (version > 0x2f7)
        {
            CutSceneDepthOfField = serializer.F32(CutSceneDepthOfField);
            CutSceneFog = serializer.F32(CutSceneFog);
        }

        if (version > 0x2f8)
            CutSceneFov = serializer.F32(CutSceneFov);

        if (version > 0x315)
            CutSceneShake = serializer.F32(CutSceneShake);

        if (version > 0x318)
            FadeAudio = serializer.Bool(FadeAudio);
        if (version > 0x33e)
            OldStyleCameraZone = serializer.Bool(OldStyleCameraZone);

        if (version > 0x369)
            CutSceneTrackPlayer = serializer.Bool(CutSceneTrackPlayer);
        if (version > 0x395)
            CutSceneSendsSignalOnCancelled =
                serializer.Bool(CutSceneSendsSignalOnCancelled);
        if (version > 0x396)
            CutSceneWasActiveLastFrame = serializer.Bool(CutSceneWasActiveLastFrame);

        if (version > 0x37d)
        {
            Nodes = serializer.Array(Nodes);

            // Fill in the LBP1 data with the first node if it exists
            if (!serializer.IsWriting() && Nodes is { Length: > 0 })
            {
                var node = Nodes[0];
                PitchAngle = node.PitchAngle;
                TargetBox = node.TargetBox;
                ZoomDistance = node.ZoomDistance;
            }
        }
        // Cache an LBP2 node based on this data
        else if (!serializer.IsWriting())
        {
            Nodes = new CameraNode[1];
            var node = new CameraNode
            {
                PitchAngle = PitchAngle,
                TargetBox = TargetBox,
                ZoomDistance = ZoomDistance
            };
            Nodes[0] = node;
        }

        if (subVersion > 0x7d)
            FrontDof = serializer.F32(FrontDof);
        if (subVersion > 0x79)
            SackTrackDof = serializer.F32(SackTrackDof);
        if (subVersion > 0x7f)
            AllowSmoothZTransition = serializer.Bool(AllowSmoothZTransition);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}