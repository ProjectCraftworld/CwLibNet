using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.Util;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things.Parts;

public class PRenderMesh: ISerializable
{
    public static int BaseAllocationSize = 0x80;
//    public static Dictionary<ResourceDescriptor, RAnimation> Animations = new();
//    public static HashSet<ResourceDescriptor> DisabledAnimations = [];

    public ResourceDescriptor? Mesh;
//    public MeshInstance Instance;
    public Thing[]? BoneThings = [];
    public Matrix4x4[] BoneModels = [];
    public ResourceDescriptor? Anim;
    public float AnimPos, AnimSpeed = 1.0f;
    public float AnimPosOld = -1.0f;
    public bool AnimLoop = true;
    public float LoopStart, LoopEnd = 1.0f;
    public int EditorColor = -1;
    public ShadowType CastShadows = ShadowType.ALWAYS;
    public bool RttEnable;
    public byte VisibilityFlags = (byte)(Enums.VisibilityFlags.PLAY_MODE | Enums.VisibilityFlags.EDIT_MODE);
    public float PoppetRenderScale = 1.0f;
    public float ParentDistanceFront, ParentDistanceSide;
    public bool IsDirty = true;
    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref Mesh, ResourceType.Mesh, false, true, false);

        Serializer.Serialize(ref BoneThings);
        
        Serializer.Serialize(ref Anim, ResourceType.Animation, false, true, false);
        Serializer.Serialize(ref AnimPos);
        Serializer.Serialize(ref AnimSpeed);
        Serializer.Serialize(ref AnimLoop);
        Serializer.Serialize(ref LoopStart);
        Serializer.Serialize(ref LoopEnd);
        
        if (version > 0x31a) Serializer.Serialize(ref EditorColor);
        else {
            if (Serializer.IsWriting())
                Serializer.GetCurrentSerializer().GetOutput().V4(Colors.FromARGB(EditorColor));
            else {
                var color = Serializer.GetCurrentSerializer().GetInput().V4();
                EditorColor = Colors.GetARGB(color);
            }
        }
        
        Serializer.Serialize(ref CastShadows);
        Serializer.Serialize(ref RttEnable);
        
        if (version > 0x2e2)
            Serializer.Serialize(ref VisibilityFlags);
        else {
            if (Serializer.IsWriting())
                Serializer.GetCurrentSerializer().GetOutput().Boole((VisibilityFlags & (byte)Enums.VisibilityFlags.PLAY_MODE) != 0);
            else {
                VisibilityFlags = (byte)Enums.VisibilityFlags.EDIT_MODE;
                if (Serializer.GetCurrentSerializer().GetInput().Boole())
                    VisibilityFlags |=  (byte)Enums.VisibilityFlags.PLAY_MODE;
            }
        }
        
        Serializer.Serialize(ref PoppetRenderScale);

        if (version is <= 0x1f5 or >= 0x34d) return;
        Serializer.Serialize(ref ParentDistanceFront);
        Serializer.Serialize(ref ParentDistanceFront);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (BoneThings != null) size += BoneThings.Length * 0x4;
        return size;
    }
}