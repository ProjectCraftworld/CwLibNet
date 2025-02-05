using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class Challenge : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;
        public int challengeID;
        public int challengeStatus;
        public int levelID;
        public int scoreToBeat;
        public int levelType;
        public int myScore;
        public string networkOnlineID;
        public override void Serialize(Serializer serializer)
        {
            Revision revision = serializer.Serialize();
            challengeID = serializer.Serialize(challengeID);
            challengeStatus = serializer.Serialize(challengeStatus);
            levelID = serializer.Serialize(levelID);
            scoreToBeat = serializer.Serialize(scoreToBeat);
            if (revision.Has(Branch.DOUBLE11, Revisions.D1_CHALLENGE_LEVEL_TYPE))
                levelType = serializer.Serialize(levelType);
            if (revision.Has(Branch.DOUBLE11, Revisions.D1_CHALLENGE_SCORE))
            {
                myScore = serializer.Serialize(myScore);
                networkOnlineID = serializer.Serialize(networkOnlineID);
            }
        }

        public virtual int GetAllocatedSize()
        {
            int size = Challenge.BASE_ALLOCATION_SIZE;
            if (this.networkOnlineID != null)
                size += (this.networkOnlineID.Length());
            return size;
        }
    }
}