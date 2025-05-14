using CwLibNet.Enums;
using CwLibNet.Structs.Slot;
using CwLibNet.Types.Data;

namespace CwLibNet.Presets;

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