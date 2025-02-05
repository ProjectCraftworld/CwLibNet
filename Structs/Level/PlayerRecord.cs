using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Level
{
    public class PlayerRecord : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x800;
        private NetworkPlayerID[] playerIDs = new NetworkPlayerID[32];
        private int[] playerNumbers = new int[32];
        private int offset;
        public PlayerRecord()
        {
            for (int i = 0; i < this.playerNumbers.length; ++i)
                playerNumbers[i] = -1;
        }

        public override void Serialize(Serializer serializer)
        {
            playerIDs = serializer.Serialize(playerIDs, typeof(NetworkPlayerID));
            playerNumbers = serializer.Serialize(playerNumbers);
            offset = serializer.Serialize(offset);
        }

        public virtual int GetAllocatedSize()
        {
            return PlayerRecord.BASE_ALLOCATION_SIZE;
        }

        public virtual NetworkPlayerID[] GetPlayerIDs()
        {
            return this.playerIDs;
        }

        public virtual int[] GetPlayerNumbers()
        {
            return this.playerNumbers;
        }

        public virtual int GetOffset()
        {
            return this.offset;
        }

        public virtual void SetOffset(int offset)
        {
            this.offset = offset;
        }
    }
}