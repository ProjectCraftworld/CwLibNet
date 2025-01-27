using CwLibNet.IO;

namespace CwLibNet.Types;

public struct Part
{
    /**
     * Minimum version required for this part
     * to be serialized.
     */
    public readonly int Version;

    /**
     * Index of this part.
     */
    public readonly int Index;

    /**
     * Serializable class reference
     */
    public readonly Type Serializable;

    /**
     * Creates a part.
     *
     * @param index        Index of this part
     * @param version      The minimum version required for this part to be serialized
     * @param serializable The serializable class represented by this part
     */
    public Part(int index, int version, Type serializable)
    {
        this.Index = index;
        this.Version = version;
        this.Serializable = serializable;
    }

    public int GetIndex()
    {
        return this.Index;
    }

    public int GetVersion()
    {
        return this.Version;
    }

    public Type GetSerializable()
    {
        return this.Serializable;
    }

    /**
     * Prepares a name used when serializating
     * this part using reflection.
     *
     * @return Field name
     */
    public string GetNameForReflection()
    {
        string name = this.name().toLowerCase();
        string[] words = name.split("_");

        for (int i = 0; i < words.length; ++i)
        {
            string word = words[i];
            words[i] = Character.toUpperCase(word.charAt(0)) + word.substring(1);
        }

        name = "P" + string.join("", words);

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
        ArrayList<Part> parts = new ArrayList<>(64);
        for (Part part : Part.values())
        {
            if (part.hasPart(head, flags, version))
                parts.add(part);
        }
        return parts.toArray(Part[]::new);
    }

    /**
     * Checks whether or not a thing contains this part
     * based on version and part flags.
     *
     * @param head    Head revision of resource
     * @param flags   Flags determing what parts can be serialized by a Thing
     * @param version Version determing what parts existed at this point
     * @return Whether or not a thing contains this part
     */
    public bool HasPart(int head, long flags, int version)
    {
        if ((head & 0xFFFF) >= 0x13c && (this.Index >= 0x36 && this.Index <= 0x3c))
            return false;
        if ((head & 0xFFFF) >= 0x18c && this == Part.PARTICLE_EMITTER_2)
            return false;
        if ((head >> 0x10) >= 0x107 && this == Part.CREATOR_ANIM)
            return false;

        int index = this.Index;
        if (version >= PartHistory.CREATOR_ANIM)
        {
            if (this == Part.CREATOR_ANIM)
                return ((1L << 0x29) & flags) != 0;
            if (((head >> 0x10) < 0x107) && index > 0x28) index++;
        }

        return version >= this.Version
               && (((1L << index) & flags) != 0);
    }
    
    /// <summary>
    /// (De)serializes this part to a stream.
    /// </summary>
    /// <param name="parts">?</param>
    /// <param name="version">Version determing what parts existed at this point</param>
    /// <param name="flags">Flags determing what parts can be serialized by this Thing</param>
    /// <param name="serializer">Instance of a serializer stream</param>
    /// <typeparam name="T">Type of part</typeparam>
    /// <returns>Whether or not the operation succeeded</returns>
    public bool Serialize<T>(ISerializable[] parts, int version, long flags, Serializer serializer) where T : ISerializable
    {
        /* The Thing doesn't have this part, so it's "successful" */
        if (!this.HasPart(serializer.getRevision().getHead(), flags, version))
            return true;

        T part = (T) parts[this.Index];

        if (this.Serializable == null)
        {
            if (serializer.isWriting())
            {
                if (part != null) return false;
                else
                {
                    serializer.i32(0);
                    return true;
                }
            }
            else if (!serializer.isWriting())
            {
                return serializer.i32(0) == 0;
            }
            return false;
        }

        parts[this.Index] = serializer.reference(part, (Class<T>) this.Serializable);

        return true;
    }
}