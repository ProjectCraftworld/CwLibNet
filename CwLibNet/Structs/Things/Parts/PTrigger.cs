using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types;
using CwLibNet.Types.Things;

namespace CwLibNet.Structs.Things.Parts;

public class PTrigger: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public TriggerType TriggerType = TriggerType.RADIUS;
    public Thing[]? InThings;
    public float RadiusMultiplier = 600.0f;

    public byte ZRangeHundreds;

    public bool AllZLayers;


    public float HysteresisMultiplier = 1.0f;
    public bool Enabled = true;

    public float ZOffset;

    public int ScoreValue;
    
    public PTrigger() { }

    public PTrigger(TriggerType type, float radius)
    {
        TriggerType = type;
        RadiusMultiplier = radius;
    }

    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();
        int subVersion = revision.GetSubVersion();


        TriggerType = serializer.Enum8(TriggerType);

        if (revision.Has(Branch.Double11, 0x17))
        {
            if (serializer.IsWriting())
            {
                MemoryOutputStream stream = serializer.GetOutput();
                stream.I32(InThings != null ? InThings.Length : 0);
                if (InThings != null)
                {
                    foreach (Thing thing in InThings)
                    {
                        serializer.Thing(thing);
                        stream.S32(0);
                    }
                }
            }
            else
            {
                MemoryInputStream stream = serializer.GetInput();
                InThings = new Thing[stream.I32()];
                for (int i = 0; i < InThings.Length; ++i)
                {
                    InThings[i] = serializer.Thing(null);
                    stream.S32(); // mThingAction
                }
            }
        }
        else
        {
            InThings = serializer.Thingarray(InThings);
        }

        RadiusMultiplier = serializer.F32(RadiusMultiplier);

        if (version < 0x1d5)
            serializer.I32(0); // zLayers?

        if (subVersion >= 0x2a)
            ZRangeHundreds = serializer.I8(ZRangeHundreds);

        AllZLayers = serializer.Bool(AllZLayers);

        if (version >= 0x19b)
        {
            HysteresisMultiplier = serializer.F32(HysteresisMultiplier);
            Enabled = serializer.Bool(Enabled);
        }

        if (version >= 0x322)
            ZOffset = serializer.F32(ZOffset);

        if (subVersion >= 0x90 || revision.Has(Branch.Double11, 0x30))
            ScoreValue = serializer.S32(ScoreValue);
    }

    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        // We'll actually calculate the size of these Things
        // in the Thing class to avoid circular dependencies.
        if (InThings != null)
            size += (InThings.Length) * Thing.BaseAllocationSize;
        return size;
    }
}