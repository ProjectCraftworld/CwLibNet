using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Parts;

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

    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();


        TriggerType = serializer.Enum32(TriggerType);

        if (revision.Has(Branch.Double11, 0x17))
        {
            if (Serializer.IsWriting())
            {
                var stream = Serializer.GetCurrentSerializer().GetOutput();
                stream.I32(InThings?.Length ?? 0);
                if (InThings != null)
                {
                    foreach (var thing in InThings)
                    {
                        Serializer.SerializeReference(thing);
                        stream.S32(0);
                    }
                }
            }
            else
            {
                var stream = Serializer.GetCurrentSerializer().GetInput();
                InThings = new Thing[stream.I32()];
                for (var i = 0; i < InThings.Length; ++i)
                {
                    Serializer.Serialize(ref InThings[i]);
                    stream.S32(); // mThingAction
                }
            }
        }
        else
        {
            Serializer.Serialize(ref InThings);
        }

        Serializer.Serialize(ref RadiusMultiplier);

        if (version < 0x1d5)
            Serializer.Serialize(ref temp_int); // zLayers?

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