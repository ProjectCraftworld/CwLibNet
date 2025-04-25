using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things;

namespace CwLibNet.Structs.Things.Components.Script;

public class ScriptInstance: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? Script;
    public InstanceLayout? InstanceLayout = new();

    
    public void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();

        Script = serializer.Resource(Script, ResourceType.Script);

        bool serialize = true;
        if (version > 0x1a0)
        {
            if (serializer.IsWriting()) serialize = InstanceLayout != null;
            serialize = serializer.Bool(serialize);
        }

        if (serialize)
        {
            int reference = 0;
            if (serializer.IsWriting())
            {
                if (InstanceLayout != null)
                    reference = serializer.GetNextReference();
                serializer.GetOutput().I32(reference);
            }
            else reference = serializer.GetInput().I32();
            if (reference == 0) return;

            if (!serializer.IsWriting())
            {
                InstanceLayout? layout = serializer.GetPointer<InstanceLayout>(reference);
                if (layout == null)
                {
                    layout = new InstanceLayout();
                    InstanceLayout = layout;
                    serializer.SetPointer(reference, layout);
                    layout.Serialize(serializer);
                }
                else
                    InstanceLayout = new InstanceLayout(layout);
            }
            else InstanceLayout.Serialize(serializer);

            bool reflectDivergent = false;
            if (version > 0x19c)
                reflectDivergent = serializer.Bool(reflectDivergent);
            FieldLayoutDetails[] fields =
                InstanceLayout.GetFieldsForReflection(reflectDivergent);
            bool writing = serializer.IsWriting();


            serializer.Log(string.Join('\n', fields.Select(f => $"{reference:X8}:{f}")));
            foreach (FieldLayoutDetails field in fields)
            {
                if (0x198 < version && version < 0x19d) serializer.U8(0);
                serializer.Log(field.Name + " " + field.MachineType);
                switch (field.MachineType)
                {
                    case MachineType.BOOL:
                        field.Value = serializer.Bool(writing && (bool) field.Value);
                        break;
                    case MachineType.CHAR:
                        field.Value = serializer.I16(writing ? (short)field.Value : (short)0);
                        break;
                    case MachineType.S32:
                        field.Value = serializer.I32(writing ? (int) field.Value : 0);
                        if (serializer.IsWriting() && field.FishType == BuiltinType.GUID && ((int) field.Value) != 0)
                        {
                            if (field.Name is "FSB")
                                serializer.AddDependency(new ResourceDescriptor(new GUID(((int) field.Value) & 0xffffffffL), ResourceType.Filename));
                            else if (field.Name is "SettingsFile")
                                serializer.AddDependency(new ResourceDescriptor(new GUID(((int) field.Value) & 0xffffffffL), ResourceType.MusicSettings));
                            else
                                serializer.AddDependency(new ResourceDescriptor(new GUID(((int) field.Value) & 0xffffffffL), ResourceType.FileOfBytes));
                        }
                        break;
                    case MachineType.F32:
                        field.Value = serializer.F32(writing ? (float) field.Value : 0);
                        break;
                    case MachineType.V4:
                        field.Value = serializer.V4((Vector4) field.Value);
                        break;
                    case MachineType.M44:
                        field.Value = serializer.M44((Matrix4x4) field.Value);
                        break;
                    case MachineType.OBJECT_REF:
                        field.Value = serializer.Struct((ScriptObject) field.Value);
                        break;
                    case MachineType.SAFE_PTR:
                        field.Value = serializer.Reference((Thing) field.Value);
                        break;
                    default:
                        throw new SerializationException("Unhandled machine type in " +
                                                         "field member " +
                                                         "reflection!");
                }
            }
        }
    }

    public void AddField(string? name, GUID value)
    {
        FieldLayoutDetails field = new FieldLayoutDetails();
        field.Modifiers.Add(ModifierType.PUBLIC);
        field.Name = name;
        field.InstanceOffset = InstanceLayout.InstanceSize;
        field.FishType = BuiltinType.GUID;
        field.MachineType = MachineType.S32;
        field.Value = (int) value.Value;
        InstanceLayout.InstanceSize += 4;
        InstanceLayout.Fields.Add(field);
    }

    public void AddField(string? name, Thing? value)
    {
        FieldLayoutDetails field = new FieldLayoutDetails();
        field.Modifiers.Add(ModifierType.PUBLIC);
        field.Name = name;
        field.InstanceOffset = InstanceLayout.InstanceSize;
        field.FishType = BuiltinType.VOID;
        field.MachineType = MachineType.SAFE_PTR;
        field.Value = value;
        InstanceLayout.InstanceSize += 4;
        InstanceLayout.Fields.Add(field);
    }

    public void UnsetField(String name)
    {
        foreach (FieldLayoutDetails field in InstanceLayout.Fields)
        {
            if (field.Name.Equals(name))
            {
                field.Value = field.MachineType switch
                {
                    MachineType.BOOL => false,
                    MachineType.CHAR => (short)0,
                    MachineType.S32 => 0,
                    MachineType.F32 => 0.0f,
                    MachineType.V4 => new Vector4(),
                    MachineType.M44 => Matrix4x4.Identity,
                    MachineType.OBJECT_REF => new ScriptObject(ScriptObjectType.NULL, null),
                    _ => null
                };
            }
        }
    }

    public FieldLayoutDetails GetField(string name)
    {
        if (InstanceLayout == null) return null;
        foreach (FieldLayoutDetails details in InstanceLayout.Fields)
        {
            if (details.Name.Equals(name))
                return details;
        }
        return null;
    }

    public void SetField(string name, object? value)
    {
        FieldLayoutDetails field = GetField(name);
        if (field != null)
            field.Value = value;
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}