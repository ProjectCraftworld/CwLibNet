using System;
using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Structs.Things;
using CwLibNet.Types.Data;

namespace Cwlib.Structs.Adventure
{
    public class PlanetArea : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x60;
        public int areaID;
        public ResourceDescriptor mainAreaDescriptor;
        public ResourceDescriptor shadowAreaDescriptor;
        public int ambienceTrack;
        public float ambienceVolume;
        public Thing[] spritelights;
        public Vector3[] spritelightPositions;
        public Thing area;
        public override void Serialize(Serializer serializer)
        {
            int subVersion = serializer.GetRevision().GetSubVersion();
            if (subVersion > 0x109)
                areaID = serializer.Serialize(areaID);
            if (subVersion < 0x187)
            {
                if (subVersion > 0x109)
                    area = serializer.Thing(area);
            }
            else
            {
                if (subVersion > 0x186)
                    mainAreaDescriptor = serializer.Resource(mainAreaDescriptor, ResourceType.PLAN, true);
                if (subVersion > 0x18b)
                    shadowAreaDescriptor = serializer.Resource(shadowAreaDescriptor, ResourceType.PLAN, true);
            }

            if (subVersion > 0x178)
            {
                ambienceTrack = serializer.S32(ambienceTrack);
                ambienceVolume = serializer.F32(ambienceVolume);
            }

            if (subVersion > 0x179)
            {
                spritelights = serializer.Thingarray(spritelights);
                int numPositions = serializer.Serialize(spritelightPositions != null ? spritelightPositions.length : 0);
                if (!serializer.IsWriting())
                    spritelightPositions = new Vector3[numPositions];
                for (int i = 0; i < numPositions; ++i)
                    spritelightPositions[i] = serializer.Vector3(spritelightPositions[i]);
                area = serializer.Thing(area);
            }
        }

        public int GetAllocatedSize()
        {
            int size = PlanetArea.BASE_ALLOCATION_SIZE;
            if (this.spritelights != null)
                size += (this.spritelights.length * 0x4);
            if (this.spritelightPositions != null)
                size += (this.spritelightPositions.length * 0x10);
            return size;
        }
    }
}