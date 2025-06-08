using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.EX;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.Types;
using CwLibNet.Types.Data;

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
        Serializer.SerializeEnum32(ref Type);
        switch (Type)
        {
            case ScriptObjectType.NULL:
                return;
            case ScriptObjectType.INSTANCE:
                Value = serializer.Reference((ScriptInstance) Value);
                return;
        }

        var reference = 0;
        if (Serializer.IsWriting())
        {
            if (Value != null)
                reference = serializer.GetNextReference();
            serializer.GetOutput().I32(reference);
        }
        else reference = serializer.GetInput().I32();
        if (reference == 0) return;

        if (!Serializer.IsWriting())
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
                Serializer.Serialize(ref boolArray);
                Value = boolArray;
                break;
            case ScriptObjectType.ARRAY_S32:
                var intArray = (int[]) Value;
                Serializer.Serialize(ref intArray);
                Value = intArray;
                break;
            case ScriptObjectType.ARRAY_F32:
                var floatArray = (float[]) Value;
                Serializer.Serialize(ref floatArray);
                Value = floatArray;
                break;
            case ScriptObjectType.ARRAY_VECTOR4:
                var vectorArray = (Vector4[]) Value;
                Serializer.Serialize(ref vectorArray);
                Value = vectorArray;
                break;
            case ScriptObjectType.STRINGW:
                var stringValue1 = (String) Value;
                Serializer.Serialize(ref stringValue1);
                Value = stringValue1;
                break;
            case ScriptObjectType.STRINGA:
                var stringValue2 = (String) Value;
                Serializer.Serialize(ref stringValue2);
                Value = stringValue2;
                break;
            case ScriptObjectType.RESOURCE:
            {
                var descriptor = (ResourceDescriptor) Value;

                var type = ResourceType.Invalid;
                if (Serializer.IsWriting()) type = descriptor.GetResourceType();
                var typeValue = type.Value;
                Serializer.Serialize(ref typeValue);
                type = ResourceType.FromType(typeValue);

                if (type.Value != ResourceType.Invalid.Value)
                {
                    if (Serializer.IsWriting())
                    {
                        var resourceDesc = (ResourceDescriptor) Value;
                        Serializer.Serialize(ref resourceDesc, type);
                    }
                    else
                    {
                        var resourceDesc = (ResourceDescriptor?) null;
                        Serializer.Serialize(ref resourceDesc, type, false, true, false);
                        Value = resourceDesc;
                    }
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
                if (!Serializer.IsWriting())
                {
                    array = new ScriptObject[serializer.GetInput().I32()];
                    serializer.SetPointer(reference, array);
                }
                else serializer.GetOutput().I32(array.Length);
                for (var i = 0; i < array.Length; ++i)
                    Serializer.Serialize(ref array[i]);
                Value = array;
                return;
            }
            default:
                throw new SerializationException("Unhandled script object type in field " +
                                                 "member " +
                                                 "reflection!");
        }

        if (!Serializer.IsWriting())
            serializer.SetPointer(reference, Value);

    }

    
    public int GetAllocatedSize()
    {
        return 0;
    }


}