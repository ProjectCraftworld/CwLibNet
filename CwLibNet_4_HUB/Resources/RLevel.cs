using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Level;
using CwLibNet.Structs.Profile;
using CwLibNet.Structs.Things;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types;
using CwLibNet.Types.Data;

namespace CwLibNet.Resources;

public class RLevel: Resource
{
    public const int BaseAllocationSize = 0x8;


    public Sha1?[] CrossPlayVitaDependencyHashes = [];

    public Thing? WorldThing;

    
    public PlayerRecord PlayerRecord = new();

    
    public List<InventoryItem> TutorialInventory = [];
    
    public List<CachedInventoryData> TutorialInventoryData = [];
    
    public bool TutorialInventoryActive;
    
    public bool CopiedFromSomeoneElse;

    // Vita
    
    public GUID? MusicGuid;
    public GUID? MusicSettingsGuid;

    public float[] MusicStemVolumes;

    
    public byte[]? DceUuid;

    
    public AdventureData? AdventureData;

    public RLevel()
    {
        var thing = new Thing(-1);

        var world = new PWorld();

        thing.SetPart(Part.Parts["BODY"], new PBody());
        thing.SetPart(Part.Parts["WORLD"], world);
        thing.SetPart(Part.Parts["POS"], new PPos());
        thing.SetPart(Part.Parts["EFFECTOR"], new PEffector());

        var script = new PScript
        {
            Instance =
            {
                Script = new ResourceDescriptor(19744, ResourceType.Script)
            }
        };

        thing.SetPart(Part.Parts["SCRIPT"], script);
        thing.SetPart(Part.Parts["GAMEPLAY_DATA"], new PGameplayData());

        world.Things.Add(thing);

        WorldThing = thing;
    }

    
    public override void Serialize(Serializer serializer)
    {
        var revision = serializer.GetRevision();
        var version = revision.GetVersion();
        var subVersion = revision.GetSubVersion();

        // Should move this to a common place at some point.
        if (serializer.IsWriting()) OnStartSave(revision);


        if (version > 0x3e6)
        {
            if (!serializer.IsWriting())
                CrossPlayVitaDependencyHashes = new Sha1?[serializer.GetInput().I32()];
            else
            {
                CrossPlayVitaDependencyHashes ??= [];
                serializer.GetOutput().I32(CrossPlayVitaDependencyHashes.Length);
            }
            for (var i = 0; i < CrossPlayVitaDependencyHashes.Length; ++i)
                CrossPlayVitaDependencyHashes[i] =
                    serializer.Sha1(CrossPlayVitaDependencyHashes[i]);
        }

        WorldThing = serializer.Reference(WorldThing);
        if (version > 0x213)
            PlayerRecord = serializer.Struct(PlayerRecord);

        if (version > 0x347)
            TutorialInventory = serializer.Arraylist(TutorialInventory);
        if (version > 0x35c)
            TutorialInventoryData = serializer.Arraylist(TutorialInventoryData);
        if (version > 0x39b)
            TutorialInventoryActive = serializer.Bool(TutorialInventoryActive);
        if (version > 0x3b0)
            CopiedFromSomeoneElse = serializer.Bool(CopiedFromSomeoneElse);

        if (revision.Has(Branch.Double11, 0x70))
        {
            MusicGuid = serializer.Guid(MusicGuid);
            MusicSettingsGuid = serializer.Guid(MusicSettingsGuid);
            MusicStemVolumes = serializer.Floatarray(MusicStemVolumes);
        }

        if (subVersion is > 0x34 and < 0x91)
            serializer.Bool(false);
        if (subVersion is > 0x34 and < 0xb3)
            serializer.Bool(false);
        if (subVersion is > 0x94 and < 0x12a)
            serializer.Bool(false); // savedThroughPusher

        DceUuid = subVersion switch
        {
            >= 0xf1 and <= 0xf9 => throw new SerializationException("Legacy adventure data not supported in " +
                                                                    "serialization!"),
            >= 0xfa => serializer.Bytearray(DceUuid),
            _ => DceUuid
        };

        switch (subVersion)
        {
            case >= 0x161 and < 0x169:
                serializer.Resource(null, ResourceType.AdventureSharedData);
                break;
            case >= 0x169:
                AdventureData = serializer.Reference(AdventureData);
                break;
        }

        // Should move this to a common place at some point.
        if (!serializer.IsWriting()) OnLoadFinished(revision);
    }

