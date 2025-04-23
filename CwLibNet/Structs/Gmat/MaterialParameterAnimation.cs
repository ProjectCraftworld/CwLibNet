using System.Numerics;
using CwLibNet.Extensions;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;

namespace CwLibNet.Structs.Gmat;

public class MaterialParameterAnimation: ISerializable
{
    public const int BaseAllocationSize = 0x20;
    public const int NameSize = 0x3;

    public Vector4 BaseValue;
    public Vector4[] Keys;
    private String name = "";
    public byte ComponentsAnimated;

    
    public void Serialize(Serializer serializer)
    {
        BaseValue = serializer.V4(BaseValue)!.Value;

        if (serializer.IsWriting())
        {
            MemoryOutputStream stream = serializer.GetOutput();
            ComponentsAnimated = CalculateComponentsAnimated();
            if (Keys == null || Keys.Length == 0) stream.I32(0);
            else
            {
                int count = GetNumberOfComponentsAnimated() * Keys.Length;
                stream.I32(count);
                for (int i = 0; i < 4; ++i)
                {
                    if ((ComponentsAnimated & (1 << i)) != 0)
                    {
                        foreach (Vector4 key in Keys) stream.F32(key.GetComponent(i));
                    }
                }
            }

            stream.I32(3);
            stream.Str(name, 3);
            stream.I8(ComponentsAnimated);
        }
        else
        {
            MemoryInputStream stream = serializer.GetInput();
            float[] components = stream.Floatarray();
            name = stream.Str(stream.I32());
            ComponentsAnimated = stream.I8();
            if (components.Length == 0) return;

            int numKeys = components.Length / GetNumberOfComponentsAnimated();
            Keys = new Vector4[numKeys];
            for (int i = 0; i < Keys.Length; ++i) Keys[i] = BaseValue;

            int offset = 0;
            for (int i = 0; i < 4; ++i)
            {
                if ((ComponentsAnimated & (1 << i)) != 0)
                {
                    foreach (Vector4 key in Keys) key.SetComponent(i, components[offset++]);
                }
            }
        }
    }

    
    public int GetAllocatedSize()
    {
        int size = MaterialParameterAnimation.BaseAllocationSize;
        if (this.Keys != null)
            size += (this.Keys.Length * 0x10);
        return size;
    }

    public String GetName()
    {
        return this.name;
    }

    public void SetName(String name)
    {
        if (name == null)
            throw new NullReferenceException("Name cannot be null!");
        if (name.Length > NameSize)
            throw new ArgumentException("Name cannot be longer than 3 characters!");
        this.name = name;
    }

    public int GetNumberOfComponentsAnimated()
    {
        int count = 0;
        if ((ComponentsAnimated & 1) != 0) count++;
        if ((ComponentsAnimated & 2) != 0) count++;
        if ((ComponentsAnimated & 4) != 0) count++;
        if ((ComponentsAnimated & 8) != 0) count++;
        return count;
    }

    public bool IsComponentAnimated(int c)
    {
        if (Keys == null) return false;
        return Keys.Any(key => key.GetComponent(c) != BaseValue.GetComponent(c));
    }

    private byte CalculateComponentsAnimated()
    {
        byte flags = 0;
        if (IsComponentAnimated(0)) flags |= 0x1;
        if (IsComponentAnimated(1)) flags |= 0x2;
        if (IsComponentAnimated(2)) flags |= 0x4;
        if (IsComponentAnimated(3)) flags |= 0x8;
        return flags;
    }


}