using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Structs.Things.Components.Switches;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

public class PSwitchKey: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public int ColorIndex;
    
    public string? Name;

    
    public bool HideInPlayMode;
    
    public bool IsDummy;

    
    
    public SwitchSignal? IsActive;
    
    
    public SwitchKeyType Type = SwitchKeyType.MAGNETIC;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();
        var subVersion = Serializer.GetCurrentSerializer().GetRevision().GetSubVersion();

        Serializer.Serialize(ref ColorIndex);
        if (version >= 0x2dc)
            Serializer.Serialize(ref Name);

        if (subVersion < 0x132 && version > 0x1bc)
        {
            if (version < 0x3ed) Serializer.Serialize(ref HideInPlayMode);
            else if (Serializer.IsWriting())
            {
                var flags = HideInPlayMode ? 0x80 : 0x0;
                if (IsDummy) flags |= 0x40;
                Serializer.GetCurrentSerializer().GetOutput().U8(flags);
            }
            else
            {
                var flags = Serializer.GetCurrentSerializer().GetInput().U8();
                HideInPlayMode = (flags & 0x80) != 0;
                IsDummy = (flags & 0x40) != 0;
            }
        }
        else
        {
            if (version > 0x1bc)
                Serializer.Serialize(ref HideInPlayMode);
            if (subVersion >= 0x132)
                Serializer.Serialize(ref IsDummy);
        }

        // isActiveBoolOldForSerialisation
        if (version is > 0x272 and < 0x2c4)
        {
            if (Serializer.IsWriting())
            {
                var isActive = IsActive != null;
                if (isActive) isActive = IsActive is not { Activation: 0.0f };
                Serializer.GetCurrentSerializer().GetOutput().Boole(isActive);
            }
            else Serializer.GetCurrentSerializer().GetInput().Boole();
        }

        if (version is > 0x29f and < 0x2c4)
            Serializer.Serialize(ref IsActive);

        if (version is > 0x27c and < 0x2dc)
            Type = serializer.Enum32(Type);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Name != null)
            size += (Name.Length * 2);
        return size;
    }


}