    private bool IsValidLevel()
    {
        // Who's serializing a level without a world thing?
        if (WorldThing == null) return false;

        var world = WorldThing.GetPart<PWorld>(Part.Parts["WORLD"]);

        // Just in case somebody's serializing a world without a world for whatever reason.
        return world != null;
    }

    
    public void OnLoadFinished(Revision revision)
    {
        if (!IsValidLevel()) return;
        var version = revision.GetVersion();
        var world = WorldThing?.GetPart<PWorld>(Part.Parts["WORLD"]);

        // If we're reading a file from after local positions stopped being serialized,
        // generate them.
        if (version < 0x341) return;
        foreach (var thing in world?.Things!)
        {
            var pos = thing?.GetPart<PPos>(Part.Parts["POS"]);
            if (pos == null) continue;

            if (thing?.Parent == null)
            {
                pos.LocalPosition = pos.WorldPosition!.Value;
                continue;
            }

            var parent = thing.Parent.GetPart<PPos>(Part.Parts["POS"]);

            // This generally shouldn't happen, but make sure to check it anyway
            if (parent == null) continue;

            Matrix4x4.Invert(parent.WorldPosition!.Value, out var inv);
            pos.LocalPosition = inv * pos.WorldPosition;
        }
    }

    
    public void OnStartSave(Revision revision)
    {
        if (!IsValidLevel()) return;
        var version = revision.GetVersion();
        var world = WorldThing?.GetPart<PWorld>(Part.Parts["WORLD"]);

        // If this is imported from a later version from JSON, the local matrices might not be
          // correct,
        // correct any that are identity matrices
        if (version < 0x341)
        {
            foreach (var thing in world?.Things!)
            {
                var pos = thing?.GetPart<PPos>(Part.Parts["POS"]);
                if (pos == null) continue;

                if (pos.LocalPosition!.Value.IsIdentity) continue;
                if (thing?.Parent == null)
                {
                    pos.LocalPosition = pos.WorldPosition!.Value;
                    continue;
                }

                var parent = thing.Parent.GetPart<PPos>(Part.Parts["POS"]);

                // This generally shouldn't happen, but make sure to check it anyway
                if (parent == null) continue;

                Matrix4x4.Invert(parent.WorldPosition!.Value, out var inv);
                pos.LocalPosition = inv * pos.WorldPosition;
            }
        }

        // Set the Parents for emitters
        if (version < 0x314)
        {
            foreach (var thing in world?.Things.OfType<Thing>().Where(thing => thing.HasPart(Part.Parts["EMITTER"])))
            {
                var emitter = thing.GetPart<PEmitter>(Part.Parts["EMITTER"]);
                emitter.ParentThing ??= thing.Parent;
            }
        }

        // Don't know the exact revision the scripts were removed, but deploy and below is a good
          // guess,
        // since that's when they overhauled the switch system, LBP2 already removes them so.
        if (version <= 0x2c3)
        {
            // Attach missing scripts to components if we're serializing to LBP1 from a later
              // version.
            var switchBaseScript = new ResourceDescriptor(42511,
                ResourceType.Script);
            var tweakJointScript = new ResourceDescriptor(19749,
                ResourceType.Script);
            var checkpointScript = new ResourceDescriptor(11757,
                ResourceType.Script);
            var triggerCollectScript = new ResourceDescriptor(11538,
                ResourceType.Script);
            var tweakEggScript = new ResourceDescriptor(27432, ResourceType.Script);
            var enemyScript = new ResourceDescriptor(31617, ResourceType.Script);
            var triggerParentScript = new ResourceDescriptor(17789,
                ResourceType.Script);
            var tweakParentScript = new ResourceDescriptor(39027,
                ResourceType.Script);
            var cameraZoneScript = new ResourceDescriptor(26580,
                ResourceType.Script);
            var tweakKeyScript = new ResourceDescriptor(52759, ResourceType.Script);
            var triggerCollectKeyScript = new ResourceDescriptor(17022,
                ResourceType.Script);
            var enemyWardScript = new ResourceDescriptor(43463, ResourceType.Script);
            var soundObjectScript = new ResourceDescriptor(31319, ResourceType.Script);

            var scoreboardScriptKey = new GUID(11599);
            var noJoinMarkerScriptKey = new GUID(39394);
            var gunScriptKey = new GUID(66090);
            var speechBubbleScriptKey = new GUID(18420);

            foreach (var thing in world.Things.OfType<Thing>())
            {
                // Remap any plans and gmats back to their original GUIDs in LBP1
                {
                    if (thing.PlanGuid != null)
                        thing.PlanGuid =
                            RGuidSubst.LBP2_TO_LBP1_PLANS.GetValueOrDefault(thing.PlanGuid.Value,
                                thing.PlanGuid.Value);

                    if (thing.HasPart(Part.Parts["EMITTER"]))
                    {
                        var emitter = thing.GetPart<PEmitter>(Part.Parts["EMITTER"]);
                        if (emitter.Plan != null && emitter.Plan.IsGUID())
                        {
                            var guid = emitter.Plan.GetGUID();
                            guid = RGuidSubst.LBP2_TO_LBP1_PLANS.GetValueOrDefault(guid!.Value, guid.Value);
                            emitter.Plan = new ResourceDescriptor(guid.Value, ResourceType.Plan);
                        }
                    }

                    if (thing.HasPart(Part.Parts["GROUP"]))
                    {
                        var group = thing.GetPart<PGroup>(Part.Parts["GROUP"]);
                        if (group.PlanDescriptor != null && group.PlanDescriptor.IsGUID())
                        {
                            var guid = group.PlanDescriptor.GetGUID();
                            guid = RGuidSubst.LBP2_TO_LBP1_PLANS.GetValueOrDefault(guid.Value, guid.Value);
                            group.PlanDescriptor = new ResourceDescriptor(guid.Value, ResourceType.Plan);
                        }
                    }

                    // A bunch of gmats got remapped in LBP2
                    if (thing.HasPart(Part.Parts["GENERATED_MESH"]))
                    {
                        var mesh = thing.GetPart<PGeneratedMesh>(Part.Parts["GENERATED_MESH"]);
                        if (mesh.GfxMaterial != null && mesh.GfxMaterial.IsGUID())
                        {
                            var guid = mesh.GfxMaterial.GetGUID();
                            guid = RGuidSubst.LBP2_TO_LBP1_GMATS.GetValueOrDefault(guid!.Value, guid.Value);
                            mesh.GfxMaterial = new ResourceDescriptor(guid.Value, ResourceType.GfxMaterial);
                        }
                    }
                }

                if (thing.HasPart(Part.Parts["SCRIPT"]))
                {
                    var script = thing.GetPart<PScript>(Part.Parts["SCRIPT"]);

                    // Remove the DialogueListGibberishFile from any script (magic mouths)
                    // if they exist, since this will always trigger a dependencies error in LBP1
                    if (script.Is(speechBubbleScriptKey))
                    {
                        script.Instance.UnsetField("DialogueListGibberishFile");
                        script.Instance.UnsetField("DialogueListFile");
                    }

                    // Deploy has custom gun, but LBP1 doesn't, will have to remove
                    // these fields conditionally, don't know the exact revision, so we'll go
                    // with 0x272, LBP1 retails readonly revision
                    if (version <= 0x272 && script.Is(gunScriptKey))
                    {
                        script.Instance.UnsetField("Plan");
                        script.Instance.UnsetField("PlanIcon");
                    }
                }

                // Fixup creature brains
                if (thing.HasPart(Part.Parts["ENEMY"]))
                {
                    var enemy = thing.GetPart<PEnemy>(Part.Parts["ENEMY"]);
                    if (enemy.PartType == EnemyPart.BRAIN)
                    {
                        if (!thing.HasPart(Part.Parts["SCRIPT"]))
                            thing.SetPart(Part.Parts["SCRIPT"], new PScript(enemyScript));

                        // Attach the prize bubble script to the life sources attached to this enemy
                        var creature = thing.GetPart<PCreature>(Part.Parts["CREATURE"]);
                        if (creature is { LifeSourceList: not null })
                        {
                            foreach (var lifeSource in creature.LifeSourceList)
                            {
                                if (lifeSource == null) continue;

                                if (lifeSource.HasPart(Part.Parts["SCRIPT"])) continue;
                                var script = new PScript(triggerCollectScript);
                                script.Instance.AddField("CreatureThing", thing);
                                lifeSource.SetPart(Part.Parts["SCRIPT"], script);
                            }
                        }
                    }
                }

                // Only set the script instances if they don't already exist
                if (thing.HasPart(Part.Parts["SCRIPT"])) continue;
                {
                    // Sound names got moved to a native field in later versions
                    if (thing.HasPart(Part.Parts["AUDIO_WORLD"]))
                    {
                        var sfx = thing.GetPart<PAudioWorld>(Part.Parts["AUDIO_WORLD"]);
                        sfx.TriggerBySwitch = HasSwitchInput(thing);
                        var script = new PScript(soundObjectScript);
                        if (sfx.SoundNames != null)
                            script.Instance.AddField("SoundNames", sfx.SoundNames.Value);
                        thing.SetPart(Part.Parts["SCRIPT"], script);
                    }

                    else if (thing.IsEnemyWard())
                        thing.SetPart(Part.Parts["SCRIPT"], new PScript(enemyWardScript));

                    // Prize bubbles and score bubbles used a trigger collect script in LBP1
                    else if (thing.HasPart(Part.Parts["GAMEPLAY_DATA"]))
                    {
                        if (thing.IsPrizeBubble())
                            thing.SetPart(Part.Parts["SCRIPT"], new PScript(tweakEggScript));
                        else if (thing.IsScoreBubble())
                            thing.SetPart(Part.Parts["SCRIPT"], new PScript(triggerCollectScript));
                    }

                    // Checkpoint tweakable scripts
                    else if (thing.HasPart(Part.Parts["CHECKPOINT"]))
                        thing.SetPart(Part.Parts["SCRIPT"], new PScript(checkpointScript));

                    // Joint tweakable scripts
                    else if (thing.HasPart(Part.Parts["JOINT"]))
                        thing.SetPart(Part.Parts["SCRIPT"], new PScript(tweakJointScript));

                    else if (thing.HasPart(Part.Parts["SWITCH_KEY"]))
                        thing.SetPart(Part.Parts["SCRIPT"], new PScript(tweakKeyScript));

                    // Switch base script get removed in later versions, I assume because it
                    // just gets automatically added internally?
                    else if (thing.HasPart(Part.Parts["SWITCH"]))
                        thing.SetPart(Part.Parts["SCRIPT"], new PScript(switchBaseScript));

                    // Both camera types
                    else if (thing.HasPart(Part.Parts["CAMERA_TWEAK"]))
                        thing.SetPart(Part.Parts["SCRIPT"], new PScript(cameraZoneScript));

                    // Fixup scoreboard, no join posts, and anything with triggers
                    else if (thing.Parent != null)
                    {
                        // Handle level keys
                        if (thing.Parent.IsKey() && thing.HasPart(Part.Parts["TRIGGER"]))
                        {
                            thing.SetPart(Part.Parts["SCRIPT"], new PScript(triggerCollectKeyScript));
                            continue;
                        }

                        var script = thing.Parent.GetPart<PScript>(Part.Parts["SCRIPT"]);
                        if (script == null) continue;
                        if (script.Is(scoreboardScriptKey))
                        {
                            if (thing.HasPart(Part.Parts["TRIGGER"]))
                                thing.SetPart(Part.Parts["SCRIPT"], new PScript(triggerParentScript));
                            else if (thing.HasPart(Part.Parts["CHECKPOINT"]))
                                thing.SetPart(Part.Parts["SCRIPT"], new PScript(checkpointScript));
                            else
                                thing.SetPart(Part.Parts["SCRIPT"], new PScript(tweakParentScript));

                        }
                        else if (thing.HasPart(Part.Parts["TRIGGER"]))
                            thing.SetPart(Part.Parts["SCRIPT"], new PScript(triggerParentScript));
                    }
                }
            }
        }

        // Attach tweak joint scripts to components if we're serialized to LBP1

        switch (version)
        {
            // If we're writing a file that was originally from after LBP1, we need to
            // put the backdropPlan as a PRef component
            case < 0x321 when world.BackdropPlan != null && world.Backdrop != null:
            {
                PRef pref;
                if (!world.Backdrop.HasPart(Part.Parts["REF"]))
                {
                    pref = new PRef();

                    world.Backdrop.SetPart(Part.Parts["REF"], pref);
                }
                else pref = world.Backdrop.GetPart<PRef>(Part.Parts["REF"]);

                pref.ChildrenSelectable = false;
                pref.StripChildren = true;
                pref.Plan = world.BackdropPlan;
                break;
            }
            case >= 0x321 when world.BackdropPlan == null && world.Backdrop != null:
            {
                var pref = world.Backdrop.GetPart<PRef>(Part.Parts["REF"]);
                if (pref == null) return;
                world.BackdropPlan = pref.Plan;
                world.Backdrop.SetPart<PRef>(Part.Parts["REF"], null);
                break;
            }
        }
    }

