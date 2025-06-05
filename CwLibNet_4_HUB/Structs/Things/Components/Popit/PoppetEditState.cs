using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetEditState: ISerializable
{
    public static int BaseAllocationSize = 0x20;

    public ObjectState[] EditObjectList;

    public int BackZ, FrontZ;

    public Thing[] FrozenList;
    public float LerpFactor;
    public StickerInfo Sticker = new();
    public DecorationInfo Decoration = new();

    public Thing CursorDummy;
    public Thing PlacementDummy;
    public PlacementBodyState[] PlacementBodyState;

    public Thing[] PauseList;

    public Vector3 VertexCursor;

    public float DecorativeThingAngle;

    public Thing SwitchConnectorRef, SwitchConnector;

    public float DecorativeThingScale;

    public PoppetShapeOverride OverrideShape = new();
    public PoppetMaterialOverride OverrideMaterial = new();

    public int LastGridMoveFrame;
    public int LastGridRotateFrame, LastGridScaleFrame;

    public int SwitchConnectorUid;
    
    public void Serialize(Serializer serializer)
    {
        throw new NotImplementedException();
    }

    public int GetAllocatedSize()
    {
        throw new NotImplementedException();
    }
}