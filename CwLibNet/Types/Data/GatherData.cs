using System;
using CwLibNet.Enums;

namespace CwLibNet.Types.Data
{
    public class GatherData
    {
        private readonly string path;
        private readonly SHA1 sha1;
        private readonly byte[]? data;
        private readonly GUID guid;

        public GatherData(string path, ResourceDescriptor descriptor, byte[]? data)
        {
            this.path = path;
            this.sha1 = SHA1.FromBuffer(data);

            this.data = data;
            this.guid = (GUID)descriptor.GetGUID();
        }

        public GatherData(string path, SHA1 sha1, byte[]? data)
            : this(path, new ResourceDescriptor(sha1, ResourceType.Invalid), data) { }
        
        public GatherData(string path, GUID guid, byte[]? data)
            : this(path, new ResourceDescriptor(guid, ResourceType.Invalid), data) { }

        public GatherData(string path, GUID guid, SHA1 sha1, byte[]? data)
        {
            this.path = path;
            this.guid = guid;
            this.sha1 = sha1;
            this.data = data;
        }

        public GatherData()
        {
            this.path = string.Empty;
            this.sha1 = new SHA1(); // Assuming SHA1 has a default constructor
            this.data = Array.Empty<byte>();
            this.guid = new GUID(); // Assuming GUID has a default constructor
        }

        public string GetPath() => this.path;
        public SHA1 GetSHA1() => this.sha1;
        public byte[]? GetData() => this.data;
        public GUID GetGUID() => this.guid;
    }
}