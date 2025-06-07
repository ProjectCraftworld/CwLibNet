using CwLibNet.IO;
using CwLibNet.Types.Data;
using CwLibNet.Enums;
using System.Numerics;
using CwLibNet.Structs.Things;
using CwLibNet.IO.Serializer;
using static CwLibNet.IO.Serializer.Serializer;

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

    public virtual void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        if (subVersion > 0x109) Serializer.Serialize(ref AreaId);
        if (subVersion < 0x108)
        {
            if (subVersion > 0x109) Area = Serializer.SerializeReference(Area);
        }
        else
        {
            if (subVersion > 0x186) Serializer.Serialize(ref MainAreaDescriptor, ResourceType.Plan);
            if (subVersion > 0x18b) Serializer.Serialize(ref ShadowAreaDescriptor, ResourceType.Plan);
        }

        if (subVersion > 0x178)
        {
            Serializer.Serialize(ref AmbienceTrack);
            Serializer.Serialize(ref AmbienceVolume);
        }

        if (subVersion > 0x189)
        {
            Serializer.Serialize(ref Spritelights);  

            var numPositions = SpritelightPositions?.Length ?? 0;
            Serializer.Serialize(ref numPositions); 
            if (!Serializer.IsWriting()) SpritelightPositions = new Vector3[numPositions];
            for (var i = 0; i < numPositions; ++i)
            {
                Vector3? pos = SpritelightPositions?[i];
                Serializer.Serialize(ref pos);
                if (SpritelightPositions != null && pos.HasValue) 
                    SpritelightPositions[i] = pos.Value;
            }
            Area = Serializer.SerializeReference(Area); // This is the area object, not the descriptor
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