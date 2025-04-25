using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things;
using CwLibNet.Util;

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
    public float AnimPos = 0.0f, AnimSpeed = 1.0f;
    public float AnimPosOld = -1.0f;
    public bool AnimLoop = true;
    public float LoopStart = 0.0f, LoopEnd = 1.0f;
    public int EditorColor = -1;
    public ShadowType CastShadows = ShadowType.ALWAYS;
    public bool RttEnable;
    public byte VisibilityFlags = (byte)(Enums.VisibilityFlags.PLAY_MODE | Enums.VisibilityFlags.EDIT_MODE);
    public float PoppetRenderScale = 1.0f;
    public float ParentDistanceFront, ParentDistanceSide;
    public bool IsDirty = true;
    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        Mesh = serializer.Resource(Mesh, ResourceType.Mesh);

        BoneThings = serializer.Thingarray(BoneThings);
        
        Anim = serializer.Resource(Anim, ResourceType.Animation);
        AnimPos = serializer.F32(AnimPos);
        AnimSpeed = serializer.F32(AnimSpeed);
        AnimLoop = serializer.Bool(AnimLoop);
        LoopStart = serializer.F32(LoopStart);
        LoopEnd = serializer.F32(LoopEnd);
        
        if (version > 0x31a) EditorColor = serializer.I32(EditorColor);
        else {
            if (serializer.IsWriting())
                serializer.GetOutput().V4(Colors.FromARGB(EditorColor));
            else {
                Vector4 color = serializer.GetInput().V4();
                EditorColor = Colors.GetARGB(color);
            }
        }
        
        CastShadows = serializer.Enum8(CastShadows);
        RttEnable = serializer.Bool(RttEnable);
        
        if (version > 0x2e2)
            VisibilityFlags = serializer.I8(VisibilityFlags);
        else {
            if (serializer.IsWriting())
                serializer.GetOutput().Boole((VisibilityFlags & (byte)Enums.VisibilityFlags.PLAY_MODE) != 0);
            else {
                VisibilityFlags = (byte)Enums.VisibilityFlags.EDIT_MODE;
                if (serializer.GetInput().Boole())
                    VisibilityFlags |=  (byte)Enums.VisibilityFlags.PLAY_MODE;
            }
        }
        
        PoppetRenderScale = serializer.F32(PoppetRenderScale);

        if (version is <= 0x1f5 or >= 0x34d) return;
        ParentDistanceFront = serializer.F32(ParentDistanceFront);
        ParentDistanceSide = serializer.F32(ParentDistanceFront);
    }

    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (BoneThings != null) size += (BoneThings.Length * 0x4);
        return size;
    }
}