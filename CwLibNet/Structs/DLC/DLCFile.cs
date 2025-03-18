using System;
using System.IO;
using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types;
using CwLibNet.Util;
using CwLibNet.Enums;
using System.Runtime.Serialization;
using ISerializable = CwLibNet.IO.ISerializable;
using System.Collections;
using CwLibNet.Types.Data;
using System.Linq;

namespace CwLibNet.Structs.DLC
{
    public class DLCFile : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x30; // for the DLCID

        public string directory;
        public string file;
        public string contentID;
        public string inGameCommerceID;
    
        public string categoryID;
        
        public List<GUID> nonPlanGuids = new List<GUID>();
        public List<GUID> guids = new List<GUID>();

        public int flags = DLCFileFlags.NONE;
        public int typeMask;

        public virtual void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();

            directory = serializer.Str(directory);
            file = serializer.Str(file);
            contentID = serializer.Str(contentID);
            inGameCommerceID = serializer.Str(inGameCommerceID);
            if (((int)revision) >= 0x264)
            {
                categoryID = serializer.Str(categoryID);
            }
            flags = serializer.I32(flags);

            if (((int)revision) >= 0x20C)
            {
                if (serializer.IsWriting())
                {
                    int[] output = new int[nonPlanGuids.Count];
                    // temporary fix for guids.size() not being correct
                    for (int i = 0; i < output.Length; ++i)
                    {
                        GUID guid = nonPlanGuids[i];
                        output[i] = (int)(guid == null ? 0 : guid.Value);
                    }
                    serializer.Intvector(output);
                }
                else
                {
                    int[] guids = serializer.Intvector(null);
                    if (guids != null)
                    {
                        nonPlanGuids = new List<GUID>(guids.Length);
                        for (int i = 0; i < guids.Length; ++i)
                            nonPlanGuids.Add(new GUID(guids[i]));
                    }
                }
            }

            if (serializer.IsWriting())
            {
                int[] output = new int[guids.Count];
                // temporary fix for guids.size() not being correct
                for (int i = 0; i < output.Length; ++i)
                {
                    GUID guid = guids[i];
                    output[i] = (int)(guid == null ? 0 : guid.Value);
                }
                serializer.Intvector(output);
            }
            else
            {
                int[] guids = serializer.Intvector(null);
                if (guids != null)
                {
                    this.guids = new List<GUID>(guids.Length);
                    for (int i = 0; i < guids.Length; ++i)
                        this.guids.Add(new GUID(guids[i]));
                }
            }

            if (revision.Has(Branch.Double11, 0x85)) typeMask = serializer.I32(typeMask);
        }

        public int GetAllocatedSize()
        {
            int size = DLCFile.BASE_ALLOCATION_SIZE;
            if (this.directory != null) size += this.directory.Length;
            if (this.file != null) size += this.file.Length;
            if (this.contentID != null) size += this.contentID.Length;
            if (this.inGameCommerceID != null) size += this.inGameCommerceID.Length;
            if (this.categoryID != null) size += this.categoryID.Length;
            if (this.guids != null)
                size += this.guids.Count * 0x4;
                // temporary fix for guids.size() not being correct
            if (this.nonPlanGuids != null)
                size += this.nonPlanGuids.Count * 0x4;
                // temporary fix for guids.size() not being correct
            return size;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}