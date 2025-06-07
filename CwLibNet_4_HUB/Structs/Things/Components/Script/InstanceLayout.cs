using CwLibNet.Enums;
using CwLibNet.IO;
using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.Structs.Things.Components.Script;

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

    
    public void Serialize()
    {
        Serializer.Serialize(ref Fields);
        if (Serializer.GetRevision().GetVersion() < 0x1ec)
        {
            if (Serializer.IsWriting())
            {
                var stream = Serializer.GetOutput();
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
                var stream = Serializer.GetInput();
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