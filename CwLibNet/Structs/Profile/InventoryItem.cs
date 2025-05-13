using System;
using System.Collections.Generic;

using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Inventory;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Profile
{
    public class InventoryItem : ISerializable
    {
        public const int BASE_ALLOCATION_SIZE = 0x40;


        public ResourceDescriptor Plan { get; set; }

        public InventoryItemDetails Details { get; set; } = new InventoryItemDetails();

        public GUID Guid { get; set; }

        public int UID { get; set; }

        public TutorialLevel TutorialLevel { get; set; } = TutorialLevel.UNKNOWN;
        public TutorialLevel TutorialVideo { get; set; } = TutorialLevel.UNKNOWN;

        public int Flags { get; set; } = InventoryItemFlags.NONE;

        public int UserCategoryIndex { get; set; }

        public InventoryItem() { }

        public InventoryItem(int uid, ResourceDescriptor descriptor, InventoryItemDetails details)
        {
            Details = details ?? new InventoryItemDetails();
            Details.DateAdded = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            UID = uid;
            Plan = descriptor;
        }

        public void Serialize(Serializer serializer)
        {
            Plan = serializer.Resource(Plan, ResourceType.Plan, true);
            if (Plan != null)
                serializer.AddDependency(Plan);

            if (serializer.GetRevision().GetSubVersion() >= (int)Revisions.ItemGuid)
                Guid = serializer.Guid(Guid) ?? new GUID();

            Details = serializer.Struct(Details);

            int version = serializer.GetRevision().GetVersion();

            if (version >= (int)Revisions.ItemFlags)
            {
                UID = serializer.I32(UID, true);
                if (version < (int)Revisions.RemoveLbp1Tutorials)
                {
                    if (serializer.IsWriting())
                    {
                        serializer.GetOutput().I32((int)TutorialLevel, true);
                        serializer.GetOutput().I32((int)TutorialVideo, true);
                    }
                    else
                    {
                        TutorialLevel = (TutorialLevel)serializer.GetInput().I32(true);
                        TutorialVideo = (TutorialLevel)serializer.GetInput().I32(true);
                    }
                }
                Flags = serializer.I32(Flags, true);
                if (version >= (int)Revisions.UserCategories)
                    UserCategoryIndex = serializer.I32(UserCategoryIndex, true);
            }
            else
            {
                throw new SerializationException("InventoryItem's below r565 are not supported!");
            }
        }

        public int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE + Details.GetAllocatedSize();
        }
    }
}
