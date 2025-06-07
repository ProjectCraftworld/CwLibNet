using System.Numerics;
using CwLibNet.IO;
using CwLibNet.Structs.Things.Components.Popit;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

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
    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref IsInUse);
        Serializer.Serialize(ref ModeStack);

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
                    Serializer.Serialize(ref temp_int); // c32
                    break;
            }

            Serializer.Serialize(ref Edit);

            if (version < 0x18f)
                Serializer.Serialize(ref null);
            if (version is >= 0x148 and < 0x185)
                Serializer.SerializeReference(null);

            if (version >= 0x147)
            {
                TweakObject = Serializer.SerializeReference(TweakObject);
                Serializer.Serialize(ref BackupCameraZoneTargetBox);
                BackupCameraZonePitchAngle = Serializer.GetCurrentSerializer().V3(BackupCameraZonePitchAngle);
                Serializer.Serialize(ref BackupCameraZoneZoomDistance);
                Serializer.Serialize(ref CameraZoneZoomSpeed);
                Serializer.Serialize(ref TweakObjectPlacement);
                if (version < 0x211)
                    Serializer.Serialize(ref temp_bool_false);
            }

            switch (version)
            {
                case > 0x184 and < 0x1dd:
                    Serializer.Serialize(ref null);
                    break;
                case > 0x1dc:
                {
                    if (version < 0x232) Serializer.Serialize(ref temp_bool_false);
                    Serializer.Serialize(ref MarqueeSelectOrigin);
                    Serializer.Serialize(ref MarqueeSelectList);
                    break;
                }
            }

            switch (version)
            {
                case >= 0x218:
                    Serializer.Serialize(ref OverrideMaterial);
                    break;
                case >= 0x1b8 and < 0x1e2:
                    Serializer.Serialize(ref temp_int);
                    break;
            }

            switch (version)
            {
                case >= 0x1ba and < 0x1e2:
                    Serializer.Serialize(ref temp_int);
                    break;
                case >= 0x232:
                    Serializer.Serialize(ref Raycast);
                    break;
            }

            if (version >= 0x232)
                Serializer.Serialize(ref DangerMode);
            if (version >= 0x236)
            {
                Serializer.Serialize(ref temp_int);
                Serializer.Serialize(ref null);
                if (version >= 0x23a)
                    Serializer.Serialize(ref null);
            }

            return;
        }

        Serializer.Serialize(ref Raycast);
        if (version > 0x2ec)
            Serializer.Serialize(ref FrozenList);
        if (version > 0x2f1)
            Serializer.Serialize(ref HiddenList);
        if (version >= 0x311)
        {
            Serializer.Serialize(ref OverrideMaterial);
            Serializer.Serialize(ref OverrideShape);
        }

        if (version >= 0x3a0)
            Serializer.Serialize(ref TweakObjects);
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}