using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile 
{
    public class Challenge : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x20;

        public int challengeID;
        public int challengeStatus;
        public int levelID;
        public int scoreToBeat;

        public int levelType;
        public int myScore;
        public string? networkOnlineID;

        public void Serialize(Serializer serializer)
        {
            challengeID = serializer.I32(challengeID);
            challengeStatus = serializer.I32(challengeStatus);
            levelID = serializer.I32(levelID);
            scoreToBeat = serializer.I32(scoreToBeat);
            
            var revision = new Revision(); // Create an instance of Revision
            if (revision.Has(Branch.Double11, (int)Revisions.D1_CHALLENGE_LEVEL_TYPE)) // Assuming 'HasBranchAndRevision' is the correct method
                levelType = serializer.I32(levelType);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_CHALLENGE_SCORE)) // Assuming 'HasBranchAndRevision' is the correct method
            {
                myScore = serializer.I32(myScore);
                networkOnlineID = serializer.Str(networkOnlineID);
            }
        }

        public int GetAllocatedSize() 
        {
            var size = BASE_ALLOCATION_SIZE;
            if (networkOnlineID != null)
            {
                size += (networkOnlineID.Length);
            }
            return size;
        }
    }
}