using System.Runtime.Serialization;
using CwLibNet.Enums;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types.Data;
using CwLibNet.Util;
using ISerializable = CwLibNet.IO.ISerializable;

namespace CwLibNet.Types.Things
{
    public class Thing : ISerializable
    {
        public static bool SerializeWorldThing = true;

        public const int BaseAllocationSize = 0x100;

        public string? Name;

        public int UID = 1;
        public Thing? World;
        public Thing? Parent;
        public Thing? GroupHead;
        public Thing? OldEmitter;

        public short CreatedBy = -1, ChangedBy = -1;
        public bool IsStamping;
        public GUID? PlanGuid;
        public bool Hidden;
        public short Flags;
        public byte ExtraFlags;

        private readonly ISerializable?[] parts = new ISerializable[0x3f];

        public Thing() { }

        public Thing(int uid)
        {
            this.UID = uid;
        }
    
        public void Serialize(Serializer serializer)
        {
            Revision revision = serializer.GetRevision();
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
            if (revision.Has(Branch.Mizuki, (int)Revisions.MzSceneGraph)) Name = serializer.Wstr(Name);
            else if (version >= (int)Revisions.ThingTestMarker || revision.Has(Branch.Leerdammer, (int)Revisions.LdTestMarker))
            {
                serializer.Log("TEST_SERIALISATION_MARKER");
                if (serializer.U8(0xAA) != 0xaa)
                    throw new SerializationException("Test serialization marker is invalid, something has gone terribly wrong!");
            }

            if (version < 0x1fd)
            {
                if (serializer.IsWriting())
                    serializer.Reference(SerializeWorldThing ? World : null);
                else
                    World = serializer.Reference(World);
            }
            if (version < 0x27f)
            {
                Parent = serializer.Reference(Parent);
                UID = serializer.I32(UID);
            }
            else
            {
                UID = serializer.I32(UID);
                Parent = serializer.Reference(Parent);
            }

            GroupHead = serializer.Reference(GroupHead);

            if (version >= 0x1c7)
                OldEmitter = serializer.Reference(OldEmitter);

            if (version >= 0x1a6 && version < 0x1bc)
                serializer.Array<PJoint>(null, true);

            if ((version >= 0x214 && !revision.IsToolkit()) || revision.Before(Branch.Mizuki, (int)Revisions.MzSceneGraph))
            {
                CreatedBy = serializer.I16(ChangedBy);
                ChangedBy = serializer.I16(ChangedBy);
            }

            if (version < 0x341)
            {
                if (version > 0x21a)
                    IsStamping = serializer.Bool(IsStamping);
                if (version >= 0x254)
                    PlanGuid = serializer.Guid(PlanGuid);
                if (version >= 0x2f2)
                    Hidden = serializer.Bool(Hidden);
            }
            else
            {
                if (version >= 0x254)
                    PlanGuid = serializer.Guid(PlanGuid);

                if (version >= 0x341)
                {
                    Flags = revision.Has(Branch.Double11, 0x62) ? serializer.I16(Flags) : serializer.I8((byte) Flags);
                }
                if (subVersion >= 0x110)
                    ExtraFlags = serializer.I8(ExtraFlags);
            }

            bool isCompressed = (version >= 0x297 || revision.Has(Branch.Leerdammer,
                (int)Revisions.LdResources));

            int partsRevision = PartHistory.STREAMING_HINT;
            long flags = -1;

            if (serializer.IsWriting())
            {
                serializer.Log("GENERATING FLAGS");
                Part? lastPart = null;
                if (isCompressed) flags = 0;
                foreach (Part part in Part.Parts.Values)
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

                    if (parts[index] == null) continue;
                    // Offset due to PCreatorAnim
                    if (subVersion < 0x107 && index > 0x28) index++;

                    flags |= (1L << index);

                    lastPart = part;
                }
                partsRevision = lastPart?.GetVersion() ?? 0;
            }

            if (serializer.IsWriting())
            {
                if (partsRevision > maxPartsRevision)
                    partsRevision = maxPartsRevision;
            }

            partsRevision = serializer.S32(partsRevision);
            if (isCompressed)
            {
                // serializer.log("FLAGS");
                flags = serializer.U64(flags);
            }

            // I have no idea why they did this
            if (version == 0x13c) partsRevision += 7;

            Part[] partsToSerialize = Part.FromFlags(revision.Head, flags, partsRevision);
            serializer.Log(string.Join(' ', partsToSerialize));

            foreach (Part part in partsToSerialize)
            {
                serializer.Log(part.Name + " [START]");
                bool o = (bool)part.GetType().GetMethod("Serialize")!.MakeGenericMethod(part.Serializable).Invoke(part, [this.parts, partsRevision, flags, serializer]);
                if (!o)
                {
                    serializer.Log(part.Name + " FAILED");
                    throw new SerializationException(part.Name + " failed to serialize!");
                }
                serializer.Log(part.Name + " [END]");
            }

            serializer.Log("THING " + Bytes.ToHex(UID) + " [END]");
        }
        
        public T? GetPart<T>(Part part) where T: ISerializable
        {
            return (T?) this.parts[part.GetIndex()];
        }

        public void SetPart<T>(Part part, T value) where T : ISerializable
        {
            this.parts[part.GetIndex()] = value;
        }
        
        public bool HasPart(Part part)
        {
            return this.parts[part.GetIndex()] != null;
        }

        public int GetAllocatedSize()
        {
            return BaseAllocationSize;
        }
    }
}