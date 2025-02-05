using Cwlib.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;
using CwLibNet.Types.Data;

namespace Cwlib.Structs.Dlc
{
    public class DLCFile : Serializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x30;
        public string directory;
        public string file;
        public string contentID;
        public string inGameCommerceID;
        public string categoryID;
        public List<GUID> guids = new List<GUID>();
        public List<GUID> nonPlanGuids = new List<GUID>();
        public int flags = DLCFileFlags.NONE;
        public int typeMask;
        public override void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
            directory = serializer.Str(directory);
            file = serializer.Str(file);
            contentID = serializer.Str(contentID);
            inGameCommerceID = serializer.Str(inGameCommerceID);
            if (revision.GetVersion() >= 0x264)
                categoryID = serializer.Str(categoryID);
            flags = serializer.I32(flags);
            if (revision.GetSubVersion() > 0x20c)
            {
                if (serializer.IsWriting())
                {
                    int[] output = new int[nonPlanGuids.Count];
                    for (int i = 0; i < output.length; ++i)
                    {
                        GUID guid = nonPlanGuids[i];
                        output[i] = guid == null ? 0 : ((int)guid.GetValue());
                    }

                    serializer.Intvector(output);
                }
                else
                {
                    int[] guids = serializer.Intvector(null);
                    if (guids != null)
                    {
                        nonPlanGuids = new List(guids.length);
                        for (int i = 0; i < guids.length; ++i)
                            nonPlanGuids.Add(new GUID(guids[i]));
                    }
                }
            }

            if (serializer.IsWriting())
            {
                int[] output = new int[guids.Count];
                for (int i = 0; i < output.length; ++i)
                {
                    GUID guid = guids[i];
                    output[i] = guid == null ? 0 : ((int)guid.GetValue());
                }

                serializer.Intvector(output);
            }
            else
            {
                int[] guids = serializer.Intvector(null);
                if (guids != null)
                {
                    this.guids = new List(guids.length);
                    for (int i = 0; i < guids.length; ++i)
                        this.guids.Add(new GUID(guids[i]));
                }
            }

            if (revision.Has(Branch.DOUBLE11, 0x85))
                typeMask = serializer.I32(typeMask);
        }

        public int GetAllocatedSize()
        {
            int size = DLCFile.BASE_ALLOCATION_SIZE;
            if (this.directory != null)
                size += this.directory.Length();
            if (this.file != null)
                size += this.file.Length();
            if (this.contentID != null)
                size += this.contentID.Length();
            if (this.inGameCommerceID != null)
                size += this.inGameCommerceID.Length();
            if (this.categoryID != null)
                size += this.categoryID.Length();
            if (this.guids != null)
                size += this.guids.Count * 0x4;
            if (this.nonPlanGuids != null)
                size += this.nonPlanGuids.Count * 0x4;
            return size;
        }
    }
}