using CwLibNet.Enums;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    public void Serialize()
    {
        var revision = Serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();


        Serializer.Serialize(ref TriggerType);

        if (revision.Has(Branch.Double11, 0x17))
        {
            if (Serializer.IsWriting())
            {
                var stream = Serializer.GetOutput();
                stream.I32(InThings?.Length ?? 0);
                if (InThings != null)
                {
                    foreach (var thing in InThings)
                    {
                        Serializer.Reference(thing);
                        stream.S32(0);
                    }
                }
            }
            else
            {
                var stream = Serializer.GetInput();
                InThings = new Thing[stream.I32()];
                for (var i = 0; i < InThings.Length; ++i)
                {
                    Serializer.Serialize(ref InThings[i])!;
                    stream.S32(); // mThingAction
                }
            }
        }
        else
        {
            InThings = Serializer.Serialize(ref InThings);
        }

        Serializer.Serialize(ref RadiusMultiplier);

        if (version < 0x1d5)
            Serializer.Serialize(ref 0); // zLayers?

        if (subVersion >= 0x2a)
            Serializer.Serialize(ref ZRangeHundreds);

        Serializer.Serialize(ref AllZLayers);

        if (version >= 0x19b)
        {
            Serializer.Serialize(ref HysteresisMultiplier);
            Serializer.Serialize(ref Enabled);
        }

        if (version >= 0x322)
            Serializer.Serialize(ref ZOffset);

        if (subVersion >= 0x90 || revision.Has(Branch.Double11, 0x30))
            Serializer.Serialize(ref ScoreValue);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        // We'll actually calculate the size of these Things
        // in the Thing class to avoid circular dependencies.
        if (InThings != null)
            size += InThings.Length * Thing.BaseAllocationSize;
        return size;
    }
}