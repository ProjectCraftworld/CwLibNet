using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.Resources;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Parts;

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
    
    public void Serialize()
    {
        var revision = Serializer.GetRevision();
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

        if (hasDepreciatedValue) Serializer.Serialize(ref 0); // unknown
        
        if (Serializer.IsWriting())
            Serializer.GetOutput().I32(Type.GetFlags());
        else
            Type = InvObjectBody.FromFlags(Serializer.GetInput().I32(), Serializer.GetRevision());
        
        Serializer.Serialize(ref SubType);
        Serializer.Serialize(ref CreationDate);
        
        Serializer.Serialize(ref Icon, Icon, ResourceType.Texture);
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