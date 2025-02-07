using CwLibNet;
using CwLibNet.IO;
using CwLibNet.Resources;

namespace Cwlib.Enums
{
    public enum ResourceType
    {
        // INVALID(null, 0, "unknown/", "")
        INVALID,
        // TEXTURE("TEX", 1, "textures/", ".tex")
        TEXTURE,
        // GTF_TEXTURE("GTF", 1, "textures/", ".tex")
        GTF_TEXTURE,
        // MESH("MSH", 2, RMesh.class, "meshes/", ".mol")
        MESH,
        // PIXEL_SHADER(null, 3, "shaders/fragment/", ".fpo")
        PIXEL_SHADER,
        // VERTEX_SHADER(null, 4, "shaders/vertex/", ".vpo")
        VERTEX_SHADER,
        // ANIMATION("ANM", 5, RAnimation.class, "animations/", ".anim")
        ANIMATION,
        // GUID_SUBSTITUTION("GSB", 6, "guid_subst/", ".gsub")
        GUID_SUBSTITUTION,
        // GFX_MATERIAL("GMT", 7, RGfxMaterial.class, "gfx_materials/", ".gmat")
        GFX_MATERIAL,
        // SPU_ELF(null, 8, "spu/", ".sbu")
        SPU_ELF,
        // LEVEL("LVL", 9, RLevel.class, "levels/", ".bin")
        LEVEL,
        // // Could be anything really, but generally will refer to
        // FILENAME(null, 10, "text/", ".txt")
        FILENAME,
        // // either FSB or BIK
        // SCRIPT("FSH", 11, "scripts/", ".ff")
        SCRIPT,
        // SETTINGS_CHARACTER("CHA", 12, "character_settings/", ".cha")
        SETTINGS_CHARACTER,
        // FILE_OF_BYTES(null, 13, "raw_data/", ".raw")
        FILE_OF_BYTES,
        // SETTINGS_SOFT_PHYS("SSP", 14, "softphys_settings/", ".sph")
        SETTINGS_SOFT_PHYS,
        // FONTFACE("FNT", 15, "fonts/", ".fnt")
        FONTFACE,
        // MATERIAL("MAT", 16, RMaterial.class, "physics_materials/", ".mat")
        MATERIAL,
        // DOWNLOADABLE_CONTENT("DLC", 17, RDLC.class, "dlc/", ".dlc")
        DOWNLOADABLE_CONTENT,
        // EDITOR_SETTINGS(null, 18, "editor_settings/", ".edset")
        EDITOR_SETTINGS,
        // JOINT("JNT", 19, RJoint.class, "joints/", ".joint")
        JOINT,
        // GAME_CONSTANTS("CON", 20, "constants/", ".con")
        GAME_CONSTANTS,
        // POPPET_SETTINGS("POP", 21, "poppet_settings/", ".pop")
        POPPET_SETTINGS,
        // CACHED_LEVEL_DATA("CLD", 22, "cached/levels/", ".cld")
        CACHED_LEVEL_DATA,
        // SYNCED_PROFILE("PRF", 23, RSyncedProfile.class, "profiles/synced/", ".pro")
        SYNCED_PROFILE,
        // BEVEL("BEV", 24, RBevel.class, "bevels/", ".bev")
        BEVEL,
        // GAME("GAM", 25, "game/", ".game")
        GAME,
        // SETTINGS_NETWORK("NWS", 26, "network_settings/", ".nws")
        SETTINGS_NETWORK,
        // PACKS("PCK", 27, RPacks.class, "packs/", ".pck")
        PACKS,
        // BIG_PROFILE("BPR", 28, RBigProfile.class, "profiles/big/", ".bpr")
        BIG_PROFILE,
        // SLOT_LIST("SLT", 29, RSlotList.class, "slots/", ".slt")
        SLOT_LIST,
        // TRANSLATION(null, 30, "translations/", ".trans")
        TRANSLATION,
        // ADVENTURE_CREATE_PROFILE("ADC", 31, RAdventureCreateProfile.class, "adventure_data/create/", ".adc")
        ADVENTURE_CREATE_PROFILE,
        // LOCAL_PROFILE("IPR", 32, RLocalProfile.class, "profiles/local/", ".ipr")
        LOCAL_PROFILE,
        // LIMITS_SETTINGS("LMT", 33, "limits/", ".lmt")
        LIMITS_SETTINGS,
        // TUTORIALS("TUT", 34, "tutorials/", ".tut")
        TUTORIALS,
        // GUID_LIST("GLT", 35, "guids/", ".glst")
        GUID_LIST,
        // AUDIO_MATERIALS("AUM", 36, "audio_materials/", ".aum")
        AUDIO_MATERIALS,
        // SETTINGS_FLUID("SSF", 37, "fluid_settings/", ".flu")
        SETTINGS_FLUID,
        // PLAN("PLN", 38, RPlan.class, "plans/", ".plan")
        PLAN,
        // TEXTURE_LIST("TXL", 39, "texture_lists/", ".yuv")
        TEXTURE_LIST,
        // MUSIC_SETTINGS("MUS", 40, "music_settings/", ".mus")
        MUSIC_SETTINGS,
        // MIXER_SETTINGS("MIX", 41, "mixer_settings/", ".mix")
        MIXER_SETTINGS,
        // REPLAY_CONFIG("REP", 42, "replays/", ".rep")
        REPLAY_CONFIG,
        // PALETTE("PAL", 43, RPalette.class, "palettes/", ".pal")
        PALETTE,
        // STATIC_MESH("SMH", 44, "static_meshes/", ".smh")
        STATIC_MESH,
        // ANIMATED_TEXTURE("ATX", 45, "animated_textures/", ".atx")
        ANIMATED_TEXTURE,
        // VOIP_RECORDING("VOP", 46, "audio/", ".vop")
        VOIP_RECORDING,
        // PINS("PIN", 47, RPins.class, "pins/", ".pin")
        PINS,
        // INSTRUMENT("INS", 48, RInstrument.class, "instruments/", ".rinst")
        INSTRUMENT,
        // SAMPLE(null, 49, "samples/", ".smp")
        SAMPLE,
        // OUTFIT_LIST("OFT", 50, "outfits/", ".oft")
        OUTFIT_LIST,
        // PAINT_BRUSH("PBR", 51, "paintbrushes/", ".pbr")
        PAINT_BRUSH,
        // THING_RECORDING("REC", 52, "recordings/", ".rec")
        THING_RECORDING,
        // PAINTING("PTG", 53, "paintings/", ".ptg")
        PAINTING,
        // QUEST("QST", 54, "quests/", ".qst")
        QUEST,
        // ANIMATION_BANK("ABK", 55, "animations/banks/", ".abnk")
        ANIMATION_BANK,
        // ANIMATION_SET("AST", 56, "animations/sets/", ".aset")
        ANIMATION_SET,
        // SKELETON_MAP("SMP", 57, "skeletons/maps/", ".smap")
        SKELETON_MAP,
        // SKELETON_REGISTRY("SRG", 58, "skeletons/registries/", ".sreg")
        SKELETON_REGISTRY,
        // SKELETON_ANIM_STYLES("SAS", 59, "skeleton/animation_styles/", ".sas")
        SKELETON_ANIM_STYLES,
        // CROSSPLAY_VITA(null, 60, "crossplay_data/", ".cpv")
        CROSSPLAY_VITA,
        // STREAMING_CHUNK("CHK", 61, "streaming_chunks/", ".chk")
        STREAMING_CHUNK,
        // ADVENTURE_SHARED_DATA("ADS", 62, "adventure_data/shared/", ".ads")
        ADVENTURE_SHARED_DATA,
        // ADVENTURE_PLAY_PROFILE("ADP", 63, "adventure_data/play_profiles/", ".adp")
        ADVENTURE_PLAY_PROFILE,
        // ANIMATION_MAP("AMP", 64, "animations/maps/", ".amap")
        ANIMATION_MAP,
        // CACHED_COSTUME_DATA("CCD", 65, "cached_costume_data/", ".ccd")
        CACHED_COSTUME_DATA,
        // DATA_LABELS("DLA", 66, "datalabels/", ".dla")
        DATA_LABELS,
        // ADVENTURE_MAPS("ADM", 67, "adventure_data/maps/", ".adm")
        ADVENTURE_MAPS,
        // // Custom Toolkit/Workbench resources
        // BONE_SET("BST", 128, RBoneSet.class, "bonesets/", ".boneset")
        BONE_SET 

