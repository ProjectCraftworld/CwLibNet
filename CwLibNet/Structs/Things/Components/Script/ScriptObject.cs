using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types.Data;
using CwLibNet.Types.Things;

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

    
    public void Serialize(Serializer serializer)
    {
        Type = serializer.Enum32(Type);
        if (Type == ScriptObjectType.NULL) return;

        if (Type == ScriptObjectType.INSTANCE)
        {
            Value = serializer.Reference((ScriptInstance) Value);
            return;
        }

        int reference = 0;
        if (serializer.IsWriting())
        {
            if (Value != null)
                reference = serializer.GetNextReference();
            serializer.GetOutput().I32(reference);
        }
        else reference = serializer.GetInput().I32();
        if (reference == 0) return;

        if (!serializer.IsWriting())
        {
            object? value = serializer.GetPointer<object>(reference);
            if (value != null)
            {
                Value = value;
                return;
            }
        }

        serializer.Log("" + Type);
        switch (Type)
        {
            case ScriptObjectType.NULL:
                Value = null;
                break;
            case ScriptObjectType.ARRAY_BOOL:
                Value = serializer.Boolarray((bool[]) Value);
                break;
            case ScriptObjectType.ARRAY_S32:
                Value = serializer.Intvector((int[]) Value);
                break;
            case ScriptObjectType.ARRAY_F32:
                Value = serializer.Floatarray((float[]) Value);
                break;
            case ScriptObjectType.ARRAY_VECTOR4:
                Value = serializer.Vectorarray((Vector4[]) Value);
                break;
            case ScriptObjectType.STRINGW:
                Value = serializer.Wstr((String) Value);
                break;
            case ScriptObjectType.STRINGA:
                Value = serializer.Str((String) Value);
                break;
            case ScriptObjectType.RESOURCE:
            {
                ResourceDescriptor descriptor = (ResourceDescriptor) Value;

                ResourceType type = ResourceType.Invalid;
                if (serializer.IsWriting()) type = descriptor.GetResourceType();
                type = ResourceType.FromType(serializer.I32(type.Value));

                if (type.Value != ResourceType.Invalid.Value)
                {
                    if (serializer.IsWriting())
                        serializer.Resource((ResourceDescriptor) Value, type);
                    else
                        Value = serializer.Resource(null, type);
                }

                break;
            }
            // case INSTANCE: object.value = serializer.reference((PScript) object.value,
            // PScript
            // .class); break;
            case ScriptObjectType.AUDIOHANDLE:
                Value = null;
                break;
            case ScriptObjectType.ARRAY_SAFE_PTR:
                Value = serializer.Array((Thing[]) Value, true);
                break;
            case ScriptObjectType.ARRAY_OBJECT_REF:
            {
                ScriptObject[] array = (ScriptObject[]) Value;
                if (!serializer.IsWriting())
                {
                    array = new ScriptObject[serializer.GetInput().I32()];
                    serializer.SetPointer(reference, array);
                }
                else serializer.GetOutput().I32(array.Length);
                for (int i = 0; i < array.Length; ++i)
                    array[i] = serializer.Struct(array[i]);
                return;
            }
            default:
                throw new SerializationException("Unhandled script object type in field " +
                                                 "member " +
                                                 "reflection!");
        }

        if (!serializer.IsWriting())
            serializer.SetPointer(reference, Value);

    }

    
    public int GetAllocatedSize()
    {
        return 0;
    }


}