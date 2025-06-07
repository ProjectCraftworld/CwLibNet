using CwLibNet.IO;
using CwLibNet.Structs.Things.Parts;

using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things;
namespace CwLibNet.Enums;

public struct Part(string name, int index, int version, Type? serializable) : IEquatable<Part>
{

    public static readonly Dictionary<string, Part> Parts = new()
    {
        { "BODY", new Part("BODY", 0x0, PartHistory.BODY, typeof(PBody)) },
        { "JOINT", new Part("JOINT", 0x1, PartHistory.JOINT, typeof(PJoint)) },
        { "WORLD", new Part("WORLD", 0x2, PartHistory.WORLD, typeof(PWorld)) },
        { "RENDER_MESH", new Part("RENDER_MESH", 0x3, PartHistory.RENDER_MESH, typeof(PRenderMesh)) },
        { "POS", new Part("POS", 0x4, PartHistory.POS, typeof(PPos)) },
        { "TRIGGER", new Part("TRIGGER", 0x5, PartHistory.TRIGGER, typeof(PTrigger)) },
        { "TRIGGER_EFFECTOR", new Part("TRIGGER_EFFECTOR", 0x36, PartHistory.TRIGGER_EFFECTOR, null) },
        { "PAINT", new Part("PAINT", 0x37, PartHistory.PAINT, null) },
        { "YELLOWHEAD", new Part("YELLOWHEAD", 0x6, PartHistory.YELLOWHEAD, typeof(PYellowHead)) },
        { "AUDIO_WORLD", new Part("AUDIO_WORLD", 0x7, PartHistory.AUDIO_WORLD, typeof(PAudioWorld)) },
        { "ANIMATION", new Part("ANIMATION", 0x8, PartHistory.ANIMATION, typeof(PAnimation)) },
        { "GENERATED_MESH", new Part("GENERATED_MESH", 0x9, PartHistory.GENERATED_MESH, typeof(PGeneratedMesh)) },
        { "PARTICLE_CLUMP", new Part("PARTICLE_CLUMP", 0x38, PartHistory.PARTICLE_CLUMP, null) },
        { "PARTICLE_EMITTER", new Part("PARTICLE_EMITTER", 0x39, PartHistory.PARTICLE_EMITTER, null) },
        { "CAMERA_ZONE", new Part("CAMERA_ZONE", 0x3a, PartHistory.CAMERA_ZONE, null) },
        { "LEVEL_SETTINGS", new Part("LEVEL_SETTINGS", 0xA, PartHistory.LEVEL_SETTINGS, typeof(PLevelSettings)) },
        { "SPRITE_LIGHT", new Part("SPRITE_LIGHT", 0xB, PartHistory.SPRITE_LIGHT, typeof(PSpriteLight)) },
        { "KEYFRAMED_POSITION", new Part("KEYFRAMED_POSITION", 0x3b, PartHistory.KEYFRAMED_POSITION, null) },
        { "CAMERA", new Part("CAMERA", 0x3c, PartHistory.CAMERA, null) },
        { "SCRIPT_NAME", new Part("SCRIPT_NAME", 0xC, PartHistory.SCRIPT_NAME, typeof(PScriptName)) },
        { "CREATURE", new Part("CREATURE", 0xD, PartHistory.CREATURE, typeof(PCreature)) },
        { "CHECKPOINT", new Part("CHECKPOINT", 0xE, PartHistory.CHECKPOINT, typeof(PCheckpoint)) },
        { "STICKERS", new Part("STICKERS", 0xF, PartHistory.STICKERS, typeof(PStickers)) },
        { "DECORATIONS", new Part("DECORATIONS", 0x10, PartHistory.DECORATIONS, typeof(PDecorations)) },
        { "SCRIPT", new Part("SCRIPT", 0x11, PartHistory.SCRIPT, typeof(PScript)) },
        { "SHAPE", new Part("SHAPE", 0x12, PartHistory.SHAPE, typeof(PShape)) },
        { "EFFECTOR", new Part("EFFECTOR", 0x13, PartHistory.EFFECTOR, typeof(PEffector)) },
        { "EMITTER", new Part("EMITTER", 0x14, PartHistory.EMITTER, typeof(PEmitter)) },
        { "REF", new Part("REF", 0x15, PartHistory.REF, typeof(PRef)) },
        { "METADATA", new Part("METADATA", 0x16, PartHistory.METADATA, typeof(PMetadata)) },
        { "COSTUME", new Part("COSTUME", 0x17, PartHistory.COSTUME, typeof(PCostume)) },
        { "PARTICLE_EMITTER_2", new Part("PARTICLE_EMITTER_2", 0x3d, PartHistory.PARTICLE_EMITTER_2, null) },
        { "CAMERA_TWEAK", new Part("CAMERA_TWEAK", 0x18, PartHistory.CAMERA_TWEAK, typeof(PCameraTweak)) },
        { "SWITCH", new Part("SWITCH", 0x19, PartHistory.SWITCH, typeof(PSwitch)) },
        { "SWITCH_KEY", new Part("SWITCH_KEY", 0x1a, PartHistory.SWITCH_KEY, typeof(PSwitchKey)) },
        { "GAMEPLAY_DATA", new Part("GAMEPLAY_DATA", 0x1b, PartHistory.GAMEPLAY_DATA, typeof(PGameplayData)) },
        { "ENEMY", new Part("ENEMY", 0x1c, PartHistory.ENEMY, typeof(PEnemy)) },
        { "GROUP", new Part("GROUP", 0x1d, PartHistory.GROUP, typeof(PGroup)) },
        { "PHYSICS_TWEAK", new Part("PHYSICS_TWEAK", 0x1e, PartHistory.PHYSICS_TWEAK, typeof(PPhysicsTweak)) }, /*
        { "NPC", new Part("NPC", 0x1f, PartHistory.NPC, typeof(PNpc)) },
        { "SWITCH_INPUT", new Part("SWITCH_INPUT", 0x20, PartHistory.SWITCH_INPUT, typeof(PSwitchInput)) },
        { "MICROCHIP", new Part("MICROCHIP", 0x21, PartHistory.MICROCHIP, typeof(PMicrochip)) },
        { "MATERIAL_TWEAK", new Part("MATERIAL_TWEAK", 0x22, PartHistory.MATERIAL_TWEAK, typeof(PMaterialTweak)) },
        { "MATERIAL_OVERRIDE", new Part("MATERIAL_OVERRIDE", 0x23, PartHistory.MATERIAL_OVERRIDE, typeof(PMaterialOverride)) },
        { "INSTRUMENT", new Part("INSTRUMENT", 0x24, PartHistory.INSTRUMENT, typeof(PInstrument)) },
        { "SEQUENCER", new Part("SEQUENCER", 0x25, PartHistory.SEQUENCER, typeof(PSequencer)) },
        { "CONTROLINATOR", new Part("CONTROLINATOR", 0x26, PartHistory.CONTROLINATOR, typeof(PControlinator)) } */
    };

