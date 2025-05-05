using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Profile;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Structs.Things.Components.Switches;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Things.Parts;

public class PSwitch: ISerializable
{
     public static readonly int BASE_ALLOCATION_SIZE = 0x200;

    public bool Inverted;
    public float Radius;
    
    public float MinRadius;
    public int ColorIndex;
    
    public string? Name;
    
    public bool CrappyOldLbp1Switch;
    public int BehaviorOld;
    public SwitchOutput[]? Outputs;

    
    public ResourceDescriptor? StickerPlan;
    
    
    public GlobalThingDescriptor? RefSticker;

    
    public bool HideInPlayMode;
    
    public SwitchType Type = SwitchType.INVALID;
    
    public Thing? ReferenceThing;
    
    public SwitchSignal? ManualActivation;
    
    public float PlatformVisualFactor;
    
    public float OldActivation;
    
    public int ActivationHoldTime;
    
    public bool RequireAll;
    
    public Vector4[]? ConnectorPos;
    
    public bool[]? ConnectorGrabbed;
    
    public Vector4? PortPosOffset;
    public Vector4? LooseConnectorPos;
    public Vector4? LooseConnectorBaseOffset;

    public bool LooseConnectorGrabbed;
    
    public float AngleRange;
    
    public int IncludeTouching;
    
    public int BulletsRequired;
    
    public int BulletsDetected;
    
    public int BulletPlayerNumber;
    
    public int BulletRefreshTime;
    
    public bool ResetWhenFull;
    
    public bool HideConnectors;
    
    public SwitchLogicType LogicType = SwitchLogicType.AND;
    
    public int UpdateFrame;
    
    
    public Thing[]? InputList;
    
    public Thing? PortThing;
    
    public bool IncludeRigidConnectors;
    
    public Vector4? CustomPortOffset;
    public Vector4? CustomConnectorOffset;

    public float TimerCount;
    
    public byte TimerAutoCount;

    // 
    
    public int TeamFilter;

    
    public SwitchBehavior Behavior = SwitchBehavior.OFF_ON;
    
    public int RandomBehavior, RandomPattern;
    
    public int RandomOnTimeMin, RandomOnTimeMax;
    
    public int RandomOffTimeMin, RandomOffTimeMax;
    
    public int RandomPhaseOn, RandomPhaseTime;
    
    public bool RetardedOldJoint;
    
    public int KeySensorMode;
    
    public int UserDefinedColour;
    
    public bool WiresVisible;
    
    public byte BulletTypes;
    
    public bool DetectUnspawnedPlayers;
    
    public byte UnspawnedBehavior;
    
    public bool PlaySwitchAudio;
    
    public byte PlayerMode;
    
    
    public DataLabelValue Value = new();
    
    public bool RelativeToSequencer;
    
    public byte LayerRange;
    
    public bool BreakSound;
    
    public int ColorTimer;
    
    public bool IsLbp3Switch;
    
    public bool RandomNonRepeating;
    
    public int StickerSwitchMode;

    /* Vita */
    
    public byte ImpactSensorMode;
    
    public int SwitchTouchType;
    
    public byte CursorScreenArea;
    
    public byte CursorInteractionType;
    
    public byte CursorTouchPanels;
    
    public byte CursorTouchIndex;
    
    public byte Flags;
    
    public int OutputAndOr;
    
    public byte IncludeSameChipTags;
    
    public int GlowFrontCol, GlowBackCol, GlowActiveCol;
    
    public byte PlayerFilter;

    
    public void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (version < 0x1a5)
        {
            serializer.Reference<Thing>(null);
            serializer.Bool(false);
        }

        Inverted = serializer.Bool(Inverted);
        Radius = serializer.F32(Radius);

        if (version >= 0x382) MinRadius = serializer.F32(MinRadius);

        ColorIndex = serializer.S32(ColorIndex);

        if (version >= 0x2dc)
            Name = serializer.Wstr(Name);

        if (version >= 0x38f)
            CrappyOldLbp1Switch = serializer.Bool(CrappyOldLbp1Switch);
        if (!serializer.IsWriting() && version < 0x309)
            CrappyOldLbp1Switch = true;

