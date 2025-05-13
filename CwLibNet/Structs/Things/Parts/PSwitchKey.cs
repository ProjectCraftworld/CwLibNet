using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Structs.Things.Components.Switches;

namespace CwLibNet.Structs.Things.Parts;

public class PSwitchKey: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public int ColorIndex;
    
    public String Name;

    
    public bool HideInPlayMode;
    
    public bool IsDummy;

    
    
    public SwitchSignal? IsActive;
    
    
    public SwitchKeyType Type = SwitchKeyType.MAGNETIC;

    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();
        int subVersion = serializer.GetRevision().GetSubVersion();

        ColorIndex = serializer.S32(ColorIndex);
        if (version >= 0x2dc)
            Name = serializer.Wstr(Name);

        if (subVersion < 0x132 && version > 0x1bc)
        {
            if (version < 0x3ed) HideInPlayMode = serializer.Bool(HideInPlayMode);
            else if (serializer.IsWriting())
            {
                int flags = HideInPlayMode ? 0x80 : 0x0;
                if (IsDummy) flags |= 0x40;
                serializer.GetOutput().U8(flags);
            }
            else
            {
                int flags = serializer.GetInput().U8();
                HideInPlayMode = (flags & 0x80) != 0;
                IsDummy = (flags & 0x40) != 0;
            }
        }
        else
        {
            if (version > 0x1bc)
                HideInPlayMode = serializer.Bool(HideInPlayMode);
            if (subVersion >= 0x132)
                IsDummy = serializer.Bool(IsDummy);
        }

        // isActiveBoolOldForSerialisation
        if (version is > 0x272 and < 0x2c4)
        {
            if (serializer.IsWriting())
            {
                bool isActive = this.IsActive != null;
                if (isActive) isActive = this.IsActive.Activation != 0.0f;
                serializer.GetOutput().Boole(isActive);
            }
            else serializer.GetInput().Boole();
        }

        if (version > 0x29f && version < 0x2c4)
            serializer.Struct(IsActive);

        if (version > 0x27c && version < 0x2dc)
            serializer.Enum32(Type);
    }

    
    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (Name != null)
            size += (Name.Length * 2);
        return size;
    }


}