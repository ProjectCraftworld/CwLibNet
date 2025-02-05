using System;
using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.IO.Streams;

namespace Cwlib.Structs.Gmat
{
    public class MaterialParameterAnimation : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x20;
        public static readonly int NAME_SIZE = 0x3;
        public Vector4 baseValue;
        public Vector4[] keys;
        private string name = "";
        public byte componentsAnimated;
        public override void Serialize(Serializer serializer)
        {
            baseValue = serializer.V4(baseValue);
            if (serializer.IsWriting())
            {
                MemoryOutputStream stream = serializer.GetOutput();
                componentsAnimated = CalculateComponentsAnimated();
                if (keys == null || keys.length == 0)
                    stream.i32(0);
                else
                {
                    int count = GetNumberOfComponentsAnimated() * keys.length;
                    stream.i32(count);
                    for (int i = 0; i < 4; ++i)
                    {
                        if ((componentsAnimated & (1 << i)) != 0)
                        {
                            foreach (Vector4 key in keys)
                                stream.f32(key[i]);
                        }
                    }
                }

                stream.i32(3);
                stream.Str(name, 3);
                stream.i8(componentsAnimated);
            }
            else
            {
                MemoryInputStream stream = serializer.GetInput();
                float[] components = stream.Floatarray();
                name = stream.Str(stream.i32());
                componentsAnimated = stream.i8();
                if (components.length == 0)
                    return;
                int numKeys = components.length / GetNumberOfComponentsAnimated();
                keys = new Vector4[numKeys];
                for (int i = 0; i < keys.length; ++i)
                    keys[i] = new Vector4(baseValue);
                int offset = 0;
                for (int i = 0; i < 4; ++i)
                {
                    if ((componentsAnimated & (1 << i)) != 0)
                    {
                        foreach (Vector4 key in keys)
                            key.SetComponent(i, components[offset++]);
                    }
                }
            }
        }

        public virtual int GetAllocatedSize()
        {
            int size = MaterialParameterAnimation.BASE_ALLOCATION_SIZE;
            if (this.keys != null)
                size += (this.keys.length * 0x10);
            return size;
        }

        public virtual string GetName()
        {
            return this.name;
        }

        public virtual void SetName(string name)
        {
            if (name == null)
                throw new NullReferenceException("Name cannot be null!");
            if (name.Length() > NAME_SIZE)
                throw new ArgumentException("Name cannot be longer than 3 characters!");
            this.name = name;
        }

        public virtual int GetNumberOfComponentsAnimated()
        {
            int count = 0;
            if ((componentsAnimated & 1) != 0)
                count++;
            if ((componentsAnimated & 2) != 0)
                count++;
            if ((componentsAnimated & 4) != 0)
                count++;
            if ((componentsAnimated & 8) != 0)
                count++;
            return count;
        }

        public virtual bool IsComponentAnimated(int c)
        {
            if (keys == null)
                return false;
            foreach (Vector4f key in keys)
            {
                if (key[c] != baseValue[c])
                    return true;
            }

            return false;
        }

        private byte CalculateComponentsAnimated()
        {
            byte flags = 0;
            if (IsComponentAnimated(0))
                flags |= 0x1;
            if (IsComponentAnimated(1))
                flags |= 0x2;
            if (IsComponentAnimated(2))
                flags |= 0x4;
            if (IsComponentAnimated(3))
                flags |= 0x8;
            return flags;
        }
    }
}