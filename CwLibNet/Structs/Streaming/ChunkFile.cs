using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Profile;
using CwLibNet.Types.Data;

namespace CwLibNet.Structs.Streaming;

public class ChunkFile: ISerializable
{
    public const int BaseAllocationSize = 0x100;

    public Sha1 ChunkHash;
    public List<StreamingCheckpoint> CheckpointList = [];
    public List<QuestTracker> QuestTrackerList = [];
    public List<QuestSwitch> QuestSwitchList = [];
    public List<CollectableData> CollectablesList = [];
    public List<ResourceDescriptor?> UserResources = [];
    public Vector3? Min;
    public Vector3? Max;
    public bool HasObjectSaver;
    public bool DeleteObjectSavers;
    public bool DeleteOtherThings;
    public bool AntiStreaming;
    public List<GUID?> Guids = [];
    public List<Sha1>? Hashes = [];

    
    public void Serialize(Serializer serializer)
    {
        var subVersion = serializer.GetRevision().GetSubVersion();

        switch (subVersion)
        {
            case >= 0x73 and <= 0x130:
            {
                ResourceDescriptor descriptor = null;
                if (serializer.IsWriting())
                {
                    if (ChunkHash != null)
                        descriptor = new ResourceDescriptor(ChunkHash,
                            ResourceType.StreamingChunk);
                }
                descriptor = serializer.Resource(descriptor, ResourceType.StreamingChunk, true);
                if (!serializer.IsWriting())
                {
                    if (descriptor != null && descriptor.IsHash())
                        ChunkHash = descriptor.GetSHA1();
                    else
                        ChunkHash = Sha1.Empty;
                }

                break;
            }
            case > 0x130:
            {
                ChunkHash = serializer.Sha1(ChunkHash);
                if (serializer.IsWriting() && ChunkHash != null)
                    serializer.AddDependency(new ResourceDescriptor(ChunkHash,
                        ResourceType.StreamingChunk));
                break;
            }
        }

        if (subVersion > 0x72)
            CheckpointList = serializer.Arraylist(CheckpointList);

        if (subVersion > 0xeb)
            QuestTrackerList = serializer.Arraylist(QuestTrackerList);

        if (subVersion > 0x118)
            QuestSwitchList = serializer.Arraylist(QuestSwitchList);

        if (subVersion > 0x11d)
            CollectablesList = serializer.Arraylist(CollectablesList);

        if (subVersion >= 0xde)
        {
            var numItems = serializer.I32(UserResources != null ?
                UserResources.Count : 0);
            if (serializer.IsWriting())
            {
                foreach (var descriptor in UserResources)
                    serializer.Resource(descriptor, descriptor.GetResourceType(), true, false,
                        true);
            }
            else
            {
                UserResources = new List<ResourceDescriptor?>(numItems);
                for (var i = 0; i < numItems; ++i)
                    UserResources.Add(serializer.Resource(null, null, true, false, true));
            }
        }

        if (subVersion > 0x72)
        {
            Min = serializer.V3(Min);
            Max = serializer.V3(Max);
        }

        if (subVersion > 0x10d)
        {
            HasObjectSaver = serializer.Bool(HasObjectSaver);
            DeleteObjectSavers = serializer.Bool(DeleteObjectSavers);
            DeleteOtherThings = serializer.Bool(DeleteOtherThings);
        }

        if (subVersion > 0x133)
            AntiStreaming = serializer.Bool(AntiStreaming);

        if (subVersion <= 0x169) return;
        {
            {
                var numItems = serializer.I32(Guids != null ? Guids.Count : 0);
                if (serializer.IsWriting())
                {
                    foreach (var guid in Guids!)
                        serializer.GetOutput().Guid(guid);
                }
                else
                {
                    Guids = new List<GUID?>(numItems);
                    for (var i = 0; i < numItems; ++i)
                        Guids.Add(serializer.GetInput().Guid());
                }
            }

            {
                var numItems = serializer.I32(Hashes?.Count ?? 0);
                if (serializer.IsWriting())
                {
                    foreach (var sha1 in Hashes!)
                        serializer.GetOutput().Sha1(sha1);
                }
                else
                {
                    Hashes = new List<Sha1>(numItems);
                    for (var i = 0; i < numItems; ++i)
                        Hashes.Add(serializer.GetInput().Sha1()!);
                }
            }
        }
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}