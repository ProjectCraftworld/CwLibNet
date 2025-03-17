using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types.Things;

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

    public void Serialize(Serializer serializer) {

        int version = serializer.GetRevision().GetVersion();
        int subVersion = serializer.GetRevision().GetSubVersion();

        // A lot of fields were removed in 0x13c, across a lot of structures,
        // so I have no idea what they are, nor they do matter in any
        // version of the game anymore.

        if (version < 0x13c)
            serializer.V3(null);

        PosVel = serializer.V3(PosVel);

        if (version < 0x13c) {
            serializer.F32(0.0f);
            serializer.F32(0.0f);
        }

        AngVel = serializer.F32(AngVel);

        if (version < 0x13c) {
            serializer.F32(0.0f);
            serializer.V3(null);

            if (serializer.IsWriting()) serializer.GetOutput().I32(0);
            else {
                MemoryInputStream stream = serializer.GetInput();
                int count = stream.I32();
                for (int i = 0; i < count; ++i)
                    stream.V3();
            }

            serializer.Resource(null, ResourceType.Material);

            serializer.U8(0);
            serializer.F32(0.0f);
            serializer.V4(null);

            serializer.Resource(null, ResourceType.Texture);

            serializer.F32(0.0f);
            serializer.I32(0);

            serializer.I32(0);
            serializer.M44(null);
            serializer.I32(0);
            serializer.F32(0.0f);
        }

        if (version >= 0x147)
            Frozen = serializer.I32(Frozen);
        else
            serializer.Bool(false);
        
        if ((version >= 0x22c && subVersion < 0x84) || subVersion >= 0x8b)
            EditingPlayer = serializer.Reference(EditingPlayer);
    }
    
    public int GetAllocatedSize() { return PBody.BaseAllocationSize; }
}