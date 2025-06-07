using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.World;

public class GlobalAudioSettings: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public int Reverb;
    public float MusicVolume = 1, SfxVolume = 1, BackgroundVolume = 1, DialogueVolume = 1;
    
    public float SfxReverbSend, DialogueReverbSend;
    
    public float MusicReverbSend, SfxFilterLp, DialogueFilterLp, MusicFilterLp;
    
    public float SfxFilterHp, DialogueFilterHp, MusicFilterHp;

    public string? AmbianceTrack;
    
    public void Serialize() {

        var version = Serializer.GetRevision().GetVersion();
        var subVersion = Serializer.GetRevision().GetSubVersion();

        Serializer.Serialize(ref Reverb);
        if (version < 0x347)
            Serializer.Serialize(ref 0);
        Serializer.Serialize(ref MusicVolume);
        Serializer.Serialize(ref SfxVolume);
        Serializer.Serialize(ref BackgroundVolume);
        Serializer.Serialize(ref DialogueVolume);

        if (version >= 0x342) {
            Serializer.Serialize(ref SfxReverbSend);
            Serializer.Serialize(ref DialogueReverbSend);
        }

        if (version >= 0x347) {
            Serializer.Serialize(ref MusicReverbSend);

            Serializer.Serialize(ref SfxFilterLp);
            Serializer.Serialize(ref DialogueFilterLp);
            Serializer.Serialize(ref MusicFilterLp);
        }

        if (subVersion >= 0xd8) {
            Serializer.Serialize(ref SfxFilterHp);
            Serializer.Serialize(ref DialogueFilterHp);
            Serializer.Serialize(ref MusicFilterHp);
        }

        if (subVersion >= 0xe2)
            Serializer.Serialize(ref AmbianceTrack);
    }

    public int GetAllocatedSize() { return BaseAllocationSize; }

}