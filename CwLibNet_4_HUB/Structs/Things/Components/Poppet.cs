using System.Numerics;
using CwLibNet.IO;
using CwLibNet.Structs.Things.Components.Popit;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Components;

public class Poppet: ISerializable
{
    public const int BaseAllocationSize = 0x0;

    public bool IsInUse;
    public PoppetMode[]? ModeStack;

    public PoppetEditState Edit = new();

    public Thing? TweakObject;

    public Vector4? BackupCameraZoneTargetBox;

    public Vector3? BackupCameraZonePitchAngle;

    public float BackupCameraZoneZoomDistance;

    public float CameraZoneZoomSpeed;

    public PoppetTweakObjectPlacement TweakObjectPlacement = new();

    public Vector3? MarqueeSelectOrigin;

    public Thing[]? MarqueeSelectList;

    public RaycastResults Raycast = new();

    public int DangerMode;

    public Thing[]? FrozenList;

    public Thing[]? HiddenList;

    public PoppetMaterialOverride OverrideMaterial = new();

    public PoppetShapeOverride OverrideShape = new();

    public Thing[]? TweakObjects;
    
    public void Serialize()
    {
        var version = Serializer.GetRevision().GetVersion();

        Serializer.Serialize(ref IsInUse);
        ModeStack = Serializer.Serialize(ref ModeStack);

        if (version < 0x2ed)
        {
            if (version < 0x232)
                Serializer.Serialize(ref null);
            switch (version)
            {
                case < 0x135:
                // revision - 0x185 < 0x35
                // scriptobjectuid
                case >= 0x185 and < 0x1ba:
                    Serializer.Serialize(ref 0); // c32
                    break;
            }

            Serializer.Serialize(ref Edit);

            if (version < 0x18f)
                Serializer.Serialize(ref null);
            if (version is >= 0x148 and < 0x185)
                Serializer.Reference(null);

            if (version >= 0x147)
            {
                TweakObject = Serializer.Reference(TweakObject);
                Serializer.Serialize(ref BackupCameraZoneTargetBox);
                BackupCameraZonePitchAngle =
                    Serializer.Serialize(ref BackupCameraZonePitchAngle);
                Serializer.Serialize(ref BackupCameraZoneZoomDistance);
                Serializer.Serialize(ref CameraZoneZoomSpeed);
                Serializer.Serialize(ref TweakObjectPlacement);
                if (version < 0x211)
                    Serializer.Serialize(ref false);
            }

            switch (version)
            {
                case > 0x184 and < 0x1dd:
                    Serializer.Serialize(ref null);
                    break;
                case > 0x1dc:
                {
                    if (version < 0x232) Serializer.Serialize(ref false);
                    MarqueeSelectOrigin = Serializer.Serialize(ref MarqueeSelectOrigin);
                    MarqueeSelectList = Serializer.Serialize(ref MarqueeSelectList);
                    break;
                }
            }

            switch (version)
            {
                case >= 0x218:
                    Serializer.Serialize(ref OverrideMaterial);
                    break;
                case >= 0x1b8 and < 0x1e2:
                    Serializer.Serialize(ref 0);
                    break;
            }

            switch (version)
            {
                case >= 0x1ba and < 0x1e2:
                    Serializer.Serialize(ref 0);
                    break;
                case >= 0x232:
                    Serializer.Serialize(ref Raycast);
                    break;
            }

            if (version >= 0x232)
                Serializer.Serialize(ref DangerMode);
            if (version >= 0x236)
            {
                Serializer.Serialize(ref 0);
                Serializer.Serialize(ref null);
                if (version >= 0x23a)
                    Serializer.Serialize(ref null);
            }

            return;
        }

        Serializer.Serialize(ref Raycast);
        if (version > 0x2ec)
            FrozenList = Serializer.Serialize(ref FrozenList);
        if (version > 0x2f1)
            HiddenList = Serializer.Serialize(ref HiddenList);
        if (version >= 0x311)
        {
            Serializer.Serialize(ref OverrideMaterial);
            Serializer.Serialize(ref OverrideShape);
        }

        if (version >= 0x3a0)
            TweakObjects = Serializer.Serialize(ref TweakObjects);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}