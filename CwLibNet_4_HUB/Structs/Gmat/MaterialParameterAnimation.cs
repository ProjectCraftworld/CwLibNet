using System.Numerics;
using CwLibNet4Hub.Extensions;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Gmat;

public class MaterialParameterAnimation: ISerializable
{
    public const int BaseAllocationSize = 0x20;
    public const int NameSize = 0x3;

    public Vector4 BaseValue;
    public Vector4[] Keys;
    private string name = "";
    public byte ComponentsAnimated;

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Vector4? tempBaseValue = BaseValue;
        Serializer.SerializeV4(ref tempBaseValue);
        BaseValue = tempBaseValue ?? Vector4.Zero;

        if (Serializer.IsWriting())
        {
            var stream = Serializer.GetCurrentSerializer().GetOutput();
            ComponentsAnimated = CalculateComponentsAnimated();
            if (Keys == null || Keys.Length == 0) stream.I32(0);
            else
            {
                var count = GetNumberOfComponentsAnimated() * Keys.Length;
                stream.I32(count);
                for (var i = 0; i < 4; ++i)
                {
                    if ((ComponentsAnimated & (1 << i)) != 0)
                    {
                        foreach (var key in Keys) stream.F32(key.GetComponent(i));
                    }
                }
            }

            stream.I32(3);
            stream.Str(name, 3);
            stream.I8(ComponentsAnimated);
        }
        else
        {
            var stream = Serializer.GetCurrentSerializer().GetInput();
            var components = stream.Floatarray();
            name = stream.Str(stream.I32());
            ComponentsAnimated = stream.I8();
            if (components.Length == 0) return;

            var numKeys = components.Length / GetNumberOfComponentsAnimated();
            Keys = new Vector4[numKeys];
            for (var i = 0; i < Keys.Length; ++i) Keys[i] = BaseValue;

            var offset = 0;
            for (var i = 0; i < 4; ++i)
            {
                if ((ComponentsAnimated & (1 << i)) != 0)
                {
                    foreach (var key in Keys) key.SetComponent(i, components[offset++]);
                }
            }
        }
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        if (Keys != null)
            size += Keys.Length * 0x10;
        return size;
    }

    public string GetName()
    {
        return name;
    }

    public void SetName(string name)
    {
        if (name == null)
            throw new NullReferenceException("Name cannot be null!");
        if (name.Length > NameSize)
            throw new ArgumentException("Name cannot be longer than 3 characters!");
        this.name = name;
    }

    public int GetNumberOfComponentsAnimated()
    {
        var count = 0;
        if ((ComponentsAnimated & 1) != 0) count++;
        if ((ComponentsAnimated & 2) != 0) count++;
        if ((ComponentsAnimated & 4) != 0) count++;
        if ((ComponentsAnimated & 8) != 0) count++;
        return count;
    }

    public bool IsComponentAnimated(int c)
    {
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