using CwLibNet.IO;
using CwLibNet.IO.Serializer;

namespace CwLibNet.Structs.Things.Components.Popit;

public class PoppetMode: ISerializable
{
    public const int BaseAllocationSize = 0x20;

    public int Mode, SubMode;
    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        Mode = serializer.I32(Mode);
        SubMode = serializer.I32(SubMode);

        if (version < 0x18b) serializer.I32(0);
        if (version > 0x1b7 && version < 0x1ba)
        {
            serializer.I32(0);
            serializer.I32(0);
            serializer.I32(0);
            serializer.I32(0);
        }
    }

    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }
}