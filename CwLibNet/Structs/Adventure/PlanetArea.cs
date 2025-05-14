using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Enums;
using System.Numerics;
using CwLibNet.Structs.Things;

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
        var subVersion = serializer.GetRevision().GetSubVersion();

        if (subVersion > 0x109) AreaId = serializer.S32(AreaId);
        if (subVersion < 0x108)
        {
            if (subVersion > 0x109) Area = serializer.Thing(Area);
        }
        else
        {
            if (subVersion > 0x186) MainAreaDescriptor = serializer.Resource(MainAreaDescriptor,
                ResourceType.Plan, true);
            if (subVersion > 0x18b) ShadowAreaDescriptor = serializer.Resource(ShadowAreaDescriptor,
                ResourceType.Plan, true);
        }

        if (subVersion > 0x178)
        {
            AmbienceTrack = serializer.S32(AmbienceTrack);
            AmbienceVolume = serializer.F32(AmbienceVolume);
        }

        if (subVersion > 0x189)
        {
            Spritelights = serializer.Thingarray(Spritelights);  

            var numPositions = serializer.I32(SpritelightPositions?.Length ?? 0); 
            if (!serializer.IsWriting()) SpritelightPositions = new Vector3[numPositions];
            for (var i = 0; i < numPositions; ++i)
            {
                SpritelightPositions[i] = (Vector3)serializer.V3(SpritelightPositions[i]);
            }
            Area = serializer.Thing(Area); // This is the area object, not the descriptor
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