    /**
     * Minimum version required for this part
     * to be serialized.
     */
    public readonly int Version = version;

    /**
     * Index of this part.
     */
    public readonly int Index = index;

    /**
     * Serializable class reference
     */
    public readonly Type? Serializable = serializable;
    public string Name = name;

    public int GetIndex()
    {
        return Index;
    }

    public int GetVersion()
    {
        return Version;
    }

    public Type? GetSerializable()
    {
        return Serializable;
    }

    /**
     * Prepares a name used when serializating
     * this part using reflection.
     *
     * @return Field name
     */
    public string GetNameForReflection()
    {
        var name = Name.ToLower();
        string[] words = name.Split('_');

        for (var i = 0; i < words.Length; ++i)
        {
            var word = words[i];
            var str = word.ToCharArray();
            str[0] = char.ToUpper(word[0]);
            words[i] = new string(str);
        }

        name = "P" + string.Join("", words);

        /* Switch is a reserved keyword */
        // if (name == "switch")
        //     name = "switchBase";

        return name;
    }

    /**
     * Gets all the parts that can be serialized
     * by a set of flags.
     *
     * @param head    Head revision of resource
     * @param flags   Flags determing what parts can be serialized by a Thing
     * @param version Parts revision
     * @return Parts
     */
    public static Part[] FromFlags(int head, long flags, int version)
    {
        List<Part> parts = new(64);
        parts.AddRange(Parts.Values.Where(part => part.HasPart(head, flags, version)));
        return [.. parts];
    }

    public static bool operator ==(Part p1, Part p2)
    {
        return p1.Equals(p2);
    }

    public static bool operator !=(Part p1, Part p2)
    {
        return !(p1 == p2);
    }

    /**
     * Checks whether or not a thing contains this part
     * based on version and part flags.
     *
     * @param head    Head revision of resource
     * @param flags   Flags determing what parts can be serialized by a Thing
     * @param version Version determing what parts existed at this point
     * @return Whether a thing contains this part
     */
    public bool HasPart(int head, long flags, int version)
    {
        if ((head & 0xFFFF) >= 0x13c && Index is >= 0x36 and <= 0x3c)
            return false;
        if ((head & 0xFFFF) >= 0x18c && this == Parts["PARTICLE_EMITTER_2"])
            return false;
        if (head >> 0x10 >= 0x107 && this == Parts["CREATOR_ANIM"])
            return false;

        var index = Index;
        if (version < PartHistory.CREATOR_ANIM)
            return version >= Version
                   && ((1L << index) & flags) != 0;
        if (this == Parts["CREATOR_ANIM"])
            return ((1L << 0x29) & flags) != 0;
        if (head >> 0x10 < 0x107 && index > 0x28) index++;

        return version >= Version
               && ((1L << index) & flags) != 0;
    }

    /// <summary>
    /// (De)serializes this part to a stream.
    /// </summary>
    /// <param name="parts">?</param>
    /// <param name="version">Version determing what parts existed at this point</param>
    /// <param name="flags">Flags determing what parts can be serialized by this Thing</param>
    /// <param name="serializer">Instance of a serializer stream</param>
    /// <typeparam name="T">Type of part</typeparam>
    /// <returns>Whether the operation succeeded</returns>
    public bool Serialize<T>(ISerializable?[] parts, int version, long flags, Serializer serializer) where T : ISerializable
    {
        /* The Thing doesn't have this part, so it's "successful". */
        if (!HasPart(Serializer.GetCurrentSerializer().GetRevision().Head, flags, version))
            return true;

        var part = (T?)parts[Index];

        if (Serializable == null)
        {
            if (Serializer.IsWriting())
            {
                if (part != null) return false;
                else
                {
                    int tempZero = 0;
                    Serializer.Serialize(ref tempZero);
                    return true;
                }
            }
            if (!Serializer.IsWriting())
            {
                int tempZero = 0;
                Serializer.Serialize(ref tempZero);
                return tempZero == 0;
            }
            return false;
        }

        Serializer.Serialize(ref part);
        parts[Index] = part;

        return true;
    }

    public bool Equals(Part other)
    {
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is Part other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}