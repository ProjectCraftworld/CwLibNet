using System.Text;
using CwLibNet.Extensions;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Types;
using CwLibNet.Util;

namespace CwLibNet.Resources;

public class RTranslationTable : Resource
{
    /**
     * Calculates LAMS key ID from translation tag.
     *
     * @param tag Translation tag
     * @return Hashed key from translation tag
     */
    public static long MakeLamsKeyID(string? tag)
    {
        tag ??= "";
        long v0 = 0, v1 = 0xC8509800L;
        for (int i = 32; i > 0; --i) {
            long c = 0x20;
            if ((i - 1) < tag.Length)
                c = tag[i - 1];
            v0 = v0 * 0x1b + c;
        }
        
        if (tag.Length > 32) {
            v1 = 0;
            for (int i = 64; i > 32; --i) {
                long c = 0x20;
                if ((i - 1) < tag.Length)
                    c = tag[i - 1];
                v1 = v1 * 0x1b + c;
            }
        }
        
        return (v0 + v1 * 0xDEADBEEFL) & 0xFFFFFFFFL;
    }

    public RTranslationTable(Dictionary<long, string?> translationTable)
    {
        this.lookup = translationTable;
    }

    public RTranslationTable(byte[]? data)
    {
        // Legacy RTranslationTable is just a text file.
        if (Bytes.ToShortBE(data) == ((ushort) 0xFEFF)) {
            string?[] lines = Encoding.Unicode.GetString(data).Split('\n');
            for (int i = 0; i < lines.Length; i+=2)
            {
                this.lookup.Add(MakeLamsKeyID(lines[i]), lines[i + 1]);
            }
            return;
        }
        
        MemoryInputStream stream = new MemoryInputStream(data);
        int dataLength = stream.GetLength();

        int count = stream.I32();

        int tableOffset = 0x4 + (count * 0x8);
        this.lookup = new Dictionary<long, string?>(count);
        for (int i = 0; i < count; ++i) {
            long key = stream.U32();
            int stringStart = stream.I32();

            int oldOffset = stream.GetOffset();

            stream.Seek(tableOffset + stringStart + 2, SeekMode.Begin);
            while (stream.GetOffset() != dataLength)
                if (stream.U16() == 0xFEFF)
                    break;
            
            int stringEnd = stream.GetOffset();
            if (stringEnd != dataLength)
                stringEnd -= 2;
            
            stream.Seek(tableOffset + stringStart, SeekMode.Begin);

            string? text = Encoding.BigEndianUnicode.GetString(stream.Bytes(stringEnd - stream.GetOffset())).Trim((char)0xFEFF);
            this.lookup.Add(key, text);

            stream.Seek(oldOffset, SeekMode.Begin);
        }
    }
        
    private Dictionary<long, string?> lookup = new();
        
    public string? Translate(String tag) {
        return this.Translate(RTranslationTable.MakeLamsKeyID(tag));
    }
        
    public string? Translate(int key) { return this.Translate(key & 0xFFFFFFFF); }
        
    public string? Translate(long key)
    {
        return this.lookup.ContainsKey(key) ? this.lookup[key] : null;
    }
        
    public void Add(String key, String value) { 
        this.lookup.Add(RTranslationTable.MakeLamsKeyID(key), value);
    }

    public void Add(long key, String value) {
        this.lookup.Add(key, value);
    }

        
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        return Build();
    }

    public override int GetAllocatedSize()
    {
        throw new NotImplementedException();
    }

    public override void Serialize(Serializer serializer)
    {
        throw new NotImplementedException();
    }

    public SerializationData Build()
    {
        int stringTableSize = this.lookup.Values
            .Select(element => (Encoding.BigEndianUnicode.GetBytes(element).Length + 1) * 2)
            .Aggregate(0, (total, element) => total + element) - 2;

        Dictionary<string, int> offsets = new Dictionary<string, int>();
        MemoryOutputStream stringTable = new MemoryOutputStream(stringTableSize);
        MemoryOutputStream keyTable = new MemoryOutputStream((this.lookup.Count * 8) + 4);
        keyTable.I32(this.lookup.Count);

        List<long> keys = this.lookup.Keys.ToList();
        keys.Sort((a, z) => a.CompareUnsigned(z));
        foreach (long key in keys)
        {
            string? value = this.lookup[key];
            keyTable.U32(key);
            if (offsets.ContainsKey(value))
            {
                keyTable.I32(offsets[value]);
                continue;
            }

            int offset = stringTable.GetOffset();
            offsets.Add(value, offset);

            keyTable.I32(offset);

            stringTable.U16(0xFEFF);
            stringTable.Wstr(value, Encoding.BigEndianUnicode.GetBytes(value).Length / 2);
        }

        stringTable.Shrink();
        return new SerializationData(Bytes.Combine(keyTable.GetBuffer(), stringTable.GetBuffer()));
    }
}