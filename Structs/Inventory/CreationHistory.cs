using CwLibNet.IO;
using CwLibNet.IO.Gson;
using CwLibNet.IO.Serialization;
using CwLibNet.IO.Streams;

namespace CwLibNet.Structs.Inventory
{
    /// <summary>
    /// Represents all the users who have had
    /// some form of contribution to this resource.
    /// </summary>
    public class CreationHistory : Serializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x4;
        public String[] creators;
        public CreationHistory()
        {
        }

        public CreationHistory(string creator)
        {
            this.creators = new string[]
            {
                creator
            };
        }

        public CreationHistory(String[] creators)
        {
            this.creators = creators;
        }

        public override void Serialize(Serializer serializer)
        {
            bool isFixed = serializer.GetRevision().GetVersion() > 0x37c;
            if (serializer.IsWriting())
            {
                MemoryOutputStream stream = serializer.GetOutput();
                if (creators != null)
                {
                    stream.I32(creators.length);
                    foreach (string editor in creators)
                    {
                        if (isFixed)
                            stream.Str(editor, 0x14);
                        else
                            stream.Wstr(editor);
                    }
                }
                else
                    stream.I32(0);
                return;
            }

            MemoryInputStream stream = serializer.GetInput();
            creators = new string[stream.I32()];
            for (int i = 0; i < creators.length; ++i)
            {
                if (isFixed)
                    creators[i] = stream.Str(0x14);
                else
                    creators[i] = stream.Wstr();
            }
        }

        public virtual int GetAllocatedSize()
        {
            int size = BASE_ALLOCATION_SIZE;
            if (this.creators != null)
                foreach (string editor in this.creators)
                    size += ((editor.Length() * 2) + 4);
            return size;
        }
    }
}