        BehaviorOld = serializer.S32(BehaviorOld);

        if (version < 0x329)
        {
            // I would just use SwitchOutput, but ternary
            // doesn't exist in this struct, despite SwitchSignal having
            // it added in 0x310
            if (serializer.IsWriting())
            {
                var stream = serializer.GetOutput();
                var output = Outputs != null && Outputs.Length != 0 ?
                    Outputs[0] : new SwitchOutput();
                serializer.F32(output.Activation.Activation);
                if (version > 0x2a2) stream.I32(output.Activation.Player);
                serializer.Array(output.TargetList);
            }
            else
            {
                var stream = serializer.GetInput();
                var output = new SwitchOutput
                {
                    Activation =
                    {
                        Activation = stream.F32()
                    }
                };
                if (version > 0x2a2)
                    output.Activation.Player = serializer.I32(output.Activation.Player);
                output.TargetList = serializer.Array<SwitchTarget>(null);
                Outputs = [output];
            }
        }
        else Outputs = serializer.Array(Outputs, true);

        if (version is < 0x398 and >= 0x140)
        {
            if (version < 0x160)
                RefSticker = serializer.Struct(RefSticker);
            else
                StickerPlan = serializer.Resource(StickerPlan, ResourceType.Plan, true,
                    false, false);
        }

        if (version is > 0x13f and < 0x1a5) serializer.S32(0);

        if (version > 0x197) HideInPlayMode = serializer.Bool(HideInPlayMode);
        if (version > 0x1a4)
        {
            Type = serializer.Enum32(Type, true);
            ReferenceThing = serializer.Reference(ReferenceThing);
            ManualActivation = serializer.Struct(ManualActivation);
        }

        switch (version)
        {
            case >= 0x398 when Type == SwitchType.STICKER || (Type == SwitchType.POCKET_ITEM && subVersion > 0x10):
                StickerPlan = serializer.Resource(StickerPlan, ResourceType.Plan, true, false,
                    false);
                break;
            case > 0x1a4 and < 0x368:
                PlatformVisualFactor = serializer.F32(PlatformVisualFactor);
                break;
        }

        if (version is > 0x1a4 and < 0x2a0) OldActivation = serializer.F32(OldActivation);

        if (version > 0x1a4) ActivationHoldTime = serializer.S32(ActivationHoldTime);
        if (version > 0x1a4) RequireAll = serializer.Bool(RequireAll);

        if (revision.Has(Branch.Double11, 0x2a))
            ImpactSensorMode = serializer.I8(ImpactSensorMode);

        if (version is > 0x1fa and < 0x327)
        {
            ConnectorPos = serializer.Vectorarray(ConnectorPos);

            // connectorGrabbed
            if (!serializer.IsWriting())
                ConnectorGrabbed = new bool[serializer.GetInput().I32()];
            else
            {
                ConnectorGrabbed ??= [];
                serializer.GetOutput().I32(ConnectorGrabbed.Length);
            }
            for (var i = 0; i < ConnectorGrabbed.Length; ++i)
                ConnectorGrabbed[i] = serializer.Bool(ConnectorGrabbed[i]);

            PortPosOffset = serializer.V4(PortPosOffset);
            LooseConnectorPos = serializer.V4(LooseConnectorPos);
            LooseConnectorBaseOffset = serializer.V4(LooseConnectorBaseOffset);
            LooseConnectorGrabbed = serializer.Bool(LooseConnectorGrabbed);
        }

        if (version > 0x23d)
        {
            AngleRange = serializer.F32(AngleRange);
            IncludeTouching = serializer.S32(IncludeTouching);
            if (version >= 0x398 && Type == SwitchType.MICROCHIP && IncludeTouching == 1)
                StickerPlan = serializer.Resource(StickerPlan, ResourceType.Plan, true,
                    false, false);
        }

        if (subVersion > 0x165 && Type == SwitchType.GAME_LIVE_STREAMING_CHOICE)
        {
            throw new SerializationException("UNSUPPORTED!");
            // Wstr[4], Wstr[5] if subVersion >= 0x187?
        }

