using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO.Streams;
using CwLibNet.Util;

namespace CwLibNet.Types.Archives;

public class FileArchive: Fart
{
    public FileArchive(string file): base(file, ArchiveType.FARC)
    {
        byte[] fatTable;

        try
        {
            var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fs.Length < 0x8)
            {
                fs.Close();
                throw new SerializationException("Invalid FARC, size is less than minimum" +
                                                 " of 8 " +
                                                 "bytes!");
            }

            // archive.seek(archive.length() - 0x8); // Seek to the bottom of the archive to read
            // entry count and magic.

            var entryCountBuffer = new byte[4];
            fs.Seek(fs.Length - 8, SeekOrigin.Begin);
            fs.Read(entryCountBuffer, 0, 4);
            var entryCount = Bytes.ToIntegerBE(entryCountBuffer);
            Entries = new Fat[entryCount];
            var magicBuffer = new byte[4];
            fs.Seek(fs.Length - 0x4, SeekOrigin.Begin);
            fs.Read(magicBuffer, 0, 0x4);
            if (!magicBuffer.SequenceEqual("FARC"u8.ToArray())  /* FARC */)
                throw new SerializationException("Invalid FARC, magic does not match!");
            FatOffset = fs.Length - 0x8 - entryCount * 0x1cL;

            fatTable = new byte[entryCount * 0x1c];

            fs.Seek(FatOffset, SeekOrigin.Begin);
            fs.Read(fatTable, 0,fatTable.Length);
        }
        catch (IOException)
        {
            throw new SerializationException("An I/O error occurred while reading the FARC.");
        }

        // Faster to read the fat table in-memory since it's small.
        var stream = new MemoryInputStream(fatTable);
        for (var i = 0; i < Entries.Length; ++i)
        {
            var fat = new Fat(this, stream.Sha1(), stream.U32(), stream.I32());
            Entries[i] = fat;
            Lookup.Add(fat.GetSha1(), fat);
        }
    }
    
    public override bool Save()
    {
        // The FARC is way too massive to build every time
        // you save, so we only ever save when there's
        // data that needs to be added.

        var size = GetQueueSize();

        var fat = new Fat[Entries.Length + Queue.Count];

        var hashes = Queue.Keys.ToArray();
        var buffers = new byte[hashes.Length][];

        // Create a new FAT table with these new entries
        // appended at the end.
        var offset = FatOffset;
        Array.Copy(Entries, 0, fat, 0, Entries.Length);
        for (int i = Entries.Length, j = 0; i < fat.Length; ++i, ++j)
        {
            buffers[j] = Queue[hashes[j]];
            fat[i] = new Fat(this, hashes[j], offset, buffers[j].Length);
            offset += buffers[j].Length;
        }

        var table = GenerateFat(fat);

        // First, preserve the existing data up to FatOffset
        byte[] existingData;
        using (var readStream = new FileStream(File, FileMode.Open, FileAccess.Read))
        {
            existingData = new byte[FatOffset];
            readStream.Read(existingData, 0, (int)FatOffset);
        }

        var stream = new FileStream(File, FileMode.Create, FileAccess.Write);

        var fatOffset = FatOffset;

        // Write the existing data first
        stream.Write(existingData, 0, existingData.Length);
        stream.Seek(offset, SeekOrigin.Begin);
        foreach (var buffer in buffers)
        {
            stream.Write(buffer, 0, buffer.Length);
            fatOffset += buffer.Length;
//            stream.Seek(offset, SeekOrigin.Begin);
        }

        stream.Write(table, 0, table.Length);
        fatOffset += table.Length;

        // Footer
        var data = Bytes.ToBytesBE(fat.Length);
        stream.Write(data, 0, data.Length);
        fatOffset += sizeof(int);
        stream.Write("FARC"u8.ToArray(), 0, 4); // FARC

        // archive.setLength(archive.getFilePointer());

        // Update the state of the archive in memory.
        Entries = fat;
        Queue.Clear();
        Lookup.Clear();
        foreach (var row in Entries)
            Lookup.Add(row.GetSha1(), row);
        FatOffset = fatOffset;
        LastModified = new DateTimeOffset(System.IO.File.GetLastAccessTime(File)).ToUnixTimeSeconds();
        stream.Close();
        return true;
    }
}