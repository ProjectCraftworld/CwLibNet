using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using Branch = CwLibNet.Types.Branch;

namespace CwLibNet.Resources
{
    /* TODO: Implement this class */
    public class RTranslationTable : Resource
    {
        /**
         * Calculates LAMS key ID from translation tag.
         *
         * @param tag Translation tag
         * @return Hashed key from translation tag
         */
        public static long MakeLamsKeyID(String tag)
        {
            throw new NotImplementedException();
        }

        public override SerializationData Build(Revision revision, byte compressionFlags)
        {
            throw new NotImplementedException();
        }

        public override int GetAllocatedSize()
        {
            throw new NotImplementedException();
        }

        public override void Serialize(Serializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}