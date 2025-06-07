using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
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

    public void Serialize() {

        var version = Serializer.GetRevision().GetVersion();
        var subVersion = Serializer.GetRevision().GetSubVersion();

        // A lot of fields were removed in 0x13c, across a lot of structures,
        // so I have no idea what they are, nor they do matter in any
        // version of the game anymore.

        if (version < 0x13c)
            Serializer.Serialize(ref null);

        PosVel = Serializer.Serialize(ref PosVel);

        if (version < 0x13c) {
            Serializer.Serialize(ref 0.0f);
            Serializer.Serialize(ref 0.0f);
        }

        Serializer.Serialize(ref AngVel);

        if (version < 0x13c) {
            Serializer.Serialize(ref 0.0f);
            Serializer.Serialize(ref null);

            if (Serializer.IsWriting()) Serializer.GetOutput().I32(0);
            else {
                var stream = Serializer.GetInput();
                var count = stream.I32();
                for (var i = 0; i < count; ++i)
                    stream.V3();
            }

            Serializer.Serialize(ref null, ResourceType.Material);

            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0.0f);
            Serializer.Serialize(ref null);

            Serializer.Serialize(ref null, ResourceType.Texture);

            Serializer.Serialize(ref 0.0f);
            Serializer.Serialize(ref 0);

            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref null);
            Serializer.Serialize(ref 0);
            Serializer.Serialize(ref 0.0f);
        }

        if (version >= 0x147)
            Serializer.Serialize(ref Frozen);
        else
            Serializer.Serialize(ref false);
        
        if ((version >= 0x22c && subVersion < 0x84) || subVersion >= 0x8b)
            Serializer.Serialize(ref EditingPlayer);
    }
    
    public int GetAllocatedSize() { return BaseAllocationSize; }
}