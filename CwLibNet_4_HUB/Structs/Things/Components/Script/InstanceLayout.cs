using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Things.Components.Script;

public class InstanceLayout: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public List<FieldLayoutDetails> Fields = [];
    public int InstanceSize;

    public InstanceLayout() { }

    public InstanceLayout(InstanceLayout? layout)
    {
        foreach (var field in layout.Fields)
            Fields.Add(new FieldLayoutDetails(field));
        InstanceSize = layout.InstanceSize;
    }

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Fields);
        if (Serializer.GetCurrentSerializer().GetRevision().GetVersion() < 0x1ec)
        {
            if (Serializer.IsWriting())
            {
                var stream = Serializer.GetCurrentSerializer().GetOutput();
                var reflectFields = GetFieldsForReflection(false);
                stream.I32(reflectFields.Length);
                foreach (var field in reflectFields)
                {
                    stream.Str(field.Name);
                    stream.I32(Fields.IndexOf(field));
                }
            }
            else
            {
                var stream = Serializer.GetCurrentSerializer().GetInput();
                var count = stream.I32();
                for (var i = 0; i < count; ++i)
                {
                    stream.Str(); // fieldName
                    stream.I32(); // fieldIndex
                }
            }
        }
        Serializer.Serialize(ref InstanceSize);
    }

    public FieldLayoutDetails[] GetFieldsForReflection(bool reflectDivergent)
    {
        List<FieldLayoutDetails> fields = new(Fields.Count);
        fields.AddRange(Fields.Where(field => !field.Modifiers.Contains(ModifierType.DIVERGENT) || reflectDivergent));
        return fields.ToArray();
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Fields != null) size += Fields.Sum(details => details.GetAllocatedSize() * 2);
        return size;
    }


}