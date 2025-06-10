using CwLibNet4Hub.Enums;
using CwLibNet4Hub.Structs.Slot;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Presets;

public class Presets
{
    public static Pack Group(string name, string description, string author, ResourceDescriptor icon)
    {
        Pack item = new()
        {
            ContentsType = ContentsType.GROUP,
            Mesh = new ResourceDescriptor(0x6a1a, ResourceType.Mesh),
            Slot =
            {
                Name = name,
                Description = description,
                Icon = icon,
                AuthorName = author
            }
        };

        return item;
    }
    public static Pack Level(string name, string description, string author, ResourceDescriptor icon)
    {
        Pack item = new()
        {
            ContentId = null,

            ContentsType = ContentsType.LEVEL,
            Mesh = new ResourceDescriptor(0x3e86, ResourceType.Mesh),
            Slot =
            {
                Name = name,
                Description = description,
                Icon = icon,
                AuthorName = author
            }
        };

        return item;
    }
}