using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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

        public void Serialize()
        {
            Serializer.Serialize(ref challengeID);
            Serializer.Serialize(ref challengeStatus);
            Serializer.Serialize(ref levelID);
            Serializer.Serialize(ref scoreToBeat);
            
            var revision = new Revision(); // Create an instance of Revision
            if (revision.Has(Branch.Double11, (int)Revisions.D1_CHALLENGE_LEVEL_TYPE)) // Assuming 'HasBranchAndRevision' is the correct method
                Serializer.Serialize(ref levelType);

            if (revision.Has(Branch.Double11, (int)Revisions.D1_CHALLENGE_SCORE)) // Assuming 'HasBranchAndRevision' is the correct method
            {
                Serializer.Serialize(ref myScore);
                Serializer.Serialize(ref networkOnlineID);
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