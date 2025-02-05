using CwLibNet.IO;
using CwLibNet.IO.Serialization;

namespace CwLibNet.Structs.Instrument
{
    public class Sample : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;
        public int baseNote = 48; // MIDI NOTE NUMBERS -> https://www.inspiredacoustics.com/en/MIDI_note_numbers_and_center_frequencies
        // MIDI NOTE NUMBERS -> https://www.inspiredacoustics.com/en/MIDI_note_numbers_and_center_frequencies
        public float baseBpm = 149.5F;
        // MIDI NOTE NUMBERS -> https://www.inspiredacoustics.com/en/MIDI_note_numbers_and_center_frequencies
        public Serialize pitched = true;
        // MIDI NOTE NUMBERS -> https://www.inspiredacoustics.com/en/MIDI_note_numbers_and_center_frequencies
        public Serialize fitBpm;
        // MIDI NOTE NUMBERS -> https://www.inspiredacoustics.com/en/MIDI_note_numbers_and_center_frequencies
        public float fineTune;
        // MIDI NOTE NUMBERS -> https://www.inspiredacoustics.com/en/MIDI_note_numbers_and_center_frequencies
        public override void Serialize(Serializer serializer)
        {
            baseNote = serializer.Serialize(baseNote);
            baseBpm = serializer.Serialize(baseBpm);
            pitched = serializer.Serialize(pitched);

            // Don't know why this is here twice? They both write to the same variable.
            fitBpm = serializer.Serialize(fitBpm);
            fitBpm = serializer.Serialize(fitBpm);
            fineTune = serializer.Serialize(fineTune);
        }

        // MIDI NOTE NUMBERS -> https://www.inspiredacoustics.com/en/MIDI_note_numbers_and_center_frequencies
        // Don't know why this is here twice? They both write to the same variable.
        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE;
        }
    }
}