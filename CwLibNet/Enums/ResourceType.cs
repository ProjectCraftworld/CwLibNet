using System.Reflection;
using CwLibNet.Resources;

namespace CwLibNet.Enums
{
    public struct ResourceType
    {
        public static readonly ResourceType
            Invalid = new(null, 0, "unknown/", ""),
            Texture = new("TEX", 1, "textures/", ".tex"),
            GtfTexture = new("GTF", 1, "textures/", ".tex"),
            Mesh = new("MSH", 2, /*typeof(RMesh),*/ "meshes/", ".mol"),
            PixelShader = new(null, 3, "shaders/fragment/", ".fpo"),
            VertexShader = new(null, 4, "shaders/vertex/", ".vpo"),
            Animation = new("ANM", 5, /* typeof(RAnimation), */ "animations/", ".anim"),
            GuidSubstitution = new("GSB", 6, "guid_subst/", ".gsub"),
            GfxMaterial = new("GMT", 7 /*, typeof(RGfxMaterial) */, "gfx_materials/", ".gmat"),
            SpuElf = new(null, 8, "spu/", ".sbu"),
            Level = new("LVL", 9, /* typeof(RLevel), */"levels/", ".bin"),
            Filename =
                new(null, 10, "text/",
                    ".txt"), // Could be anything really, but generally will refer to either FSB or BIK
            Script = new("FSH", 11, "scripts/", ".ff"),
            SettingsCharacter = new("CHA", 12, "character_settings/", ".cha"),
            FileOfBytes = new(null, 13, "raw_data/", ".raw"),
            SettingsSoftPhys = new("SSP", 14, "softphys_settings/", ".sph"),
            FontFace = new("FNT", 15, typeof(RFontFace), "fonts/", ".fnt"),
            Material = new("MAT", 16, /* typeof(RMaterial), */"physics_materials/", ".mat"),
            DownloadableContent = new("DLC", 17, typeof(RDLC), "dlc/", ".dlc"),
            EditorSettings = new(null, 18, "editor_settings/", ".edset"),
            Joint = new("JNT", 19, /* typeof(RJoint), */ "joints/", ".joint"),
            GameConstants = new("CON", 20, "constants/", ".con"),
            PoppetSettings = new("POP", 21, "poppet_settings/", ".pop"),
            CachedLevelData = new("CLD", 22, "cached/levels/", ".cld"),
            // SyncedProfile = new("PRF", 23, typeof(RSyncedProfile), "profiles/synced/", ".pro"),
            Bevel = new("BEV", 24, /* typeof(RBevel), */ "bevels/", ".bev"),
            Game = new("GAM", 25, "game/", ".game"),
            SettingsNetwork = new("NWS", 26, "network_settings/", ".nws"),
            // Packs = new("PCK", 27, typeof(RPacks), "packs/", ".pck"),
            // BigProfile = new("BPR", 28, typeof(RBigProfile), "profiles/big/", ".bpr"),
            SlotList = new("SLT", 29, typeof(RSlotList), "slots/", ".slt"),
            Translation = new(null, 30, typeof(RTranslationTable), "translations/", ".trans"),
            AdventureCreateProfile = new("ADC", 31, typeof(RAdventureCreateProfile), "adventure_data/create/", ".adc"),
            // LocalProfile = new("IPR", 32, typeof(RLocalProfile), "profiles/local/", ".ipr"),
            LimitsSettings = new("LMT", 33, "limits/", ".lmt"),
            Tutorials = new("TUT", 34, "tutorials/", ".tut"),
            GuidList = new("GLT", 35, "guids/", ".glst"),
            AudioMaterials = new("AUM", 36, "audio_materials/", ".aum"),
            SettingsFluid = new("SSF", 37, "fluid_settings/", ".flu"),
            Plan = new("PLN", 38, typeof(RPlan), "plans/", ".plan"),
            TextureList = new("TXL", 39, "texture_lists/", ".yuv"),
            MusicSettings = new("MUS", 40, "music_settings/", ".mus"),
            MixerSettings = new("MIX", 41, "mixer_settings/", ".mix"),
            ReplayConfig = new("REP", 42, "replays/", ".rep"),
            Palette = new("PAL", 43, typeof(RPalette), "palettes/", ".pal"),
            StaticMesh = new("SMH", 44, "static_meshes/", ".smh"),
            AnimatedTexture = new("ATX", 45, "animated_textures/", ".atx"),
            VoipRecording = new("VOP", 46, "audio/", ".vop"),
            Pins = new("PIN", 47, typeof(RPins), "pins/", ".pin"),
            // Instrument = new("INS", 48, typeof(RInstrument), "instruments/", ".rinst"),
            Sample = new(null, 49, "samples/", ".smp"),
            OutfitList = new("OFT", 50, "outfits/", ".oft"),
            PaintBrush = new("PBR", 51, "paintbrushes/", ".pbr"),
            ThingRecording = new("REC", 52, "recordings/", ".rec"),
            Painting = new("PTG", 53, "paintings/", ".ptg"),
            Quest = new("QST", 54, "quests/", ".qst"),
            AnimationBank = new("ABK", 55, "animations/banks/", ".abnk"),
            AnimationSet = new("AST", 56, "animations/sets/", ".aset"),
            SkeletonMap = new("SMP", 57, "skeletons/maps/", ".smap"),
            SkeletonRegistry = new("SRG", 58, "skeletons/registries/", ".sreg"),
            SkeletonAnimStyles = new("SAS", 59, "skeleton/animation_styles/", ".sas"),
            CrossplayVita = new(null, 60, "crossplay_data/", ".cpv"),
            StreamingChunk = new("CHK", 61, "streaming_chunks/", ".chk"),
            AdventureSharedData = new("ADS", 62, "adventure_data/shared/", ".ads"),
            AdventurePlayProfile = new("ADP", 63, "adventure_data/play_profiles/", ".adp"),
            AnimationMap = new("AMP", 64, "animations/maps/", ".amap"),
            CachedCostumeData = new("CCD", 65, "cached_costume_data/", ".ccd"),
            DataLabels = new("DLA", 66, "datalabels/", ".dla"),
            AdventureMaps = new("ADM", 67, "adventure_data/maps/", ".adm"); //,
            // Custom Toolkit/Workbench resources
            // BoneSet = new("BST", 128, typeof(RBoneSet), "bonesets/", ".boneset");
        // SHADER_CACHE = new("CGC", 129, RShaderCache.class, "shader_caches/", ".shadercache"),
        // SCENE_GRAPH = new("SCE", 130, RSceneGraph.class, "scenes/", ".sg"),
        // TYPE_LIBRARY = new("LIB", 131, "type_library/", ".lib");
    
