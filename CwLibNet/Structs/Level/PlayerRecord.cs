using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Level;

public class PlayerRecord: ISerializable
{
    public const int BaseAllocationSize = 0x800;

    private NetworkPlayerID[]? playerIDs = new NetworkPlayerID[32];
    private int[] playerNumbers = new int[32];
    private int offset;

    public PlayerRecord()
    {
        for (var i = 0; i < playerNumbers.Length; ++i)
            playerNumbers[i] = -1;
    }

    
    public void Serialize(Serializer serializer)
    {
        playerIDs = serializer.Array(playerIDs);
        playerNumbers = serializer.Intarray(playerNumbers);
        offset = serializer.I32(offset);
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }

    public NetworkPlayerID[]? GetPlayerIDs()
    {
        return playerIDs;
    }

    public int[] GetPlayerNumbers()
    {
        return playerNumbers;
    }

    public int GetOffset()
    {
        return offset;
    }

    public void SetOffset(int offset)
    {
        this.offset = offset;
    }



}