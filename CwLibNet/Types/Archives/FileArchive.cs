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
            SafeFileHandle handle = System.IO.File.OpenHandle(File);
            if (RandomAccess.GetLength(handle) < 0x8)
            {
                handle.Close();
                throw new SerializationException("Invalid FARC, size is less than minimum" +
                                                 " of 8 " +
                                                 "bytes!");
            }

            // archive.seek(archive.length() - 0x8); // Seek to the bottom of the archive to read
            // entry count and magic.

            int entryCount;
            byte[] entryCountBuffer = new byte[4];
            RandomAccess.Read(handle, entryCountBuffer, RandomAccess.GetLength(handle) - 0x8);
            entryCount = BitConverter.ToInt32(entryCountBuffer, 0);
            this.Entries = new Fat[entryCount];
            byte[] magicBuffer = new byte[4];
            RandomAccess.Read(handle, magicBuffer, RandomAccess.GetLength(handle) - 0x8);
            if (BitConverter.ToInt32(magicBuffer, 0) != 0x46415243 /* FARC */)
                throw new SerializationException("Invalid FARC, magic does not match!");
            this.FatOffset = RandomAccess.GetLength(handle) - 0x8 - (entryCount * 0x1cL);

            fatTable = new byte[entryCount * 0x1c];

//            archive.seek(this.FatOffset);
            RandomAccess.Read(handle, fatTable, this.FatOffset);
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
        
        SafeFileHandle handle = System.IO.File.OpenHandle(File);

        long fatOffset = this.FatOffset;

        foreach (byte[] buffer in buffers)
        {
            RandomAccess.Write(handle, buffer, fatOffset);
            fatOffset += buffer.Length;
        }

        RandomAccess.Write(handle, table, fatOffset);
        fatOffset += table.Length;

        // Footer
        RandomAccess.Write(handle, Bytes.ToBytesBE(fat.Length), fatOffset);
        fatOffset += sizeof(int);
        RandomAccess.Write(handle, "FARC"u8, fatOffset); // FARC

        // archive.setLength(archive.getFilePointer());

        // Update state of the archive in memory.
        this.Entries = fat;
        this.Queue.Clear();
        this.Lookup.Clear();
        foreach (Fat row in this.Entries)
            this.Lookup.Add(row.getSHA1(), row);
        this.FatOffset = fatOffset;
        this.LastModified = new DateTimeOffset(System.IO.File.GetLastAccessTime(File)).ToUnixTimeSeconds();

        return true;
    }
}