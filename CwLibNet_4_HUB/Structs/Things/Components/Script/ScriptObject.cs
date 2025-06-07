using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using static CwLibNet.IO.Serializer.Serializer;

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
        serializer.Serialize(ref Type);
        switch (Type)
        {
            case ScriptObjectType.NULL:
                return;
            case ScriptObjectType.INSTANCE:
                Value = serializer.Reference((ScriptInstance) Value);
                return;
        }

        var reference = 0;
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
            var value = serializer.GetPointer<object>(reference);
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
                var boolArray = (bool[]) Value;
                Value = serializer.Serialize(ref boolArray);
                break;
            case ScriptObjectType.ARRAY_S32:
                var intArray = (int[]) Value;
                Value = serializer.Serialize(ref intArray);
                break;
            case ScriptObjectType.ARRAY_F32:
                var floatArray = (float[]) Value;
                Value = serializer.Serialize(ref floatArray);
                break;
            case ScriptObjectType.ARRAY_VECTOR4:
                var vectorArray = (Vector4[]) Value;
                Value = serializer.Serialize(ref vectorArray);
                break;
            case ScriptObjectType.STRINGW:
                var stringValue1 = (String) Value;
                Value = serializer.Serialize(ref stringValue1);
                break;
            case ScriptObjectType.STRINGA:
                var stringValue2 = (String) Value;
                Value = serializer.Serialize(ref stringValue2);
                break;
            case ScriptObjectType.RESOURCE:
            {
                var descriptor = (ResourceDescriptor) Value;

                var type = ResourceType.Invalid;
                if (serializer.IsWriting()) type = descriptor.GetResourceType();
                type = ResourceType.FromType(serializer.Serialize(ref type.Value));

                if (type.Value != ResourceType.Invalid.Value)
                {
                    if (serializer.IsWriting())
                        serializer.Serialize(ref (ResourceDescriptor) Value, type);
                    else
                        serializer.Serialize(ref Value, type, false, true, false);
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
                Value = serializer.Array((Thing[]) Value, true);
                break;
            case ScriptObjectType.ARRAY_OBJECT_REF:
            {
                var array = (ScriptObject[]) Value;
                if (!serializer.IsWriting())
                {
                    array = new ScriptObject[serializer.GetInput().I32()];
                    serializer.SetPointer(reference, array);
                }
                else serializer.GetOutput().I32(array.Length);
                for (var i = 0; i < array.Length; ++i)
                    serializer.Serialize(ref array[i]);
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