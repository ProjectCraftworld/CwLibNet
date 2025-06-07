using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.Structs.Profile;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Structs.Things.Components.Switches;
using CwLibNet.Types.Data;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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

    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        if (version < 0x1a5)
        {
            Thing? nullThing = null;
            Serializer.GetCurrentSerializer().Reference(nullThing);
            bool tempFalse = false;
            Serializer.Serialize(ref tempFalse);
        }

        Serializer.Serialize(ref Inverted);
        Serializer.Serialize(ref Radius);

        if (version >= 0x382) Serializer.Serialize(ref MinRadius);

        Serializer.Serialize(ref ColorIndex);

        if (version >= 0x2dc)
            Serializer.Serialize(ref Name);

        if (version >= 0x38f)
            Serializer.Serialize(ref CrappyOldLbp1Switch);
        if (!Serializer.IsWriting() && version < 0x309)
            CrappyOldLbp1Switch = true;

        Serializer.Serialize(ref BehaviorOld);

        if (version < 0x329)
        {
            // I would just use SwitchOutput, but ternary
            // doesn't exist in this struct, despite SwitchSignal having
            // it added in 0x310
            if (Serializer.IsWriting())
            {
                var stream = Serializer.GetCurrentSerializer().GetOutput();
                var output = Outputs != null && Outputs.Length != 0 ?
                    Outputs[0] : new SwitchOutput();
                Serializer.Serialize(ref output.Activation.Activation);
                if (version > 0x2a2) stream.I32(output.Activation.Player);
                Serializer.Serialize(ref output.TargetList);
            }
            else
            {
                var stream = Serializer.GetCurrentSerializer().GetInput();
                var output = new SwitchOutput
                {
                    Activation =
                    {
                        Activation = stream.F32()
                    }
                };
                if (version > 0x2a2)
                {
                    var tempPlayer = output.Activation.Player;
                    Serializer.Serialize(ref tempPlayer);
                    output.Activation.Player = tempPlayer;
                }
                ResourceDescriptor? nullDescriptor = null;
                output.Serialize(Serializer.GetCurrentSerializer());
                Outputs = [output];
            }
        }
        else 
        {
            Serializer.Serialize(ref Outputs);
        }

        if (version is < 0x398 and >= 0x140)
        {
            if (version < 0x160)
                Serializer.Serialize(ref RefSticker);
            else
                Serializer.Serialize(ref StickerPlan, ResourceType.Plan);
        }

        if (version is > 0x13f and < 0x1a5) 
        {
            int tempZero = 0;
            Serializer.Serialize(ref tempZero);
        }

        if (version > 0x197) Serializer.Serialize(ref HideInPlayMode);
        if (version > 0x1a4)
        {
            Serializer.SerializeEnum32(ref Type);
            Serializer.Serialize(ref ReferenceThing);
            Serializer.Serialize(ref ManualActivation);
        }

        switch (version)
        {
            case >= 0x398 when Type == SwitchType.STICKER || (Type == SwitchType.POCKET_ITEM && subVersion > 0x10):
                Serializer.Serialize(ref StickerPlan, ResourceType.Plan);
                break;
            case > 0x1a4 and < 0x368:
                Serializer.Serialize(ref PlatformVisualFactor);
                break;
        }

        if (version is > 0x1a4 and < 0x2a0) Serializer.Serialize(ref OldActivation);

        if (version > 0x1a4) Serializer.Serialize(ref ActivationHoldTime);
        if (version > 0x1a4) Serializer.Serialize(ref RequireAll);

        if (revision.Has(Branch.Double11, 0x2a))
            Serializer.Serialize(ref ImpactSensorMode);

        if (version is > 0x1fa and < 0x327)
        {
            Serializer.Serialize(ref ConnectorPos);

            // connectorGrabbed
            if (!Serializer.IsWriting())
                ConnectorGrabbed = new bool[Serializer.GetCurrentSerializer().GetInput().I32()];
            else
            {
                ConnectorGrabbed ??= [];
                Serializer.GetCurrentSerializer().GetOutput().I32(ConnectorGrabbed.Length);
            }
            for (var i = 0; i < ConnectorGrabbed.Length; ++i)
                Serializer.Serialize(ref ConnectorGrabbed[i]);

            Serializer.Serialize(ref PortPosOffset);
            Serializer.Serialize(ref LooseConnectorPos);
            Serializer.Serialize(ref LooseConnectorBaseOffset);
            Serializer.Serialize(ref LooseConnectorGrabbed);
        }

        if (version > 0x23d)
        {
            Serializer.Serialize(ref AngleRange);
            Serializer.Serialize(ref IncludeTouching);
            if (version >= 0x398 && Type == SwitchType.MICROCHIP && IncludeTouching == 1)
                Serializer.Serialize(ref StickerPlan, ResourceType.Plan);
        }

        if (subVersion > 0x165 && Type == SwitchType.GAME_LIVE_STREAMING_CHOICE)
        {
            throw new SerializationException("UNSUPPORTED!");
            // Wstr[4], Wstr[5] if subVersion >= 0x187?
        }

        if (version > 0x243) Serializer.Serialize(ref BulletsRequired);
        switch (version)
        {
            case 0x244:
                int tempZero = 0;
                Serializer.Serialize(ref tempZero); // ???
                break;
            case > 0x244:
                Serializer.Serialize(ref BulletsDetected);
                break;
        }

        if (version is > 0x245 and < 0x398)
            Serializer.Serialize(ref BulletPlayerNumber);
        if (version > 0x248)
            Serializer.Serialize(ref BulletRefreshTime);

        if (version >= 0x2f5) Serializer.Serialize(ref ResetWhenFull);

        if (version is > 0x24a and < 0x327)
            Serializer.Serialize(ref HideConnectors);

        if (version is > 0x272 and < 0x398)
            Serializer.SerializeEnum32(ref LogicType);
        if (version is > 0x272 and < 0x369)
            Serializer.Serialize(ref UpdateFrame);
        if (version > 0x272)
            Serializer.Serialize(ref InputList);

        if (version is > 0x276 and < 0x327)
            PortThing = Serializer.SerializeReference(PortThing);

        if (version > 0x283)
            Serializer.Serialize(ref IncludeRigidConnectors);

        if (version is > 0x284 and < 0x327)
        {
            Serializer.Serialize(ref CustomPortOffset);
            Serializer.Serialize(ref CustomConnectorOffset);
        }

        if (version > 0x28c)
        {
            Serializer.Serialize(ref TimerCount);
            if (version < 0x2c4)
                Serializer.Serialize(ref TimerAutoCount);
        }

        if (version > 0x2ad && subVersion < 0x100)
            Serializer.Serialize(ref TeamFilter);

        if (version > 0x2c3) Serializer.Serialize(ref Behavior);

        if (version is < 0x329 and > 0x2c3) Serializer.Serialize(ref temp_int);

        if (version > 0x2c3)
        {
            Serializer.Serialize(ref RandomBehavior);
            Serializer.Serialize(ref RandomPattern);
            Serializer.Serialize(ref RandomOnTimeMin);
            Serializer.Serialize(ref RandomOnTimeMax);
            Serializer.Serialize(ref RandomOffTimeMin);
            Serializer.Serialize(ref RandomOffTimeMax);
            if (version < 0x3ad)
            {
                Serializer.Serialize(ref RandomPhaseOn);
                Serializer.Serialize(ref RandomPhaseTime);
            }
            Serializer.Serialize(ref RetardedOldJoint);
        }

        if (version > 0x30f) Serializer.Serialize(ref KeySensorMode);
        if (version > 0x34c)
        {
            Serializer.Serialize(ref UserDefinedColour);
            Serializer.Serialize(ref WiresVisible);
        }
        if (version > 0x34f) Serializer.Serialize(ref BulletTypes);
        if (version > 0x390) Serializer.Serialize(ref DetectUnspawnedPlayers);
        if (subVersion > 0x216)
            Serializer.Serialize(ref UnspawnedBehavior);
        if (version > 0x3a4) Serializer.Serialize(ref PlaySwitchAudio);

        if (revision.IsVita())
        {
            int vita = revision.GetBranchRevision();

            if (vita >= 0x1) // > 0x3c0
                Serializer.Serialize(ref SwitchTouchType);

            if (vita is >= 0x6 and < 0x36) // > 0x3c0
                Serializer.Serialize(ref temp_int);

            CursorScreenArea = vita switch
            {
                // > 0x3c0
                >= 0x9 and < 0x2c => (byte)Serializer.Serialize(ref CursorScreenArea),
                >= 0x2c => Serializer.Serialize(ref CursorScreenArea),
                _ => CursorScreenArea
            };

            if (vita >= 0xb) // > 0x3c0
                Serializer.Serialize(ref CursorInteractionType);
            if (vita >= 0xc) // > 0x3c0
                Serializer.Serialize(ref CursorTouchPanels);
            if (vita >= 0x23) // > 0x3c0
                Serializer.Serialize(ref CursorTouchIndex);

            switch (vita)
            {
                case < 0x36:
                {
                    // > 0x3c0
                    if (vita >= 0x23) Serializer.Serialize(ref temp_int);
                    if (vita >= 0x13) Serializer.Serialize(ref temp_int);
                    if (vita >= 0x7) Serializer.Serialize(ref temp_int);
                    if (vita >= 0x15) Serializer.Serialize(ref temp_int);
                    if (vita >= 0x24) Serializer.Serialize(ref temp_int); // Most of these should correspond to a value in sw.flags
                    break;
                }
                case >= 0x36:
                    Serializer.Serialize(ref Flags);
                    break;
            }

            if (vita >= 0x2b)
                Serializer.Serialize(ref OutputAndOr);

            if (vita is >= 0x2b and < 0x36)
                Serializer.Serialize(ref temp_int);

            // data sampler, although 0x2f shouldn't be the switch type?
            if (revision.IsVita() && (int)Type == 0x2f)
            {
                Value ??= new DataLabelValue();

                if (vita >= 0x45)
                    Serializer.Serialize(ref Value.LabelIndex);
                if (vita >= 0x46)
                {
                    Value.CreatorId = Serializer.GetCurrentSerializer().Struct(Value.CreatorId);
                    Value.LabelName = Serializer.GetCurrentSerializer().Wstr(Value.LabelName);
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1_LABEL_ANALOGUE_ARRAY))
                    Value.Analogue = Serializer.GetCurrentSerializer().Floatarray(Value.Analogue);
                else if (revision.Has(Branch.Double11, (int)Revisions.D_1DATALABELS))
                {
                    if (Serializer.IsWriting())
                    {
                        var analogue = Value.Analogue != null && Value.Analogue.Length != 0 ?
                            Value.Analogue[0] : 0.0f;
                        Serializer.GetCurrentSerializer().GetOutput().F32(analogue);
                    }
                    else Value.Analogue = [Serializer.GetCurrentSerializer().GetInput().F32()];
                }

                if (revision.Has(Branch.Double11, (int)Revisions.D1_LABEL_TERNARY))
                    Value.Ternary = Serializer.Serialize(ref Value.Ternary);
            }

            switch (vita)
            {
                case >= 0x38 and < 0x41:
                    Serializer.Serialize(ref temp_int); // if equal to 0, includeSameChipTags is 1
                    break;
                case >= 0x41:
                    Serializer.Serialize(ref IncludeSameChipTags);
                    break;
            }

            if (vita >= 0x43)
            {
                Serializer.Serialize(ref GlowFrontCol);
                Serializer.Serialize(ref GlowBackCol);
                Serializer.Serialize(ref GlowActiveCol);
            }

            if (vita >= 0x54)
                Serializer.Serialize(ref PlayerFilter);
        }

        if (version > 0x3ec) Serializer.Serialize(ref PlayerMode);

        if (version > 0x3ee && Type == SwitchType.DATA_SAMPLER)
        {
            Value ??= new DataLabelValue();

            Serializer.Serialize(ref Value.LabelIndex);
            Value.CreatorId = Serializer.Serialize(ref Value.CreatorId);
            Value.LabelName = Serializer.Serialize(ref Value.LabelName);
            Value.Analogue = Serializer.Serialize(ref Value.Analogue);
            Value.Ternary = Serializer.Serialize(ref Value.Ternary);
        }

        if (subVersion > 0x21)
            Serializer.Serialize(ref RelativeToSequencer);
        if (subVersion > 0x2f)
            Serializer.Serialize(ref LayerRange);
        if (subVersion > 0x7a)
        {
            Serializer.Serialize(ref BreakSound);
            Serializer.Serialize(ref ColorTimer);
        }

        if (subVersion > 0x67)
            Serializer.Serialize(ref IsLbp3Switch);
        if (subVersion > 0x68)
            Serializer.Serialize(ref RandomNonRepeating);
        if (subVersion > 0x102)
            Serializer.Serialize(ref StickerSwitchMode);
    }

    // TODO: Actually implement

    public int GetAllocatedSize()
    {
        return 0;
    }
}