        if (version > 0x243) BulletsRequired = serializer.S32(BulletsRequired);
        switch (version)
        {
            case 0x244:
                serializer.I32(0); // ???
                break;
            case > 0x244:
                BulletsDetected = serializer.S32(BulletsDetected);
                break;
        }

        if (version is > 0x245 and < 0x398)
            BulletPlayerNumber = serializer.I32(BulletPlayerNumber);
        if (version > 0x248)
            BulletRefreshTime = serializer.I32(BulletRefreshTime);

        if (version >= 0x2f5) ResetWhenFull = serializer.Bool(ResetWhenFull);

        if (version is > 0x24a and < 0x327)
            HideConnectors = serializer.Bool(HideConnectors);

        if (version is > 0x272 and < 0x398)
            LogicType = serializer.Enum32(LogicType);
        if (version is > 0x272 and < 0x369)
            UpdateFrame = serializer.I32(UpdateFrame);
        if (version > 0x272)
            InputList = serializer.Thingarray(InputList);

        if (version is > 0x276 and < 0x327)
            PortThing = serializer.Thing(PortThing);

        if (version > 0x283)
            IncludeRigidConnectors = serializer.Bool(IncludeRigidConnectors);

        if (version is > 0x284 and < 0x327)
        {
            CustomPortOffset = serializer.V4(CustomPortOffset);
            CustomConnectorOffset = serializer.V4(CustomConnectorOffset);
        }

        if (version > 0x28c)
        {
            TimerCount = serializer.F32(TimerCount);
            if (version < 0x2c4)
                TimerAutoCount = serializer.I8(TimerAutoCount);
        }

        if (version > 0x2ad && subVersion < 0x100)
            TeamFilter = serializer.I32(TeamFilter);

        if (version > 0x2c3) Behavior = serializer.Enum32(Behavior);

        if (version is < 0x329 and > 0x2c3) serializer.S32(0);

        if (version > 0x2c3)
        {
            RandomBehavior = serializer.I32(RandomBehavior);
            RandomPattern = serializer.I32(RandomPattern);
            RandomOnTimeMin = serializer.I32(RandomOnTimeMin);
            RandomOnTimeMax = serializer.I32(RandomOnTimeMax);
            RandomOffTimeMin = serializer.I32(RandomOffTimeMin);
            RandomOffTimeMax = serializer.I32(RandomOffTimeMax);
            if (version < 0x3ad)
            {
                RandomPhaseOn = serializer.U8(RandomPhaseOn);
                RandomPhaseTime = serializer.S32(RandomPhaseTime);
            }
            RetardedOldJoint = serializer.Bool(RetardedOldJoint);
        }

        if (version > 0x30f) KeySensorMode = serializer.I32(KeySensorMode);
        if (version > 0x34c)
        {
            UserDefinedColour = serializer.I32(UserDefinedColour);
            WiresVisible = serializer.Bool(WiresVisible);
        }
        if (version > 0x34f) BulletTypes = serializer.I8(BulletTypes);
        if (version > 0x390) DetectUnspawnedPlayers = serializer.Bool(DetectUnspawnedPlayers);
        if (subVersion > 0x216)
            UnspawnedBehavior = serializer.I8(UnspawnedBehavior);
        if (version > 0x3a4) PlaySwitchAudio = serializer.Bool(PlaySwitchAudio);

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();

            if (vita >= 0x1) // > 0x3c0
                SwitchTouchType = serializer.I32(SwitchTouchType);

            if (vita is >= 0x6 and < 0x36) // > 0x3c0
                serializer.U8(0);

            CursorScreenArea = vita switch
            {
                // > 0x3c0
                >= 0x9 and < 0x2c => (byte)serializer.U32(CursorScreenArea),
                >= 0x2c => serializer.I8(CursorScreenArea),
                _ => CursorScreenArea
            };

