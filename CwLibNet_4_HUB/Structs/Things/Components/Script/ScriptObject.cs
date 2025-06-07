using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.Types.Data;
using static net.torutheredfox.craftworld.serialization.Serializer;

namespace CwLibNet.Structs.Things.Components.Script;

public class ScriptObject: ISerializable
{
    public ScriptObjectType Type = ScriptObjectType.NULL;
    public object? Value;

    public ScriptObject() { }

    public ScriptObject(ScriptObjectType type, object? value)
    {
        Type = type;
        Value = value;
    }

    
    public void Serialize()
    {
        Serializer.Serialize(ref Type);
        switch (Type)
        {
            case ScriptObjectType.NULL:
                return;
            case ScriptObjectType.INSTANCE:
                Value = Serializer.Reference((ScriptInstance) Value);
                return;
        }

        var reference = 0;
        if (Serializer.IsWriting())
        {
            if (Value != null)
                reference = Serializer.GetNextReference();
            Serializer.GetOutput().I32(reference);
        }
        else reference = Serializer.GetInput().I32();
        if (reference == 0) return;

        if (!Serializer.IsWriting())
        {
            var value = Serializer.GetPointer<object>(reference);
            if (value != null)
            {
                Value = value;
                return;
            }
        }

        Serializer.Log("" + Type);
        switch (Type)
        {
            case ScriptObjectType.NULL:
                Value = null;
                break;
            case ScriptObjectType.ARRAY_BOOL:
                Value = Serializer.Serialize(ref (bool[]) Value);
                break;
            case ScriptObjectType.ARRAY_S32:
                Value = Serializer.Serialize(ref (int[]) Value);
                break;
            case ScriptObjectType.ARRAY_F32:
                Value = Serializer.Serialize(ref (float[]) Value);
                break;
            case ScriptObjectType.ARRAY_VECTOR4:
                Value = Serializer.Serialize(ref (Vector4[]) Value);
                break;
            case ScriptObjectType.STRINGW:
                Value = Serializer.Serialize(ref (String) Value);
                break;
            case ScriptObjectType.STRINGA:
                Value = Serializer.Serialize(ref (String) Value);
                break;
            case ScriptObjectType.RESOURCE:
            {
                var descriptor = (ResourceDescriptor) Value;

                var type = ResourceType.Invalid;
                if (Serializer.IsWriting()) type = descriptor.GetResourceType();
                type = ResourceType.FromType(Serializer.Serialize(ref type.Value));

                if (type.Value != ResourceType.Invalid.Value)
                {
                    if (Serializer.IsWriting())
                        Serializer.Serialize(ref (ResourceDescriptor) Value, type);
                    else
                        Serializer.Serialize(ref Value, null, type);
                }

                break;
            }
            // case INSTANCE: object.value = Serializer.Reference((PScript) object.value,
            // PScript
            // .class); break;
            case ScriptObjectType.AUDIOHANDLE:
                Value = null;
                break;
            case ScriptObjectType.ARRAY_SAFE_PTR:
                Value = Serializer.Array((Thing[]) Value, true);
                break;
            case ScriptObjectType.ARRAY_OBJECT_REF:
            {
                var array = (ScriptObject[]) Value;
                if (!Serializer.IsWriting())
                {
                    array = new ScriptObject[Serializer.GetInput().I32()];
                    Serializer.SetPointer(reference, array);
                }
                else Serializer.GetOutput().I32(array.Length);
                for (var i = 0; i < array.Length; ++i)
                    Serializer.Serialize(ref array[i]);
                return;
            }
            default:
                throw new SerializationException("Unhandled script object type in field " +
                                                 "member " +
                                                 "reflection!");
        }

        if (!Serializer.IsWriting())
            Serializer.SetPointer(reference, Value);

    }

    
    public int GetAllocatedSize()
    {
        return 0;
    }


}