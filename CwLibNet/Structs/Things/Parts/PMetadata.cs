using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Resources;
using CwLibNet.Structs.Inventory;
using CwLibNet.Structs.Things.Components;
using CwLibNet.Types;
using CwLibNet.Types.Data;

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
    
    public void Serialize(Serializer serializer)
    {
        Revision revision = serializer.GetRevision();
        int version = revision.GetVersion();

        bool hasDepreciatedValue = (version < 0x297 && !revision.IsLeerdammer()) || (revision.IsLeerdammer() && !revision.Has(Branch.Leerdammer, (int)Revisions.LdResources));

        if (hasDepreciatedValue)
            Value = serializer.Struct(Value);
        
        if (revision.Has(Branch.Leerdammer, (int)Revisions.LdLamsKeys) || version > 0x2ba) {
            TitleKey = serializer.U32(TitleKey);
            DescriptionKey = serializer.U32(DescriptionKey);
            Location = serializer.U32(Location);
            Category = serializer.U32(Category);
        } else {
            NameTranslationTag = serializer.Str(NameTranslationTag);
            if (version < 0x159)
                DescTranslationTag = serializer.Str(DescTranslationTag);
            else if (!serializer.IsWriting())
                DescTranslationTag = NameTranslationTag + "_DESC";
            
            LocationTag = serializer.Str(LocationTag);
            CategoryTag = serializer.Str(CategoryTag);
            if (!serializer.IsWriting()) {
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
            PrimaryIndex = serializer.I32(PrimaryIndex);
        FluffCost = serializer.I32(FluffCost);

        if (hasDepreciatedValue) serializer.I32(0); // unknown
        
        if (serializer.IsWriting())
            serializer.GetOutput().I32(Type.GetFlags());
        else
            Type = InvObjectBody.FromFlags(serializer.GetInput().I32(), serializer.GetRevision());
        
        SubType = serializer.I32(SubType);
        CreationDate = serializer.U32(CreationDate);
        
        Icon = serializer.Resource(Icon, ResourceType.Texture);
        PhotoMetadata = serializer.Reference(PhotoMetadata);
        
        if (version >= 0x15f)
            Referencable = serializer.Bool(Referencable);
        if (version >= 0x205)
            AllowEmit = serializer.Bool(AllowEmit);
    }

    public int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (this.NameTranslationTag != null) size += this.NameTranslationTag.Length;
        if (this.DescTranslationTag != null) size += this.DescTranslationTag.Length;
        if (this.LocationTag != null) size += this.LocationTag.Length;
        if (this.CategoryTag != null) size += this.CategoryTag.Length;
        if (this.PhotoMetadata != null) size += this.PhotoMetadata.GetAllocatedSize();
        return size;
    }
}