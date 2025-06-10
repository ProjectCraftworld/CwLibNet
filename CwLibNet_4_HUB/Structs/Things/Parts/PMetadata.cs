using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Resources;
using CwLibNet4Hub.Structs.Inventory;
using CwLibNet4Hub.Structs.Things.Components;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Parts;

public class PMetadata: ISerializable
{
    public const int BaseAllocationSize = 0x160;
    
    public string? NameTranslationTag;
    public string? LocationTag;
    public string? CategoryTag;
    public string? DescTranslationTag;

    public long TitleKey, DescriptionKey, Location, Category;

    public int PrimaryIndex;
    public int FluffCost;
    public HashSet<InventoryObjectType> Type = [];
    public int SubType;
    public long CreationDate;

    public ResourceDescriptor? Icon;
    public PhotoMetadata? PhotoMetadata;
    
    public Value? Value;

    public bool Referencable;
    public bool AllowEmit;
    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var revision = Serializer.GetCurrentSerializer().GetRevision();
        var version = revision.GetVersion();

        var hasDepreciatedValue = (version < 0x297 && !revision.IsLeerdammer()) || (revision.IsLeerdammer() && !revision.Has(Branch.Leerdammer, (int)Revisions.LD_RESOURCES));

        if (hasDepreciatedValue)
            Serializer.Serialize(ref Value);
        
        if (revision.Has(Branch.Leerdammer, (int)Revisions.LD_LAMS_KEYS) || version > 0x2ba) {
            Serializer.Serialize(ref TitleKey);
            Serializer.Serialize(ref DescriptionKey);
            Serializer.Serialize(ref Location);
            Serializer.Serialize(ref Category);
        } else {
            Serializer.Serialize(ref NameTranslationTag);
            if (version < 0x159)
                Serializer.Serialize(ref DescTranslationTag);
            else if (!Serializer.IsWriting())
                DescTranslationTag = NameTranslationTag + "_DESC";
            
            Serializer.Serialize(ref LocationTag);
            Serializer.Serialize(ref CategoryTag);
            if (!Serializer.IsWriting()) {
                if (version < 0x159) {
                    TitleKey =
                        RTranslationTable.MakeLamsKeyID(NameTranslationTag);
                    DescriptionKey =
                        RTranslationTable.MakeLamsKeyID(DescTranslationTag);
                } else {
                    TitleKey = RTranslationTable.MakeLamsKeyID(NameTranslationTag + "_NAME");
                    TitleKey = RTranslationTable.MakeLamsKeyID(NameTranslationTag + "_DESC");
                }
                
                Location 
                        = RTranslationTable.MakeLamsKeyID(LocationTag);
                Category 
                        = RTranslationTable.MakeLamsKeyID(CategoryTag);
            }
        }
        
        if (version >= 0x195)
            Serializer.Serialize(ref PrimaryIndex);
        Serializer.Serialize(ref FluffCost);

        if (hasDepreciatedValue) Serializer.Serialize(ref temp_int); // unknown
        
        if (Serializer.IsWriting())
            Serializer.GetCurrentSerializer().GetOutput().I32(Type.GetFlags());
        else
            Type = InvObjectBody.FromFlags(Serializer.GetCurrentSerializer().GetInput().I32(), Serializer.GetCurrentSerializer().GetRevision());
        
        Serializer.Serialize(ref SubType);
        Serializer.Serialize(ref CreationDate);
        
        Serializer.Serialize(ref Icon, ResourceType.Texture, false, true, false);
        Serializer.Serialize(ref PhotoMetadata);
        
        if (version >= 0x15f)
            Serializer.Serialize(ref Referencable);
        if (version >= 0x205)
            Serializer.Serialize(ref AllowEmit);
    }

    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (NameTranslationTag != null) size += NameTranslationTag.Length;
        if (DescTranslationTag != null) size += DescTranslationTag.Length;
        if (LocationTag != null) size += LocationTag.Length;
        if (CategoryTag != null) size += CategoryTag.Length;
        if (PhotoMetadata != null) size += PhotoMetadata.GetAllocatedSize();
        return size;
    }
}