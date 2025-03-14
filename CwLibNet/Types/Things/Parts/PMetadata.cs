using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;

namespace CwLibNet.Types.Things.Parts;

public class PMetadata: ISerializable
{
    public String NameTranslationTag, LocationTag, CategoryTag;
    public String DescTranslationTag;

    public long TitleKey, DescriptionKey, Location, Category;

    public int PrimaryIndex;
    public int FluffCost;
    public HashSet<InventoryObjectType> Type = [];
    public int SubType;
    public long CreationDate;

    public ResourceDescriptor Icon;
    // public PhotoMetadata PhotoMetadata;

    public bool Referencable;
    public bool AllowEmit;
    
    public void Serialize(Serializer serializer)
    {
        throw new NotImplementedException();
    }

    public int GetAllocatedSize()
    {
        throw new NotImplementedException();
    }
}