        public string? Header { get; }
        public int Value { get; }
        public Type? Compressable { get; }
        public string Folder { get; }
        public string Extension { get; }
    
        public ResourceType(string? magic, int value, Type type, string folder, string extension)
        {
            this.Header = magic;
            this.Value = value;
            this.Folder = folder;
            this.Compressable = type;
            this.Extension = extension;
        }

        public ResourceType(string? magic, int value, string folder, string extension)
        {
            this.Header = magic;
            this.Value = value;
            this.Compressable = null;
            this.Folder = folder;
            this.Extension = extension;
        }

        /**
         * Attempts to get a valid ResourceType from a 3-byte magic header.
         *
         * @param value Magic header
         * @return Resource type
         */
        public static ResourceType FromMagic(string value)
        {
            if (value.Length > 3)
                value = value[..3];
            value = value.ToUpper();
            return (ResourceType)(typeof(ResourceType).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.FieldType == typeof(ResourceType))
                .Where(p => ((ResourceType)(p.GetValue(null) ?? Invalid)).Header == value).Select(e => e.GetValue(null))
                .FirstOrDefault(Invalid) ?? Invalid);
        }

        /// <summary>
        /// Attempts to get a valid ResourceType from the value index.
        /// </summary>
        /// <param name="value">Resource value index</param>
        /// <returns>Resource type</returns>
        public static ResourceType FromType(int value)
        {
            // TODO
            return Invalid;
        }
    }
}