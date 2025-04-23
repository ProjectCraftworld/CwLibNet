using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO.Streams;
using CwLibNet.Util;
using Microsoft.Win32.SafeHandles;

namespace CwLibNet.Types.Archives;

public class FileArchive: Fart
{
    public FileArchive(string file): base(file, ArchiveType.FARC)
    {
        byte[] fatTable = null;

        try
        {
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fs.Length < 0x8)
            {
                fs.Close();
                throw new SerializationException("Invalid FARC, size is less than minimum" +
                                                 " of 8 " +
                                                 "bytes!");
            }

            // archive.seek(archive.length() - 0x8); // Seek to the bottom of the archive to read
            // entry count and magic.

            int entryCount;
            byte[] entryCountBuffer = new byte[4];
            fs.Seek(fs.Length - 8, SeekOrigin.Begin);
            fs.Read(entryCountBuffer, 0, 4);
            entryCount = Bytes.ToIntegerBE(entryCountBuffer);
            this.Entries = new Fat[entryCount];
            byte[] magicBuffer = new byte[4];
            fs.Seek(fs.Length - 0x4, SeekOrigin.Begin);
            fs.Read(magicBuffer, 0, 0x4);
            if (!magicBuffer.SequenceEqual("FARC"u8.ToArray())  /* FARC */)
                throw new SerializationException("Invalid FARC, magic does not match!");
            this.FatOffset = fs.Length - 0x8 - (entryCount * 0x1cL);

            fatTable = new byte[entryCount * 0x1c];

            fs.Seek(this.FatOffset, SeekOrigin.Begin);
            fs.Read(fatTable, 0,fatTable.Length);
        }
        catch (IOException ex)
        {
            throw new SerializationException("An I/O error occurred while reading the FARC.");
        }

        // Faster to read the fat table in-memory since it's small.
        MemoryInputStream stream = new MemoryInputStream(fatTable);
        for (int i = 0; i < this.Entries.Length; ++i)
        {
            Fat fat = new Fat(this, stream.Sha1(), stream.U32(), stream.I32());
            this.Entries[i] = fat;
            this.Lookup.Add(fat.getSHA1(), fat);
        }
    }
    
    public override bool Save()
    {
        // The FARC is way too massive to build every time
        // you save, so we only ever save when there's
        // data that needs to be added.

        long size = this.GetQueueSize();

        Fat[] fat = new Fat[this.Entries.Length + this.Queue.Count];

        SHA1[] hashes = this.Queue.Keys.ToArray();
        byte[][] buffers = new byte[hashes.Length][];

        // Create a new FAT table with these new entries
        // appended at the end.
        long offset = this.FatOffset;
        Array.Copy(this.Entries, 0, fat, 0, this.Entries.Length);
        for (int i = this.Entries.Length, j = 0; i < fat.Length; ++i, ++j)
        {
            buffers[j] = this.Queue[hashes[j]];
            fat[i] = new Fat(this, hashes[j], offset, buffers[j].Length);
            offset += buffers[j].Length;
        }

        byte[] table = Fart.GenerateFat(fat);

        FileStream stream = new FileStream(File, FileMode.Create, FileAccess.Write);

        long fatOffset = this.FatOffset;

        stream.Seek(offset, SeekOrigin.Begin);
        foreach (byte[] buffer in buffers)
        {
            stream.Write(buffer, 0, buffer.Length);
            fatOffset += buffer.Length;
//            stream.Seek(offset, SeekOrigin.Begin);
        }

        stream.Write(table, 0, table.Length);
        fatOffset += table.Length;

        // Footer
        byte[] data = Bytes.ToBytesBE(fat.Length);
        stream.Write(data, 0, data.Length);
        fatOffset += sizeof(int);
        stream.Write("FARC"u8.ToArray(), 0, 4); // FARC

        // archive.setLength(archive.getFilePointer());

        // Update the state of the archive in memory.
        this.Entries = fat;
        this.Queue.Clear();
        this.Lookup.Clear();
        foreach (Fat row in this.Entries)
            this.Lookup.Add(row.getSHA1(), row);
        this.FatOffset = fatOffset;
        this.LastModified = new DateTimeOffset(System.IO.File.GetLastAccessTime(File)).ToUnixTimeSeconds();
        stream.Close();
        return true;
    }
}