            if (vita >= 0xb) // > 0x3c0
                CursorInteractionType = serializer.I8(CursorInteractionType);
            if (vita >= 0xc) // > 0x3c0
                CursorTouchPanels = serializer.I8(CursorTouchPanels);
            if (vita >= 0x23) // > 0x3c0
                CursorTouchIndex = serializer.I8(CursorTouchIndex);

            switch (vita)
            {
                case < 0x36:
                {
                    // > 0x3c0
                    if (vita >= 0x23) serializer.U8(0);
                    if (vita >= 0x13) serializer.U8(0);
                    if (vita >= 0x7) serializer.U8(0);
                    if (vita >= 0x15) serializer.U8(0);
                    if (vita >= 0x24) serializer.U8(0); // Most of these should correspond to a value in sw.flags
                    break;
                }
                case >= 0x36:
                    Flags = serializer.I8(Flags);
                    break;
            }

            if (vita >= 0x2b)
                OutputAndOr = serializer.S32(OutputAndOr);

            if (vita is >= 0x2b and < 0x36)
                serializer.U8(0);

            // data sampler, although 0x2f shouldn't be the switch type?
            if (revision.IsVita() && (int)Type == 0x2f)
            {
                Value ??= new DataLabelValue();

                if (vita >= 0x45)
                    Value.LabelIndex = serializer.I32(Value.LabelIndex);
                if (vita >= 0x46)
                {
                    Value.CreatorId = serializer.Struct(Value.CreatorId);
                    Value.LabelName = serializer.Wstr(Value.LabelName);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1_LABEL_ANALOGUE_ARRAY))
                    Value.Analogue = serializer.Floatarray(Value.Analogue);
                else if (revision.Has(Branch.Double11, (int)Revisions.D_1DATALABELS))
                {
                    if (serializer.IsWriting())
                    {
                        var analogue = Value.Analogue != null && Value.Analogue.Length != 0 ?
                            Value.Analogue[0] : 0.0f;
                        serializer.GetOutput().F32(analogue);
                    }
                    else Value.Analogue = [serializer.GetInput().F32()];
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1_LABEL_TERNARY))
                    Value.Ternary = serializer.Bytearray(Value.Ternary);
            }

            switch (vita)
            {
                case >= 0x38 and < 0x41:
                    serializer.U8(0); // if equal to 0, includeSameChipTags is 1
                    break;
                case >= 0x41:
                    IncludeSameChipTags = serializer.I8(IncludeSameChipTags);
                    break;
            }

            if (vita >= 0x43)
            {
                GlowFrontCol = serializer.I32(GlowFrontCol);
                GlowBackCol = serializer.I32(GlowBackCol);
                GlowActiveCol = serializer.I32(GlowActiveCol);
            }

            if (vita >= 0x54)
                PlayerFilter = serializer.I8(PlayerFilter);
        }

        if (version > 0x3ec) PlayerMode = serializer.I8(PlayerMode);

        if (version > 0x3ee && Type == SwitchType.DATA_SAMPLER)
        {
            Value ??= new DataLabelValue();

            Value.LabelIndex = serializer.I32(Value.LabelIndex);
            Value.CreatorId = serializer.Struct(Value.CreatorId);
            Value.LabelName = serializer.Wstr(Value.LabelName);
            Value.Analogue = serializer.Floatarray(Value.Analogue);
            Value.Ternary = serializer.Bytearray(Value.Ternary);
        }

        if (subVersion > 0x21)
            RelativeToSequencer = serializer.Bool(RelativeToSequencer);
        if (subVersion > 0x2f)
            LayerRange = serializer.I8(LayerRange);
        if (subVersion > 0x7a)
        {
            BreakSound = serializer.Bool(BreakSound);
            ColorTimer = serializer.S32(ColorTimer);
        }

        if (subVersion > 0x67)
            IsLbp3Switch = serializer.Bool(IsLbp3Switch);
        if (subVersion > 0x68)
            RandomNonRepeating = serializer.Bool(RandomNonRepeating);
        if (subVersion > 0x102)
            StickerSwitchMode = serializer.I32(StickerSwitchMode);
    }

    // TODO: Actually implement

    public int GetAllocatedSize()
    {
        return 0;
    }
}