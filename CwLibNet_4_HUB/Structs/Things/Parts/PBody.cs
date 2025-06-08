using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using CwLibNet.Types.Data;
using static CwLibNet.IO.Serializer.Serializer;
namespace CwLibNet.Structs.Things.Parts;

public class PBody: ISerializable
{
    public const int BaseAllocationSize = 0x100;

    /**
     * Positional velocity
     */
    public Vector3? PosVel;

    /**
     * Angular velocity
     */
    public float AngVel;

    /**
     * The state of this object
     */
    public int Frozen;

    /**
     * The player that's currently editing this thing(?)
     */
    public Thing? EditingPlayer;

    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer) {
        int temp_int = 0;
        float temp_float_0 = 0.0f;
        bool temp_bool_true = true;
        bool temp_bool_false = false;


        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        // A lot of fields were removed in 0x13c, across a lot of structures,
        // so I have no idea what they are, nor they do matter in any
        // version of the game anymore.

        if (version < 0x13c)
        {
            int tempNullInt = 0; // Placeholder for object serialization
            Serializer.Serialize(ref tempNullInt);
        }

        Serializer.Serialize(ref PosVel);

        if (version < 0x13c) {
            Serializer.Serialize(ref temp_float_0);
            Serializer.Serialize(ref temp_float_0);
        }

        Serializer.Serialize(ref AngVel);

        if (version < 0x13c) {
            Serializer.Serialize(ref temp_float_0);
            int tempNullInt = 0; // Placeholder for object serialization
            Serializer.Serialize(ref tempNullInt);

            if (Serializer.IsWriting()) Serializer.GetCurrentSerializer().GetOutput().I32(0);
            else {
                var stream = Serializer.GetCurrentSerializer().GetInput();
                var count = stream.I32();
                for (var i = 0; i < count; ++i)
                    stream.V3();
            }

            ResourceDescriptor? tempNull3 = null;
            Serializer.Serialize(ref tempNull3, ResourceType.Material);

            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_float_0);
            int tempNullInt2 = 0; // Placeholder for object serialization
            Serializer.Serialize(ref tempNullInt2);

            ResourceDescriptor? tempNull5 = null;
            Serializer.Serialize(ref tempNull5, ResourceType.Texture);

            Serializer.Serialize(ref temp_float_0);
            Serializer.Serialize(ref temp_int);

            Serializer.Serialize(ref temp_int);
            int tempNullInt3 = 0; // Placeholder for object serialization
            Serializer.Serialize(ref tempNullInt3);
            Serializer.Serialize(ref temp_int);
            Serializer.Serialize(ref temp_float_0);
        }

        if (version >= 0x147)
            Serializer.Serialize(ref Frozen);
        else
            Serializer.Serialize(ref temp_bool_false);
        
        if ((version >= 0x22c && subVersion < 0x84) || subVersion >= 0x8b)
            Serializer.Serialize(ref EditingPlayer);
    }
    
    public int GetAllocatedSize() { return BaseAllocationSize; }
}