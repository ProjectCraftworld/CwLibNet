using System.Runtime.Serialization;
using CwLibNet.Enums;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types.Data;
using CwLibNet.Util;
using ISerializable = CwLibNet.IO.ISerializable;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
using static CwLibNet.IO.Serializer.Serializer;

namespace CwLibNet.Structs.Things;

public class Thing : ISerializable
{
    public static bool SerializeWorldThing = true;

    public const int BaseAllocationSize = 0x100;

    public string? Name;

    public int Uid = 1;
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
        Uid = uid;
    }
    
    public void Serialize(CwLibNet.IO.Serializer.Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        var maxPartsRevision = PartHistory.STREAMING_HINT;
        if (version <= 0x3e2)
            maxPartsRevision = PartHistory.CONTROLINATOR;
        if (version <= 0x33a)
            maxPartsRevision = PartHistory.MATERIAL_OVERRIDE;
        if (version <= 0x2c3)
            maxPartsRevision = PartHistory.MATERIAL_TWEAK;
        if (version <= 0x272)
            maxPartsRevision = PartHistory.GROUP;

        // Test serialization marker.
        if (revision.Has(Branch.Mizuki, (int)Revisions.MZ_SCENE_GRAPH)) Serializer.Serialize(ref Name);
        else if (version >= (int)Revisions.THING_TEST_MARKER || revision.Has(Branch.Leerdammer, (int)Revisions.LD_TEST_MARKER))
        {
            Serializer.LogMessage("TEST_SERIALISATION_MARKER");
            var testMarker = 0xAA;
            Serializer.Serialize(ref testMarker);
            if (testMarker != 0xaa)
                throw new SerializationException("Test serialization marker is invalid, something has gone terribly wrong!");
        }

        if (version < 0x1fd)
        {
            if (Serializer.IsWriting())
                Serializer.SerializeReference(SerializeWorldThing ? World : null);
            else
                Serializer.Serialize(ref World);
        }
        if (version < 0x27f)
        {
            Serializer.Serialize(ref Parent);
            Serializer.Serialize(ref Uid);
        }
        else
        {
            Serializer.Serialize(ref Uid);
            Serializer.Serialize(ref Parent);
        }

        Serializer.Serialize(ref GroupHead);

        switch (version)
        {
            case >= 0x1c7:
                Serializer.Serialize(ref OldEmitter);
                break;
            case >= 0x1a6 and < 0x1bc:
                Serializer.SerializeArray<PJoint>(null, true);
                break;
        }

        if ((version >= 0x214 && !revision.IsToolkit()) || revision.Before(Branch.Mizuki, (int)Revisions.MZ_SCENE_GRAPH))
        {
            Serializer.Serialize(ref ChangedBy);
            Serializer.Serialize(ref ChangedBy);
        }

        if (version < 0x341)
        {
            if (version > 0x21a)
                Serializer.Serialize(ref IsStamping);
            if (version >= 0x254)
                Serializer.Serialize(ref PlanGuid);
            if (version >= 0x2f2)
                Serializer.Serialize(ref Hidden);
        }
        else
        {
            if (version >= 0x254)
                Serializer.Serialize(ref PlanGuid);

            if (version >= 0x341)
            {
                if (revision.Has(Branch.Double11, 0x62))
                {
                    Serializer.Serialize(ref Flags);
                }
                else
                {
                    var byteFlags = (byte) Flags;
                    Serializer.Serialize(ref byteFlags);
                    Flags = byteFlags;
                }
            }
            if (subVersion >= 0x110)
                Serializer.Serialize(ref ExtraFlags);
        }

        var isCompressed = version >= 0x297 || revision.Has(Branch.Leerdammer,
            (int)Revisions.LD_RESOURCES);

        var partsRevision = PartHistory.STREAMING_HINT;
        long flags = -1;

        if (Serializer.IsWriting())
        {
            Serializer.LogMessage("GENERATING FLAGS");
            Part? lastPart = null;
            if (isCompressed) flags = 0;
            foreach (var part in Part.Parts.Values)
            {
                var index = part.GetIndex();
                switch (version)
                {
                    case >= 0x13c when index is >= 0x36 and <= 0x3c:
                    case >= 0x18c when index == 0x3d:
                        continue;
                }

                if (subVersion >= 0x107 && index == 0x3e) continue;
                else if (index == 0x3e)
                {
                    if (parts[index] != null)
                    {
                        flags |= 1L << 0x29;
                        lastPart = part;
                    }
                    continue;
                }

                if (parts[index] == null) continue;
                // Offset due to PCreatorAnim
                if (subVersion < 0x107 && index > 0x28) index++;

                flags |= 1L << index;

                lastPart = part;
            }
            partsRevision = lastPart?.GetVersion() ?? 0;
        }

        if (Serializer.IsWriting())
        {
            if (partsRevision > maxPartsRevision)
                partsRevision = maxPartsRevision;
        }

        Serializer.Serialize(ref partsRevision);
        if (isCompressed)
        {
            // Serializer.LogMessage("FLAGS");
            Serializer.Serialize(ref flags);
        }

        // I have no idea why they did this
        if (version == 0x13c) partsRevision += 7;

        var partsToSerialize = Part.FromFlags(revision.Head, flags, partsRevision);
        Serializer.LogMessage(string.Join(' ', partsToSerialize));

        foreach (var part in partsToSerialize)
        {
            Serializer.LogMessage(part.Name + " [START]");
            var o = (bool)part.GetType().GetMethod("Serialize")!.MakeGenericMethod(part.Serializable).Invoke(part, [parts, partsRevision, flags, serializer]);
            if (!o)
            {
                Serializer.LogMessage(part.Name + " FAILED");
                throw new SerializationException(part.Name + " failed to serialize!");
            }
            Serializer.LogMessage(part.Name + " [END]");
        }

        Serializer.LogMessage("THING " + Bytes.ToHex(Uid) + " [END]");
    }
        
    public T? GetPart<T>(Part part) where T: ISerializable
    {
        return (T?) parts[part.GetIndex()];
    }

    public void SetPart<T>(Part part, T? value) where T: ISerializable
    {
        parts[part.GetIndex()] = value;
    }
        
    public bool HasPart(Part part)
    {
        return parts[part.GetIndex()] != null;
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
    
    public bool IsEnemyWard()
    {
        var enemyWardMeshKey = new GUID(43456);
        var enemyWardPlanKey = new GUID(53114);

        if (enemyWardPlanKey == PlanGuid) return true;
        if (HasPart(Part.Parts["GROUP"]))
        {
            var group = GetPart<PGroup>(Part.Parts["GROUP"]);
            if (group.PlanDescriptor != null && group.PlanDescriptor.IsGUID() && enemyWardPlanKey.Equals(group.PlanDescriptor.GetGUID()))
                return true;
        }

        if (HasPart(Part.Parts["RENDER_MESH"]))
        {
            var mesh = GetPart<PRenderMesh>(Part.Parts["RENDER_MESH"]);
            if (mesh.Mesh != null && mesh.Mesh.IsGUID())
                return enemyWardMeshKey.Equals(mesh.Mesh.GetGUID());
        }

        return false;
    }

    public bool IsKey()
    {
        var data = GetPart<PGameplayData>(Part.Parts["GAMEPLAY_DATA"]);
        if (data == null) return false;

        if (data.GameplayType == GameplayPartType.LEVEL_KEY) return true;
        if (data.KeyLink != null) return true;

        var keyPlanGuid = new GUID(31738);
        var keyMeshGuid = new GUID(3763);

        if (keyPlanGuid.Equals(PlanGuid)) return true;
        if (HasPart(Part.Parts["GROUP"]))
        {
            var group = GetPart<PGroup>(Part.Parts["GROUP"]);
            if (group.PlanDescriptor != null && group.PlanDescriptor.IsGUID() && keyPlanGuid.Equals(group.PlanDescriptor.GetGUID()))
                return true;
        }

        // Not entirely sure if having the mesh is entirely just criteria, but it's probably fine
        if (HasPart(Part.Parts["RENDER_MESH"]))
        {
            var mesh = GetPart<PRenderMesh>(Part.Parts["RENDER_MESH"]);
            if (mesh.Mesh != null && mesh.Mesh.IsGUID())
                return keyMeshGuid.Equals(mesh.Mesh.GetGUID());
        }

        return false;
    }

    public bool IsScoreBubble()
    {
        var data = GetPart<PGameplayData>(Part.Parts["GAMEPLAY_DATA"]);
        if (data == null) return false;

        // This is only relevant in LBP2 onwards
        if (data.GameplayType == GameplayPartType.SCORE_BUBBLE) return true;
        if (data.EggLink != null) return false;

        var scoreBubblePlanGuid = new GUID(31733);
        var scoreBubbleMeshGuid = new GUID(3753);

        if (scoreBubblePlanGuid.Equals(PlanGuid)) return true;
        if (HasPart(Part.Parts["GROUP"]))
        {
            var group = GetPart<PGroup>(Part.Parts["GROUP"]);
            if (group.PlanDescriptor != null && group.PlanDescriptor.IsGUID() && scoreBubblePlanGuid.Equals(group.PlanDescriptor.GetGUID()))
                return true;
        }

        // Not entirely sure if having the mesh is entirely just criteria, but it's probably fine
        if (HasPart(Part.Parts["RENDER_MESH"]))
        {
            var mesh = GetPart<PRenderMesh>(Part.Parts["RENDER_MESH"]);
            if (mesh.Mesh != null && mesh.Mesh.IsGUID())
                return scoreBubbleMeshGuid.Equals(mesh.Mesh.GetGUID());
        }

        return false;
    }

    public bool IsPrizeBubble()
    {
        var data = GetPart<PGameplayData>(Part.Parts["GAMEPLAY_DATA"]);
        if (data == null) return false;

        // This is only relevant in LBP2 onwards
        if (data.GameplayType == GameplayPartType.PRIZE_BUBBLE) return true;
        if (data.EggLink != null) return true;

        var prizePlanGuid = new GUID(31743);
        var prizeMeshGuid = new GUID(21180);

        if (prizePlanGuid.Equals(PlanGuid)) return true;
        if (HasPart(Part.Parts["GROUP"]))
        {
            var group = GetPart<PGroup>(Part.Parts["GROUP"]);
            if (group.PlanDescriptor != null && group.PlanDescriptor.IsGUID() && prizePlanGuid.Equals(group.PlanDescriptor.GetGUID()))
                return true;
        }

        // Not entirely sure if having the mesh is entirely just criteria, but it's probably fine
        if (HasPart(Part.Parts["RENDER_MESH"]))
        {
            var mesh = GetPart<PRenderMesh>(Part.Parts["RENDER_MESH"]);
            if (mesh.Mesh != null && mesh.Mesh.IsGUID())
                return prizeMeshGuid.Equals(mesh.Mesh.GetGUID());
        }

        return false;
    }


}