using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.EX;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Things.Components.Script;

public class ScriptInstance: ISerializable
{
    public const int BaseAllocationSize = 0x30;

    public ResourceDescriptor? Script;
    public InstanceLayout? InstanceLayout = new();

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        int temp_int = 0;
        bool temp_bool_true = true;
        bool temp_bool_false = false;

        var version = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        Serializer.Serialize(ref Script, ResourceType.Script, false, true, false);

        var serialize = true;
        if (version > 0x1a0)
        {
            if (Serializer.IsWriting()) serialize = InstanceLayout != null;
            Serializer.Serialize(ref serialize);
        }

        if (!serialize) return;
        var reference = 0;
        if (Serializer.IsWriting())
        {
            if (InstanceLayout != null)
                reference = Serializer.GetCurrentSerializer().GetNextReference();
            Serializer.GetCurrentSerializer().GetOutput().I32(reference);
        }
        else reference = Serializer.GetCurrentSerializer().GetInput().I32();
        if (reference == 0) return;

        if (!Serializer.IsWriting())
        {
            var layout = Serializer.GetCurrentSerializer().GetPointer<InstanceLayout>(reference);
            if (layout == null)
            {
                layout = new InstanceLayout();
                InstanceLayout = layout;
                Serializer.GetCurrentSerializer().SetPointer(reference, layout);
                layout.Serialize(serializer);
            }
            else
                InstanceLayout = new InstanceLayout(layout);
        }
        else InstanceLayout.Serialize(serializer);

        var reflectDivergent = false;
        if (version > 0x19c)
            Serializer.Serialize(ref reflectDivergent);
        var fields =
            InstanceLayout.GetFieldsForReflection(reflectDivergent);
        var writing = Serializer.IsWriting();


        Serializer.LogMessage(string.Join('\n', fields.Select(f => $"{reference:X8}:{f}")));
        foreach (var field in fields)
        {
            if (version is > 0x198 and < 0x19d) Serializer.Serialize(ref temp_int);
            Serializer.LogMessage(field.Name + " " + field.MachineType);
            switch (field.MachineType)
            {
                case MachineType.BOOL:
                    var boolValue = writing && (bool) field.Value!;
                    Serializer.Serialize(ref boolValue);
                    field.Value = boolValue;
                    break;
                case MachineType.CHAR:
                    var charValue = writing ? (short)field.Value! : (short)0;
                    Serializer.Serialize(ref charValue);
                    field.Value = charValue;
                    break;
                case MachineType.S32:
                    var s32Value = writing ? (int) field.Value! : 0;
                    Serializer.Serialize(ref s32Value);
                    field.Value = s32Value;
                    if (Serializer.IsWriting() && field.FishType == BuiltinType.GUID && (int) field.Value != 0)
                    {
                        switch (field.Name)
                        {
                            case "FSB":
                                Serializer.GetCurrentSerializer().AddDependency(new ResourceDescriptor(new GUID((int) field.Value & 0xffffffffL), ResourceType.Filename));
                                break;
                            case "SettingsFile":
                                Serializer.GetCurrentSerializer().AddDependency(new ResourceDescriptor(new GUID((int) field.Value & 0xffffffffL), ResourceType.MusicSettings));
                                break;
                            default:
                                Serializer.GetCurrentSerializer().AddDependency(new ResourceDescriptor(new GUID((int) field.Value & 0xffffffffL), ResourceType.FileOfBytes));
                                break;
                        }
                    }
                    break;
                case MachineType.F32:
                    var f32Value = writing ? (float) field.Value! : 0;
                    Serializer.Serialize(ref f32Value);
                    field.Value = f32Value;
                    break;
                case MachineType.V4:
                    var v4Value = writing ? (Vector4)field.Value! : Vector4.Zero;
                    serializer.V4(v4Value);
                    field.Value = v4Value;
                    break;
                case MachineType.M44:
                    var m44Value = writing ? (Matrix4x4)field.Value! : Matrix4x4.Identity;
                    serializer.M44(m44Value);
                    field.Value = m44Value;
                    break;
                case MachineType.OBJECT_REF:
                    var objRef = (ScriptObject) field.Value!;
                    Serializer.Serialize(ref objRef);
                    field.Value = objRef;
                    break;
                case MachineType.SAFE_PTR:
                    field.Value = Serializer.SerializeReference((Thing) field.Value!);
                    break;
                default:
                    throw new SerializationException("Unhandled machine type in " +
                                                     "field member " +
                                                     "reflection!");
            }
        }
    }

    public void AddField(string? name, GUID value)
    {
        var field = new FieldLayoutDetails();
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
        var field = new FieldLayoutDetails();
        field.Modifiers.Add(ModifierType.PUBLIC);
        field.Name = name;
        field.InstanceOffset = InstanceLayout.InstanceSize;
        field.FishType = BuiltinType.VOID;
        field.MachineType = MachineType.SAFE_PTR;
        field.Value = value;
        InstanceLayout.InstanceSize += 4;
        InstanceLayout.Fields.Add(field);
    }

    public void UnsetField(string name)
    {
        foreach (var field in InstanceLayout.Fields.Where(field => field.Name.Equals(name)))
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

    public FieldLayoutDetails GetField(string name)
    {
        return InstanceLayout?.Fields.FirstOrDefault(details => details.Name.Equals(name));
    }

    public void SetField(string name, object? value)
    {
        var field = GetField(name);
        if (field != null)
            field.Value = value;
    }

    
    public int GetAllocatedSize()
    {
        return BaseAllocationSize;
    }


}