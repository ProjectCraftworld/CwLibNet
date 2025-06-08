using System.Numerics;
using CwLibNet.IO;
using CwLibNet.Structs.Things.Components;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        if (version < 0x37e)
        {
            Serializer.Serialize(ref PitchAngle);
            Serializer.Serialize(ref TargetBox);
        }

        Serializer.Serialize(ref TriggerBox);

        if (subVersion >= 0x1b)
        {
            Serializer.Serialize(ref TriggerLayerOffset);
            Serializer.Serialize(ref TriggerLayerDepth);
            if (subVersion >= 0x3d)
                Serializer.Serialize(ref IsCameraZRelative);
        }

        if (version < 0x37e)
            Serializer.Serialize(ref ZoomDistance);

        Serializer.Serialize(ref PositionFactor);

        if (version >= 0x1f5)
            Serializer.Serialize(ref PhotoBoothTimerLength);

        // some 0x13d and 0x176 levels somehow don't serialize this,
        // if a level at that revision crashes,
        // it might be this
        if (EnableImproperLoading && version is 0x13d or 0x176)
        {
            if (!Serializer.IsWriting())
                Serializer.LogMessage("ADD CAMERA TYPE HERE");
        }
        else
        {
            CameraType = Serializer.GetCurrentSerializer().I32(CameraType);
        }


        if (version is > 0x196 and < 0x2c4)
            Serializer.Serialize(ref ActivationLimit);

        switch (version)
        {
            case > 0x1b6 and < 0x1d2:
                Serializer.Serialize(ref temp_int);
                Serializer.Serialize(ref temp_int);
                break;
            case >= 0x1ff:
                Serializer.Serialize(ref DisableZoomMode);
                break;
        }

        if (version >= 0x26a)
            Serializer.Serialize(ref RequireAll);

        if (version >= 0x2ba)
            Serializer.Serialize(ref MotionControllerZone);

        if (version >= 0x2c4)
            Serializer.Serialize(ref Behavior);

        if (version is >= 0x2f8 and < 0x37e)
            Serializer.Serialize(ref temp_int);

        if (version >= 0x2eb)
        {
            Serializer.Serialize(ref CutSceneTransitionType);
            Serializer.Serialize(ref CutSceneHoldTime);
        }

        if (version >= 0x2eb)
            Serializer.Serialize(ref CutSceneSkippable);

        if (Serializer.GetCurrentSerializer().GetRevision().GetSubVersion() >= 0x9f)
            Serializer.Serialize(ref CutSceneUseHoldTime);

        if (version is > 0x2ea and < 0x2f1)
        {
            Serializer.SerializeReference<Thing>(null);
            Serializer.Serialize(ref temp_int);
        }

        if (version > 0x2ed)
        {
            Serializer.Serialize(ref CutSceneTimeSinceUsed);
            Serializer.Serialize(ref CutSceneTransitionTime);
            if (version < 0x35a)
                Serializer.Serialize(ref temp_bool_false);
        }

        if (version > 0x359)
            Serializer.Serialize(ref CutSceneColour);

        if (version > 0x2ed)
            Serializer.Serialize(ref CutSceneMovieMode);

        if (version > 0x2f7)
        {
            Serializer.Serialize(ref CutSceneDepthOfField);
            Serializer.Serialize(ref CutSceneFog);
        }

        if (version > 0x2f8)
            Serializer.Serialize(ref CutSceneFov);

        if (version > 0x315)
            Serializer.Serialize(ref CutSceneShake);

        if (version > 0x318)
            Serializer.Serialize(ref FadeAudio);
        if (version > 0x33e)
            Serializer.Serialize(ref OldStyleCameraZone);

        if (version > 0x369)
            Serializer.Serialize(ref CutSceneTrackPlayer);
        if (version > 0x395)
            Serializer.Serialize(ref CutSceneSendsSignalOnCancelled);
        if (version > 0x396)
            Serializer.Serialize(ref CutSceneWasActiveLastFrame);

        if (version > 0x37d)
        {
            Serializer.Serialize(ref Nodes);

            // Fill in the LBP1 data with the first node if it exists
            if (!Serializer.IsWriting() && Nodes is { Length: > 0 })
            {
                var node = Nodes[0];
                PitchAngle = node.PitchAngle;
                TargetBox = node.TargetBox;
                ZoomDistance = node.ZoomDistance;
            }
        }
        // Cache an LBP2 node based on this data
        else if (!Serializer.IsWriting())
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
            Serializer.Serialize(ref FrontDof);
        if (subVersion > 0x79)
            Serializer.Serialize(ref SackTrackDof);
        if (subVersion > 0x7f)
            Serializer.Serialize(ref AllowSmoothZTransition);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}