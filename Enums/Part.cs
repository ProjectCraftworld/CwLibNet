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
    }

    public sealed class PartBody
    {
        private readonly Part version;
        private readonly Part index;
        private readonly Part serializable;

        /// <summary>
        /// Index of this part
        /// </summary>
        /// <param name="index">Part index</param>
        /// <returns>PartBody</returns>        

        /// <summary>
        /// The minimum version required for this part to be serialized
        /// </summary>
        /// <param name="version">Part version</param>
        /// <returns>PartBody</returns>   

        /// <summary>
        /// The serializable class represented by this part
        /// </summary>
        /// <param name="serializable">Part serializable</param>
        /// <returns>PartBody</returns>    

        PartBody(int index, int version, Part serializable)
        {
            this.index = (Part)index;
            this.version = (Part)version;
            this.serializable = (Part)serializable;
        }

        public Part getIndex()
        {
            return this.index;
        }

        public Part getVersion()
        {
            return this.version;
        }
        
        public Part getSerializable()
        {
            return this.serializable;
        }

        /// <summary>
        /// Prepares a name used when serializing this part using reflection
        /// </summary>
        ///<returns>Field name</returns>
    }
}