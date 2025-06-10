using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Profile;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Streaming;

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

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

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
                Serializer.Serialize(ref descriptor, ResourceType.StreamingChunk, true, true, true);
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
                Serializer.Serialize(ref ChunkHash);
                if (Serializer.IsWriting() && ChunkHash != null)
                    Serializer.GetCurrentSerializer().AddDependency(new ResourceDescriptor(ChunkHash,
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
            int numItems = UserResources != null ? UserResources.Count : 0;
            Serializer.Serialize(ref numItems);
            if (Serializer.IsWriting())
            {
                foreach (var descriptor in UserResources)
                {
                    var desc = descriptor;
                    Serializer.Serialize(ref desc, descriptor.GetResourceType(), true, false,
                        true);
                }
            }
            else
            {
                UserResources = new List<ResourceDescriptor?>(numItems);
                for (var i = 0; i < numItems; ++i)
                {
                    ResourceDescriptor? tempResource = null;
                    Serializer.Serialize(ref tempResource, ResourceType.FileOfBytes, true, true, false);
                    UserResources.Add(tempResource);
                }
            }
        }

        if (subVersion > 0x72)
        {
            Serializer.Serialize(ref Min);
            Serializer.Serialize(ref Max);
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
                var numItems = Guids != null ? Guids.Count : 0;
                Serializer.Serialize(ref numItems);
                if (Serializer.IsWriting())
                {
                    foreach (var guid in Guids!)
                        Serializer.GetCurrentSerializer().GetOutput().Guid(guid);
                }
                else
                {
                    Guids = new List<GUID?>(numItems);
                    for (var i = 0; i < numItems; ++i)
                        Guids.Add(Serializer.GetCurrentSerializer().GetInput().Guid());
                }
            }

            {
                var numItems = Hashes?.Count ?? 0;
                Serializer.Serialize(ref numItems);
                if (Serializer.IsWriting())
                {
                    foreach (var sha1 in Hashes!)
                        Serializer.GetCurrentSerializer().GetOutput().Sha1(sha1);
                }
                else
                {
                    Hashes = new List<Sha1>(numItems);
                    for (var i = 0; i < numItems; ++i)
                        Hashes.Add(Serializer.GetCurrentSerializer().GetInput().Sha1()!);
                }
            }
        }
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}