using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;

namespace CwLibNet.Structs.Things.Components.Script;

public class InstanceLayout: ISerializable
{
    public const int BaseAllocationSize = 0x8;

    public List<FieldLayoutDetails> Fields = [];
    public int InstanceSize;

    public InstanceLayout() { }

    public InstanceLayout(InstanceLayout? layout)
    {
        foreach (FieldLayoutDetails field in layout.Fields)
            this.Fields.Add(new FieldLayoutDetails(field));
        this.InstanceSize = layout.InstanceSize;
    }

    
    public void Serialize(Serializer serializer)
    {
        Fields = serializer.Arraylist(Fields);
        if (serializer.GetRevision().GetVersion() < 0x1ec)
        {
            if (serializer.IsWriting())
            {
                MemoryOutputStream stream = serializer.GetOutput();
                FieldLayoutDetails[] reflectFields = GetFieldsForReflection(false);
                stream.I32(reflectFields.Length);
                foreach (FieldLayoutDetails field in reflectFields)
                {
                    stream.Str(field.Name);
                    stream.I32(this.Fields.IndexOf(field));
                }
            }
            else
            {
                MemoryInputStream stream = serializer.GetInput();
                int count = stream.I32();
                for (int i = 0; i < count; ++i)
                {
                    stream.Str(); // fieldName
                    stream.I32(); // fieldIndex
                }
            }
        }
        InstanceSize = serializer.I32(InstanceSize);
    }

    public FieldLayoutDetails[] GetFieldsForReflection(bool reflectDivergent)
    {
        List<FieldLayoutDetails> fields = new(this.Fields.Count);
        foreach (FieldLayoutDetails field in this.Fields)
        {
            if (field.Modifiers.Contains(ModifierType.DIVERGENT) && !reflectDivergent)
                continue;
            fields.Add(field);
        }

        return fields.ToArray();
    }

    
    public int GetAllocatedSize()
    {
        int size = InstanceLayout.BaseAllocationSize;
        if (this.Fields != null) size += this.Fields.Sum(details => (details.GetAllocatedSize() * 2));
        return size;
    }


}