    public bool HasSwitchInput(Thing target)
    {
        if (!IsValidLevel()) return false;
        var world = WorldThing.GetPart<PWorld>(Part.Parts["WORLD"]);
        foreach (var thing in world.Things.OfType<Thing>().Where(thing => thing.HasPart(Part.Parts["SWITCH"])))
        {
            var switchBase = thing.GetPart<PSwitch>(Part.Parts["SWITCH"]);
            if (switchBase?.Outputs == null) continue;
            foreach (var output in switchBase.Outputs)
            {
                if (output.TargetList == null) continue;
                if (output.TargetList.Any(switchTarget => switchTarget.Thing == target))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private List<Thing> GetAllReferences(List<Thing> things, Thing thing)
    {
        var world = WorldThing?.GetPart<PWorld>(Part.Parts["WORLD"]);
        if (!things.Contains(thing)) things.Add(thing);
        foreach (var worldThing in from worldThing in world.Things.OfType<Thing>() where worldThing != thing where !worldThing.HasPart(Part.Parts["WORLD"]) && !things.Contains(worldThing) where worldThing.Parent == thing || (thing.GroupHead != null && (worldThing.GroupHead == thing.GroupHead || thing.GroupHead == worldThing.GroupHead)) select worldThing)
        {
            GetAllReferences(things, worldThing);
        }
        return things;
    }

    public Dictionary<string, RPlan> GetPalettes(String name, Revision revision,
                                              byte compressionFlags, bool includeChildren)
    {
        Dictionary<String, RPlan> plans = new();
        var world = WorldThing.GetPart<PWorld>(Part.Parts["WORLD"]);
        Thing.SerializeWorldThing = false;
        foreach (var thing in world.Things)
        {
            if (thing == null || thing.HasPart(Part.Parts["WORLD"])) continue;
            var metadata = thing.GetPart<PMetadata>(Part.Parts["METADATA"]);
            if (metadata == null) continue;
            if (includeChildren)
            {
                var things =
                    GetAllReferences([], thing).ToArray();
                plans.Add(name + "_" + thing.Uid + ".plan", new RPlan(revision,
                    compressionFlags,
                    things, metadata));
            }
            else
                plans.Add(name + "_" + thing.Uid + ".plan", new RPlan(revision,
                    compressionFlags,
                    thing, metadata));
        }
        Thing.SerializeWorldThing = true;
        return plans;
    }

    public int GetNextUid()
    {
        return ++WorldThing.GetPart<PWorld>(Part.Parts["WORLD"])!.ThingUidCounter;
    }

    public void AddPlan(RPlan plan)
    {
        var things = plan.GetThings();
        var world = WorldThing.GetPart<PWorld>(Part.Parts["WORLD"]);
        foreach (var thing in things)
        {
            if (thing == null) continue;
            thing.Uid = GetNextUid();
            world.Things.Add(thing);
        }
    }

    public byte[] ToPlan()
    {
        var plan = new RPlan
        {
            Revision = new Revision(0x272, 0x4c44, 0x0017),
            CompressionFlags = 0x7
        };
        List<Thing> things = [];
        var world = WorldThing.GetPart<PWorld>(Part.Parts["WORLD"]);
        foreach (var thing in world.Things)
        {
            if (thing == WorldThing) continue;
            if (thing == world.Backdrop) continue;

            things.Add(thing);
        }

        plan.SetThings(things.ToArray());

        plan.InventoryData = new InventoryItemDetails
        {
            Type = [],
            Icon = new ResourceDescriptor(2551, ResourceType.Texture),
            UserCreatedDetails = new UserCreatedDetails("World Export",
                "Exported " +
                "world")
        };


        return SerializedResource.Compress(plan.Build());
    }

    
    public override int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }

    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        // 16MB buffer for generation of levels, since the allocated size will get
        // stuck in a recursive loop until I fix it.
        var serializer = new Serializer(0x1000000, revision, compressionFlags);
        serializer.Struct(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Level,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
    }
}