        // --------------------
        // TODO enum body members
        // // SHADER_CACHE("CGC", 129, RShaderCache.class, "shader_caches/", ".shadercache"),
        // // SCENE_GRAPH("SCE", 130, RSceneGraph.class, "scenes/", ".sg"),
        // // TYPE_LIBRARY("LIB", 131, "type_library/", ".lib");
        // private final String header;
        // private final int value;
        // private final Class<? extends Serializable> compressable;
        // private final String folder;
        // private final String extension;
        // ResourceType(String magic, int value, Class<? extends Serializable> clazz, String folder, String extension) {
        //     this.header = magic;
        //     this.value = value;
        //     this.folder = folder;
        //     this.compressable = clazz;
        //     this.extension = extension;
        // }
        // ResourceType(String magic, int value, String folder, String extension) {
        //     this.header = magic;
        //     this.value = value;
        //     this.compressable = null;
        //     this.folder = folder;
        //     this.extension = extension;
        // }
        // public String getHeader() {
        //     return this.header;
        // }
        // public Integer getValue() {
        //     return this.value;
        // }
        // public Class<? extends Serializable> getCompressable() {
        //     return this.compressable;
        // }
        // public String getFolder() {
        //     return this.folder;
        // }
        // public String getExtension() {
        //     return this.extension;
        // }
        // /**
        //  * Attempts to get a valid ResourceType from a 3-byte magic header.
        //  *
        //  * @param value Magic header
        //  * @return Resource type
        //  */
        // public static ResourceType fromMagic(String value) {
        //     if (value.length() > 3)
        //         value = value.substring(0, 3);
        //     value = value.toUpperCase();
        //     for (ResourceType type : ResourceType.values()) {
        //         if (type.header == null)
        //             continue;
        //         if (type.header.equals(value))
        //             return type;
        //     }
        //     return ResourceType.INVALID;
        // }
        // /**
        //  * Attempts to get a valid ResourceType from the value index.
        //  *
        //  * @param value Resource value index
        //  * @return Resource type
        //  */
        // public static ResourceType fromType(int value) {
        //     for (ResourceType type : ResourceType.values()) {
        //         if (type.value == value)
        //             return type;
        //     }
        //     return ResourceType.INVALID;
        // }
        // --------------------
    }

    public sealed class ResourceBody
    {
        private readonly ResourceType header;
        private readonly ResourceType part;
        private readonly ResourceType<T> compressable;
        private readonly ResourceType folder;
        private readonly String extension;
        private readonly ResourceType extension;
        
        ResourceBody(String magic, int value, Class<? extends Serializable> clazz, String folder, String extension)
        {
            this.header = (ResourceType)magic;
            this.part = (ResourceType)value;
            this.compressable = clazz == null? null : (ResourceType)value;
            this.folder = (ResourceType)folder;
            this.extension = (ResourceType)extension; 
        }

        public String getHeader()
        {
            return this.header;
        }

        public ResourceType getValue()
        {
            return this.value;
        }
    }
}