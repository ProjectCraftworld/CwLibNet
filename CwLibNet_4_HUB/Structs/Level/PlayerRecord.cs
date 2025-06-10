using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Level;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref playerIDs);
        Serializer.Serialize(ref playerNumbers);
        Serializer.Serialize(ref offset);
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