using System;
using System.IO;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Enums;
using CwLibNet.Types.Things;
using System.Numerics;

namespace CwLibNet.Structs.Adventure
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

        public virtual void serialize(Serializer serializer)
        {
            int subVersion = serializer.GetRevision().GetSubVersion();

            if (subVersion > 0x109) areaID = serializer.S32(areaID);
            if (subVersion < 0x108)
            {
                if (subVersion > 0x109) area = serializer.Thing(area);
            }
            else
            {
                if (subVersion > 0x186) mainAreaDescriptor = serializer.Resource(mainAreaDescriptor,
                    ResourceType.Plan, true);
                if (subVersion > 0x18b) shadowAreaDescriptor = serializer.Resource(shadowAreaDescriptor,
                    ResourceType.Plan, true);
            }

            if (subVersion > 0x178)
            {
                ambienceTrack = serializer.S32(ambienceTrack);
                ambienceVolume = serializer.F32(ambienceVolume);
            }

            if (subVersion > 0x189)
            {
                spritelights = serializer.Thingarray(spritelights);  

                int numPositions = serializer.I32(spritelightPositions?.Length ?? 0); 
                if (!serializer.IsWriting()) spritelightPositions = new Vector3[numPositions];
                for (int i = 0; i < numPositions; ++i)
                {
                    spritelightPositions[i] = (Vector3)serializer.V3(spritelightPositions[i]);
                }
                area = serializer.Thing(area); // This is the area object, not the descriptor
            }
        }


        public int GetAllocatedSize()
        {
            int size = PlanetArea.BASE_ALLOCATION_SIZE;
            if (this.spritelights != null)
                size += (this.spritelights.Length * 0x4);
            if (this.spritelightPositions != null)
                size += (this.spritelightPositions.Length * 0x10);
            return size;
        }

        public void Serialize(Serializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}