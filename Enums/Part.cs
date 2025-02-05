using CwLibNet.IO;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums
{
    public enum Part
    {
        // BODY(0x0, PartHistory.BODY, PBody.class)
        BODY,
        // JOINT(0x1, PartHistory.JOINT, PJoint.class)
        JOINT,
        // WORLD(0x2, PartHistory.WORLD, PWorld.class)
        WORLD,
        // RENDER_MESH(0x3, PartHistory.RENDER_MESH, PRenderMesh.class)
        RENDER_MESH,
        // POS(0x4, PartHistory.POS, PPos.class)
        POS,
        // TRIGGER(0x5, PartHistory.TRIGGER, PTrigger.class)
        TRIGGER,
        // @Deprecated
        // TRIGGER_EFFECTOR(0x36, PartHistory.TRIGGER_EFFECTOR, null)
        TRIGGER_EFFECTOR,
        // @Deprecated
        // PAINT(0x37, PartHistory.PAINT, null)
        PAINT,
        // YELLOWHEAD(0x6, PartHistory.YELLOWHEAD, PYellowHead.class)
        YELLOWHEAD,
        // AUDIO_WORLD(0x7, PartHistory.AUDIO_WORLD, PAudioWorld.class)
        AUDIO_WORLD,
        // ANIMATION(0x8, PartHistory.ANIMATION, PAnimation.class)
        ANIMATION,
        // GENERATED_MESH(0x9, PartHistory.GENERATED_MESH, PGeneratedMesh.class)
        GENERATED_MESH,
        // @Deprecated
        // PARTICLE_CLUMP(0x38, PartHistory.PARTICLE_CLUMP, null)
        PARTICLE_CLUMP,
        // @Deprecated
        // PARTICLE_EMITTER(0x39, PartHistory.PARTICLE_EMITTER, null)
        PARTICLE_EMITTER,
        // @Deprecated
        // CAMERA_ZONE(0x3a, PartHistory.CAMERA_ZONE, null)
        CAMERA_ZONE,
        // LEVEL_SETTINGS(0xA, PartHistory.LEVEL_SETTINGS, PLevelSettings.class)
        LEVEL_SETTINGS,
        // SPRITE_LIGHT(0xB, PartHistory.SPRITE_LIGHT, PSpriteLight.class)
        SPRITE_LIGHT,
        // @Deprecated
        // KEYFRAMED_POSITION(0x3b, PartHistory.KEYFRAMED_POSITION, null)
        KEYFRAMED_POSITION,
        // @Deprecated
        // CAMERA(0x3c, PartHistory.CAMERA, null)
        CAMERA,
        // SCRIPT_NAME(0xC, PartHistory.SCRIPT_NAME, PScriptName.class)
        SCRIPT_NAME,
        // CREATURE(0xD, PartHistory.CREATURE, PCreature.class)
        CREATURE,
        // CHECKPOINT(0xE, PartHistory.CHECKPOINT, PCheckpoint.class)
        CHECKPOINT,
        // STICKERS(0xF, PartHistory.STICKERS, PStickers.class)
        STICKERS,
        // DECORATIONS(0x10, PartHistory.DECORATIONS, PDecorations.class)
        DECORATIONS,
        // SCRIPT(0x11, PartHistory.SCRIPT, PScript.class)
        SCRIPT,
        // SHAPE(0x12, PartHistory.SHAPE, PShape.class)
        SHAPE,
        // EFFECTOR(0x13, PartHistory.EFFECTOR, PEffector.class)
        EFFECTOR,
        // EMITTER(0x14, PartHistory.EMITTER, PEmitter.class)
        EMITTER,
        // REF(0x15, PartHistory.REF, PRef.class)
        REF,
        // METADATA(0x16, PartHistory.METADATA, PMetadata.class)
        METADATA,
        // COSTUME(0x17, PartHistory.COSTUME, PCostume.class)
        COSTUME,
        // @Deprecated
        // PARTICLE_EMITTER_2(0x3d, PartHistory.PARTICLE_EMITTER_2, null)
        PARTICLE_EMITTER_2,
        // CAMERA_TWEAK(0x18, PartHistory.CAMERA_TWEAK, PCameraTweak.class)
        CAMERA_TWEAK,
        // SWITCH(0x19, PartHistory.SWITCH, PSwitch.class)
        SWITCH,
        // SWITCH_KEY(0x1a, PartHistory.SWITCH_KEY, PSwitchKey.class)
        SWITCH_KEY,
        // GAMEPLAY_DATA(0x1b, PartHistory.GAMEPLAY_DATA, PGameplayData.class)
        GAMEPLAY_DATA,
        // ENEMY(0x1c, PartHistory.ENEMY, PEnemy.class)
        ENEMY,
        // GROUP(0x1d, PartHistory.GROUP, PGroup.class)
        GROUP,
        // PHYSICS_TWEAK(0x1e, PartHistory.PHYSICS_TWEAK, PPhysicsTweak.class)
        PHYSICS_TWEAK,
        // NPC(0x1f, PartHistory.NPC, PNpc.class)
        NPC,
        // SWITCH_INPUT(0x20, PartHistory.SWITCH_INPUT, PSwitchInput.class)
        SWITCH_INPUT,
        // MICROCHIP(0x21, PartHistory.MICROCHIP, PMicrochip.class)
        MICROCHIP,
        // MATERIAL_TWEAK(0x22, PartHistory.MATERIAL_TWEAK, PMaterialTweak.class)
        MATERIAL_TWEAK,
        // MATERIAL_OVERRIDE(0x23, PartHistory.MATERIAL_OVERRIDE, PMaterialOverride.class)
        MATERIAL_OVERRIDE,
        // INSTRUMENT(0x24, PartHistory.INSTRUMENT, PInstrument.class)
        INSTRUMENT,
        // SEQUENCER(0x25, PartHistory.SEQUENCER, PSequencer.class)
        SEQUENCER,
        // CONTROLINATOR(0x26, PartHistory.CONTROLINATOR, PControlinator.class)
        CONTROLINATOR,
        // POPPET_POWERUP(0x27, PartHistory.POPPET_POWERUP, PPoppetPowerup.class)
        POPPET_POWERUP,
        // POCKET_ITEM(0x28, PartHistory.POCKET_ITEM, PPocketItem.class)
        POCKET_ITEM,
        // @Deprecated
        // CREATOR_ANIM(0x3e, PartHistory.CREATOR_ANIM, null)
        CREATOR_ANIM,
        // TRANSITION(0x29, PartHistory.TRANSITION, PTransition.class)
        TRANSITION,
        // FADER(0x2a, PartHistory.FADER, PFader.class)
        FADER,
        // ANIMATION_TWEAK(0x2b, PartHistory.ANIMATION_TWEAK, PAnimationTweak.class)
        ANIMATION_TWEAK,
        // WIND_TWEAK(0x2c, PartHistory.WIND_TWEAK, PWindTweak.class)
        WIND_TWEAK,
        // POWER_UP(0x2d, PartHistory.POWER_UP, PPowerUp.class)
        POWER_UP,
        // HUD_ELEM(0x2e, PartHistory.HUD_ELEM, PHudElem.class)
        HUD_ELEM,
        // TAG_SYNCHRONIZER(0x2f, PartHistory.TAG_SYNCHRONIZER, PTagSynchroniser.class)
        TAG_SYNCHRONIZER,
        // WORMHOLE(0x30, PartHistory.WORMHOLE, PWormhole.class)
        WORMHOLE,
        // QUEST(0x31, PartHistory.QUEST, PQuest.class)
        QUEST,
        // CONNECTOR_HOOK(0x32, PartHistory.CONNECTOR_HOOK, PConnectorHook.class)
        CONNECTOR_HOOK,
        // ATMOSPHERIC_TWEAK(0x33, PartHistory.ATMOSHPERIC_TWEAK, PAtmosphericTweak.class)
        ATMOSPHERIC_TWEAK,
        // STREAMING_DATA(0x34, PartHistory.STREAMING_DATA, PStreamingData.class)
        STREAMING_DATA,
        // STREAMING_HINT(0x35, PartHistory.STREAMING_HINT, PStreamingHint.class)
        STREAMING_HINT 

        // --------------------
        // TODO enum body members
        // /**
        //  * Minimum version required for this part
        //  * to be serialized.
        //  */
        // private final int version;
        // /**
        //  * Index of this part.
        //  */
        // private final int index;
        // /**
        //  * Serializable class reference
        //  */
        // private final Class<?> serializable;
        // /**
        //  * Creates a part.
        //  *
        //  * @param index        Index of this part
        //  * @param version      The minimum version required for this part to be serialized
        //  * @param serializable The serializable class represented by this part
        //  */
        // Part(int index, int version, Class<?> serializable) {
        //     this.index = index;
        //     this.version = version;
        //     this.serializable = serializable;
        // }
        // public int getIndex() {
        //     return this.index;
        // }
        // public int getVersion() {
        //     return this.version;
        // }
        // public Class<?> getSerializable() {
        //     return this.serializable;
        // }
        // /**
        //  * Prepares a name used when serializating
        //  * this part using reflection.
        //  *
        //  * @return Field name
        //  */
        // public String getNameForReflection() {
        //     String name = this.name().toLowerCase();
        //     String[] words = name.split("_");
        //     for (int i = 0; i < words.length; ++i) {
        //         String word = words[i];
        //         words[i] = Character.toUpperCase(word.charAt(0)) + word.substring(1);
        //     }
        //     name = "P" + String.join("", words);
        //     /* Switch is a reserved keyword */
        //     // if (name == "switch")
        //     //     name = "switchBase";
        //     return name;
        // }
        // /**
        //  * Gets all the parts that can be serialized
        //  * by a set of flags.
        //  *
        //  * @param head    Head revision of resource
        //  * @param flags   Flags determing what parts can be serialized by a Thing
        //  * @param version Parts revision
        //  * @return Parts
        //  */
        // public static Part[] fromFlags(int head, long flags, int version) {
        //     ArrayList<Part> parts = new ArrayList<>(64);
        //     for (Part part : Part.values()) {
        //         if (part.hasPart(head, flags, version))
        //             parts.add(part);
        //     }
        //     return parts.toArray(Part[]::new);
        // }
        // /**
        //  * Checks whether or not a thing contains this part
        //  * based on version and part flags.
        //  *
        //  * @param head    Head revision of resource
        //  * @param flags   Flags determing what parts can be serialized by a Thing
        //  * @param version Version determing what parts existed at this point
        //  * @return Whether or not a thing contains this part
        //  */
        // public boolean hasPart(int head, long flags, int version) {
        //     if ((head & 0xFFFF) >= 0x13c && (this.index >= 0x36 && this.index <= 0x3c))
        //         return false;
        //     if ((head & 0xFFFF) >= 0x18c && this == Part.PARTICLE_EMITTER_2)
        //         return false;
        //     if ((head >> 0x10) >= 0x107 && this == Part.CREATOR_ANIM)
        //         return false;
        //     int index = this.index;
        //     if (version >= PartHistory.CREATOR_ANIM) {
        //         if (this == Part.CREATOR_ANIM)
        //             return ((1L << 0x29) & flags) != 0;
        //         if (((head >> 0x10) < 0x107) && index > 0x28)
        //             index++;
        //     }
        //     return version >= this.version && (((1L << index) & flags) != 0);
        // }
        // /**
        //  * (De)serializes this part to a stream.
        //  *
        //  * @param <T>        Type of part
        //  * @param thing      Thing that potentially contains this part
        //  * @param version    Version determing what parts existed at this point
        //  * @param flags      Flags determing what parts can be serialized by this Thing
        //  * @param serializer Instance of a serializer stream
        //  * @return Whether or not the operation succeeded
        //  */
        // @SuppressWarnings("unchecked")
        // public <T extends Serializable> boolean serialize(Serializable[] parts, int version, long flags, Serializer serializer) {
        //     /* The Thing doesn't have this part, so it's "successful" */
        //     if (!this.hasPart(serializer.getRevision().getHead(), flags, version))
        //         return true;
        //     T part = (T) parts[this.index];
        //     if (this.serializable == null) {
        //         if (serializer.isWriting()) {
        //             if (part != null)
        //                 return false;
        //             else {
        //                 serializer.i32(0);
        //                 return true;
        //             }
        //         } else if (!serializer.isWriting()) {
        //             return serializer.i32(0) == 0;
        //         }
        //         return false;
        //     }
        //     parts[this.index] = serializer.reference(part, (Class<T>) this.serializable);
        //     return true;
        // }
        // --------------------
    }
}