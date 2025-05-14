using CwLibNet.IO;
using CwLibNet.IO.Serializer;

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
    
    public void Serialize(Serializer serializer) {

        var version = serializer.GetRevision().GetVersion();
        var subVersion = serializer.GetRevision().GetSubVersion();

        Reverb = serializer.S32(Reverb);
        if (version < 0x347)
            serializer.F32(0);
        MusicVolume = serializer.F32(MusicVolume);
        SfxVolume = serializer.F32(SfxVolume);
        BackgroundVolume = serializer.F32(BackgroundVolume);
        DialogueVolume = serializer.F32(DialogueVolume);

        if (version >= 0x342) {
            SfxReverbSend = serializer.F32(SfxReverbSend);
            DialogueReverbSend = serializer.F32(DialogueReverbSend);
        }

        if (version >= 0x347) {
            MusicReverbSend = serializer.F32(MusicReverbSend);

            SfxFilterLp = serializer.F32(SfxFilterLp);
            DialogueFilterLp = serializer.F32(DialogueFilterLp);
            MusicFilterLp = serializer.F32(MusicFilterLp);
        }

        if (subVersion >= 0xd8) {
            SfxFilterHp = serializer.F32(SfxFilterHp);
            DialogueFilterHp = serializer.F32(DialogueFilterHp);
            MusicFilterHp = serializer.F32(MusicFilterHp);
        }

        if (subVersion >= 0xe2)
            AmbianceTrack = serializer.Str(AmbianceTrack);
    }

    public int GetAllocatedSize() { return BaseAllocationSize; }

}