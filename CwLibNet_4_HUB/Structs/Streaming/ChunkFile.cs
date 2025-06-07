using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Structs.Profile;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

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

    
    public void Serialize()
    {
        var subVersion = Serializer.GetRevision().GetSubVersion();

        switch (subVersion)
        {
            case >= 0x73 and <= 0x130:
            {
                ResourceDescriptor descriptor = null;
                if (Serializer.IsWriting())
                {
                    if (ChunkHash != null)
                        descriptor = new ResourceDescriptor(ChunkHash,
                            ResourceType.StreamingChunk);
                }
                Serializer.Serialize(ref descriptor, descriptor, ResourceType.StreamingChunk, true);
                if (!Serializer.IsWriting())
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
                ChunkHash = Serializer.Serialize(ref ChunkHash);
                if (Serializer.IsWriting() && ChunkHash != null)
                    Serializer.AddDependency(new ResourceDescriptor(ChunkHash,
                        ResourceType.StreamingChunk));
                break;
            }
        }

        if (subVersion > 0x72)
            Serializer.Serialize(ref CheckpointList);

        if (subVersion > 0xeb)
            Serializer.Serialize(ref QuestTrackerList);

        if (subVersion > 0x118)
            Serializer.Serialize(ref QuestSwitchList);

        if (subVersion > 0x11d)
            Serializer.Serialize(ref CollectablesList);

        if (subVersion >= 0xde)
        {
            var numItems = Serializer.Serialize(ref UserResources != null ?
                UserResources.Count : 0);
            if (Serializer.IsWriting())
            {
                foreach (var descriptor in UserResources)
                    Serializer.Serialize(ref descriptor, descriptor.GetResourceType(), true, false,
                        true);
            }
            else
            {
                UserResources = new List<ResourceDescriptor?>(numItems);
                for (var i = 0; i < numItems; ++i)
                    UserResources.Add(Serializer.Serialize(ref null, null, true, false, true));
            }
        }

        if (subVersion > 0x72)
        {
            Min = Serializer.Serialize(ref Min);
            Max = Serializer.Serialize(ref Max);
        }

        if (subVersion > 0x10d)
        {
            Serializer.Serialize(ref HasObjectSaver);
            Serializer.Serialize(ref DeleteObjectSavers);
            Serializer.Serialize(ref DeleteOtherThings);
        }

        if (subVersion > 0x133)
            Serializer.Serialize(ref AntiStreaming);

        if (subVersion <= 0x169) return;
        {
            {
                var numItems = Serializer.Serialize(ref Guids != null ? Guids.Count : 0);
                if (Serializer.IsWriting())
                {
                    foreach (var guid in Guids!)
                        Serializer.GetOutput().Guid(guid);
                }
                else
                {
                    Guids = new List<GUID?>(numItems);
                    for (var i = 0; i < numItems; ++i)
                        Guids.Add(Serializer.GetInput().Guid());
                }
            }

            {
                var numItems = Serializer.Serialize(ref Hashes?.Count ?? 0);
                if (Serializer.IsWriting())
                {
                    foreach (var sha1 in Hashes!)
                        Serializer.GetOutput().Sha1(sha1);
                }
                else
                {
                    Hashes = new List<Sha1>(numItems);
                    for (var i = 0; i < numItems; ++i)
                        Hashes.Add(Serializer.GetInput().Sha1()!);
                }
            }
        }
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}