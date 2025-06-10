using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.World;

public class GlobalAudioSettings: ISerializable
{
    public const int BaseAllocationSize = 0x10;

    public int Reverb;
    public float MusicVolume = 1, SfxVolume = 1, BackgroundVolume = 1, DialogueVolume = 1;
    
    public float SfxReverbSend, DialogueReverbSend;
    
    public float MusicReverbSend, SfxFilterLp, DialogueFilterLp, MusicFilterLp;
    
    public float SfxFilterHp, DialogueFilterHp, MusicFilterHp;

    public string? AmbianceTrack;
    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer) {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;


        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        Serializer.Serialize(ref Reverb);
        if (version < 0x347)
            Serializer.Serialize(ref temp_int);
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