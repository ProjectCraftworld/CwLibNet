using System.Runtime.Serialization;
using CwLibNet.Enums;
using CwLibNet.Types.Data;
using ISerializable = CwLibNet.IO.ISerializable;

namespace CwLibNet.Types.Things
{
    public class Thing : ISerializable
    {
        public static bool SerializeWorldThing = true;

        public const int BaseAllocationSize = 0x100;

        public string Name;

        public int UID = 1;
        public Thing World;
        public Thing Parent;
        public Thing GroupHead;
        public Thing OldEmitter;

        public short CreatedBy = -1, ChangedBy = -1;
        public bool IsStamping;
        public GUID PlanGuid;
        public bool Hidden;
        public short Flags;
        public byte ExtraFlags;

        private readonly ISerializable[] _parts = new ISerializable[0x3f];

        public Thing() { }

        public Thing(int uid)
        {
            this.UID = uid;
        }
    
        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.getRevision();
            int version = revision.GetVersion();
            int subVersion = revision.GetSubVersion();

            int maxPartsRevision = PartHistory.STREAMING_HINT;
            if (version <= 0x3e2)
                maxPartsRevision = PartHistory.CONTROLINATOR;
            if (version <= 0x33a)
                maxPartsRevision = PartHistory.MATERIAL_OVERRIDE;
            if (version <= 0x2c3)
                maxPartsRevision = PartHistory.MATERIAL_TWEAK;
            if (version <= 0x272)
                maxPartsRevision = PartHistory.GROUP;

            // Test serialization marker.
            if (revision.Has(Branch.Mizuki, (int)Revisions.MzSceneGraph)) name = serializer.wstr(name);
            else if (version >= Revisions.THING_TEST_MARKER || revision.has(Branch.LEERDAMMER, Revisions.LD_TEST_MARKER))
            {
                serializer.log("TEST_SERIALISATION_MARKER");
                if (serializer.u8(0xAA) != 0xaa)
                    throw new SerializationException("Test serialization marker is invalid, something has gone terribly wrong!");
            }

            if (version < 0x1fd)
            {
                if (serializer.isWriting())
                    serializer.reference(SERIALIZE_WORLD_THING ? world : null, typeof(Thing));
                else
                    world = serializer.reference(world, typeof(Thing));
            }
            if (version < 0x27f)
            {
                parent = serializer.reference(parent, typeof(Thing));
                UID = serializer.i32(UID);
            }
            else
            {
                UID = serializer.i32(UID);
                parent = serializer.reference(parent, typeof(Thing));
            }

            groupHead = serializer.reference(groupHead, typeof(Thing));

            if (version >= 0x1c7)
                oldEmitter = serializer.reference(oldEmitter, typeof(Thing));

            if (version >= 0x1a6 && version < 0x1bc)
                serializer.array(null, PJoint.class, true);

            if ((version >= 0x214 && !revision.isToolkit()) || revision.before(Branch.MIZUKI,
                    Revisions.MZ_SCENE_GRAPH))
            {
                createdBy = serializer.i16(createdBy);
                changedBy = serializer.i16(changedBy);
            }

            if (version < 0x341)
            {
                if (version > 0x21a)
                    isStamping = serializer.bool(isStamping);
                if (version >= 0x254)
                    planGUID = serializer.guid(planGUID);
                if (version >= 0x2f2)
                    hidden = serializer.bool(hidden);
            }
            else
            {
                if (version >= 0x254)
                    planGUID = serializer.guid(planGUID);

                if (version >= 0x341)
                {
                    if (revision.has(Branch.DOUBLE11, 0x62))
                        flags = serializer.i16(flags);
                    else
                        flags = serializer.i8((byte) flags);
                }
                if (subVersion >= 0x110)
                    extraFlags = serializer.i8(extraFlags);
            }

            bool isCompressed = (version >= 0x297 || revision.has(Branch.LEERDAMMER,
                Revisions.LD_RESOURCES));

            int partsRevision = PartHistory.STREAMING_HINT;
            long flags = -1;

            if (serializer.isWriting())
            {
                serializer.log("GENERATING FLAGS");
                Part lastPart = null;
                if (isCompressed) flags = 0;
                foreach (Part part in Part.values())
                {
                    int index = part.GetIndex();
                    if (version >= 0x13c && (index >= 0x36 && index <= 0x3c)) continue;
                    if (version >= 0x18c && index == 0x3d) continue;
                    if (subVersion >= 0x107 && index == 0x3e) continue;
                    else if (index == 0x3e)
                    {
                        if (parts[index] != null)
                        {
                            flags |= (1L << 0x29);
                            lastPart = part;
                        }
                        continue;
                    }

                    if (parts[index] != null)
                    {
                        // Offset due to PCreatorAnim
                        if (subVersion < 0x107 && index > 0x28) index++;

                        flags |= (1L << index);

                        lastPart = part;
                    }
                }
                partsRevision = (lastPart == null) ? 0 : lastPart.GetVersion();
            }

            if (serializer.isWriting())
            {
                if (partsRevision > maxPartsRevision)
                    partsRevision = maxPartsRevision;
            }

            partsRevision = serializer.s32(partsRevision);
            if (isCompressed)
            {
                // serializer.log("FLAGS");
                flags = serializer.u64(flags);
            }

            // I have no idea why they did this
            if (version == 0x13c) partsRevision += 7;

            Part[] partsToSerialize = Part.FromFlags(revision.getHead(), flags, partsRevision);
            serializer.log(Arrays.toString(partsToSerialize));

            for (Part part : partsToSerialize)
            {
                serializer.log(part.name() + " [START]");
                if (!part.serialize(this.parts, partsRevision, flags, serializer))
                {
                    serializer.log(part.name() + " FAILED");
                    throw new SerializationException(part.name() + " failed to serialize!");
                }
                serializer.log(part.name() + " [END]");
            }

            serializer.log("THING " + Bytes.toHex(UID) + " [END]");
        }
    }
}