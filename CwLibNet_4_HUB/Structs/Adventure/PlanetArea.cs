using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.Enums;
using System.Numerics;
using CwLibNet.Structs.Things;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Adventure;

public class PlanetArea : ISerializable
{
    public const int BaseAllocationSize = 0x60;

    public int AreaId;
    public ResourceDescriptor? MainAreaDescriptor;
    public ResourceDescriptor? ShadowAreaDescriptor;
    public int AmbienceTrack;
    public float AmbienceVolume;
    public Thing[]? Spritelights;
    public Vector3[]? SpritelightPositions;
    public Thing? Area;

    public virtual void Serialize(Serializer serializer)
    {
        var subVersion = Serializer.GetRevision().GetSubVersion();

        if (subVersion > 0x109) Serializer.Serialize(ref AreaId);
        if (subVersion < 0x108)
        {
            if (subVersion > 0x109) Area = Serializer.Reference(Area);
        }
        else
        {
            if (subVersion > 0x186) MainAreaDescriptor = Serializer.Serialize(ref MainAreaDescriptor, ResourceType.Plan);
            if (subVersion > 0x18b) ShadowAreaDescriptor = Serializer.Serialize(ref ShadowAreaDescriptor, ResourceType.Plan);
        }

        if (subVersion > 0x178)
        {
            Serializer.Serialize(ref AmbienceTrack);
            Serializer.Serialize(ref AmbienceVolume);
        }

        if (subVersion > 0x189)
        {
            Spritelights = Serializer.Serialize(ref Spritelights);  

            var numPositions = Serializer.Serialize(ref SpritelightPositions?.Length ?? 0); 
            if (!Serializer.IsWriting()) SpritelightPositions = new Vector3[numPositions];
            for (var i = 0; i < numPositions; ++i)
            {
                Serializer.Serialize(ref SpritelightPositions[i]);
            }
            Area = Serializer.Reference(Area); // This is the area object, not the descriptor
        }
    }


    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Spritelights != null)
            size += Spritelights.Length * 0x4;
        if (SpritelightPositions != null)
            size += SpritelightPositions.Length * 0x10;
        return size;
    }
}