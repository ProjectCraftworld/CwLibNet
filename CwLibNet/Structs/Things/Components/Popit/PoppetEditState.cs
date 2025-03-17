using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Things;

namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetEditState: ISerializable
{
    public static int BASE_ALLOCATION_SIZE = 0x20;

    public ObjectState[] editObjectList;

    public int backZ, frontZ;

    public Thing[] frozenList;
    public float lerpFactor;
    public StickerInfo sticker = new StickerInfo();
    public DecorationInfo decoration = new DecorationInfo();

    public Thing cursorDummy;
    public Thing placementDummy;
    public PlacementBodyState[] placementBodyState;

    public Thing[] pauseList;

    public Vector3 vertexCursor;

    public float decorativeThingAngle;

    public Thing switchConnectorRef, switchConnector;

    public float decorativeThingScale;

    public PoppetShapeOverride overrideShape = new PoppetShapeOverride();
    public PoppetMaterialOverride overrideMaterial = new PoppetMaterialOverride();

    public int lastGridMoveFrame;
    public int lastGridRotateFrame, lastGridScaleFrame;

    public int switchConnectorUID;
    
    public void Serialize(Serializer serializer)
    {
        throw new NotImplementedException();
    }

    public int GetAllocatedSize()
    {
        throw new NotImplementedException();
    }
}