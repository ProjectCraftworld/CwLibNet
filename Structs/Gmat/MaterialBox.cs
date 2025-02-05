using System;
using CwLibNet.Enums;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.Resources;
using CwLibNet.Util;

namespace Cwlib.Structs.Gmat
{
    public class MaterialBox : ISerializable
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x80;
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        public static readonly int LEGACY_PARAMETER_COUNT = 0x6;
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        public static readonly int PARAMETER_COUNT = 0x8;
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        public int type;
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        private int[] parameters = new int[PARAMETER_COUNT];
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        public float x, y, w, h;
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        public int subType;
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        public MaterialParameterAnimation anim = new MaterialParameterAnimation();
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        public MaterialParameterAnimation anim2 = new MaterialParameterAnimation();
        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        public MaterialBox()
        {
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        public MaterialBox(Vector2f scale, Vector2f offset, int channel, int texture)
        {
            this.type = BoxType.TEXTURE_SAMPLE;
            this.@parameters[0] = Float.FloatToIntBits(scale.x);
            this.@parameters[1] = Float.FloatToIntBits(scale.y);
            this.@parameters[2] = Float.FloatToIntBits(offset.x);
            this.@parameters[3] = Float.FloatToIntBits(offset.y);
            this.@parameters[4] = channel;
            this.@parameters[5] = texture;
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        public MaterialBox(Vector4f transform, int channel, int texture)
        {
            this.type = BoxType.TEXTURE_SAMPLE;
            this.@parameters[0] = Float.FloatToIntBits(transform.x);
            this.@parameters[1] = Float.FloatToIntBits(transform.y);
            this.@parameters[2] = Float.FloatToIntBits(transform.z);
            this.@parameters[3] = Float.FloatToIntBits(transform.w);
            this.@parameters[4] = channel;
            this.@parameters[5] = texture;
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public MaterialBox(Vector4f color)
        {
            this.type = BoxType.COLOR;
            this.@parameters[0] = Float.FloatToIntBits(color.x);
            this.@parameters[1] = Float.FloatToIntBits(color.y);
            this.@parameters[2] = Float.FloatToIntBits(color.z);
            this.@parameters[3] = Float.FloatToIntBits(color.w);
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual float GetTextureRotation()
        {
            if (type != BoxType.TEXTURE_SAMPLE || subType != 1)
                return 0F;
            Vector2f col0 = new Vector2f(Float.IntBitsToFloat(@parameters[0]), Float.IntBitsToFloat(@parameters[1]));
            col0.Div(col0.Length());
            return (float)Math.Acos(col0.x);
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual Vector4f GetTextureTransform()
        {
            if (type != BoxType.TEXTURE_SAMPLE)
                return new Vector4f(1F, 1F, 0F, 0F);
            if (subType != 1)
            {
                return new Vector4f(Float.IntBitsToFloat(@parameters[0]), Float.IntBitsToFloat(@parameters[1]), Float.IntBitsToFloat(@parameters[2]), Float.IntBitsToFloat(@parameters[3]));
            }
            else
            {
                Vector2f col0 = new Vector2f(Float.IntBitsToFloat(@parameters[0]), Float.IntBitsToFloat(@parameters[1]));
                Vector2f col1 = new Vector2f(Float.IntBitsToFloat(@parameters[3]), Float.IntBitsToFloat(@parameters[6]));
                return new Vector4f(col0.Length(), col1.Length(), Float.IntBitsToFloat(@parameters[2]), Float.IntBitsToFloat(@parameters[7]));
            }
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public override void Serialize(Serializer serializer)
        {
            type = serializer.I32(type);
            int head = serializer.GetRevision().GetVersion();
            if (!serializer.IsWriting())
                @parameters = new int[PARAMETER_COUNT];
            if (head < 0x2a4)
            {
                for (int i = 0; i < LEGACY_PARAMETER_COUNT; ++i)
                    @parameters[i] = serializer.I32(@parameters[i]);
            }
            else
            {
                serializer.I32(PARAMETER_COUNT);
                for (int i = 0; i < PARAMETER_COUNT; ++i)
                    @parameters[i] = serializer.I32(@parameters[i]);
            }

            x = serializer.F32(x);
            y = serializer.F32(y);
            w = serializer.F32(w);
            h = serializer.F32(h);
            if (head > 0x2a3)
                subType = serializer.I32(subType);
            if (head > 0x2a1)
                anim = serializer.Struct(anim, typeof(MaterialParameterAnimation));
            if (head > 0x2a3)
                anim2 = serializer.Struct(anim2, typeof(MaterialParameterAnimation));
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual int GetAllocatedSize()
        {
            int size = MaterialBox.BASE_ALLOCATION_SIZE;
            size += this.anim.GetAllocatedSize();
            size += this.anim2.GetAllocatedSize();
            return size;
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual int[] GetParameters()
        {
            return this.@parameters;
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual bool IsColor()
        {
            return this.type == BoxType.COLOR;
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual bool IsTexture()
        {
            return this.type == BoxType.TEXTURE_SAMPLE;
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual bool IsMultiply()
        {
            return this.type == BoxType.MULTIPLY;
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual bool IsSimpleMultiply(RGfxMaterial material)
        {
            if (!IsMultiply())
                return false;
            MaterialBox[] ports = material.GetBoxesConnected(this);
            if (ports.length != 2)
                return false;
            if (ports[0].type == BoxType.COLOR && ports[1].type == BoxType.TEXTURE_SAMPLE)
                return true;
            return ports[0].type == BoxType.TEXTURE_SAMPLE && ports[1].type == BoxType.COLOR;
        }

        /// <summary>
        /// Number of parameters used before r0x2a4
        /// </summary>
        /// <summary>
        /// Creates an output node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a texture sample node
        /// </summary>
        /// <summary>
        /// Creates a color node
        /// </summary>
        public virtual void ToXML(RGfxMaterial material, XmlFormatter xml)
        {
            switch (type)
            {
                case BoxType.OUTPUT:
                    return;
                case BoxType.TEXTURE_SAMPLE:
                {
                    xml.StartTag("Texture");
                    float rotation = 0F;
                    int channel = @parameters[4];
                    int texunit = @parameters[5];
                    Vector2f scale, offset;
                    if (subType == 0)
                    {
                        scale = new Vector2f(Float.IntBitsToFloat(@parameters[0]), Float.IntBitsToFloat(@parameters[1]));
                        offset = new Vector2f(Float.IntBitsToFloat(@parameters[2]), Float.IntBitsToFloat(@parameters[3]));
                    }
                    else
                    {
                        float[] rotation_matrix = new float[]
                        {
                            Float.IntBitsToFloat(@parameters[0]),
                            Float.IntBitsToFloat(@parameters[3]),
                            Float.IntBitsToFloat(@parameters[1]),
                            Float.IntBitsToFloat(@parameters[6]),
                            Float.IntBitsToFloat(@parameters[2]),
                            Float.IntBitsToFloat(@parameters[7])
                        };
                        Vector2f col0 = new Vector2f(rotation_matrix[0], rotation_matrix[1]);
                        Vector2f col1 = new Vector2f(rotation_matrix[2], rotation_matrix[3]);
                        scale = new Vector2f(col0.Length(), col1.Length());
                        col0.Div(scale.x);
                        col1.Div(scale.y);
                        rotation = (float)Math.Acos(col0.x);
                        offset = new Vector2f(rotation_matrix[4], rotation_matrix[5]);
                        xml.StartTag("RotationMatrix");
                        xml.AddTag("Row", String.Format("%f, %f, %f", rotation_matrix[0], rotation_matrix[2], rotation_matrix[4]));
                        xml.AddTag("Row", String.Format("%f, %f, %f", rotation_matrix[1], rotation_matrix[3], rotation_matrix[5]));
                        xml.EndTag();
                    }

                    offset.x = (float)(offset.x + scale.y * Math.Sin(rotation));
                    offset.y = (float)(1F - offset.y - scale.y * Math.Cos(rotation));
                    xml.StartTag("Scale");
                    xml.AddTag("x", scale.x);
                    xml.AddTag("y", scale.y);
                    xml.EndTag();
                    xml.StartTag("Offset");
                    xml.AddTag("x", offset.x);
                    xml.AddTag("y", offset.y);
                    xml.EndTag();
                    xml.AddTag("Rotation", String.Format("%.1fdeg", Math.ToDegrees(rotation)));
                    xml.AddTag("Channel", channel);
                    xml.AddTag("Texture", texunit);
                    xml.EndTag();
                    break;
                }

                case BoxType.THING_COLOR:
                {
                    xml.AddTag("Color", "type=\"Thing\"", null);
                    break;
                }

                case BoxType.COLOR:
                {
                    float[] rgba = new float[]
                    {
                        Float.IntBitsToFloat(@parameters[0]),
                        Float.IntBitsToFloat(@parameters[1]),
                        Float.IntBitsToFloat(@parameters[2]),
                        Float.IntBitsToFloat(@parameters[3])
                    };
                    string hex = String.Format("#%02X%02X%02X%02X", (int)(rgba[0] * 255F), (int)(rgba[1] * 255F), (int)(rgba[2] * 255F), (int)(rgba[3] * 255F));
                    xml.StartTag("Color", "type=\"RGBA\"");
                    xml.AddTag("Float4", String.Format("%f, %f, %f, %f", rgba[0], rgba[1], rgba[2], rgba[3]));
                    xml.AddTag("Hex", hex);
                    xml.EndTag();
                    break;
                }

                case BoxType.CONSTANT:
                {
                    xml.AddTag("Float", @parameters[0]);
                    break;
                }

                case BoxType.CONSTANT2:
                {
                    xml.AddTag("Float2", String.Format("%f, %f", @parameters[0], @parameters[1]));
                    break;
                }

                case BoxType.CONSTANT3:
                {
                    xml.AddTag("Float3", String.Format("%f, %f", @parameters[0], @parameters[1], @parameters[2]));
                    break;
                }

                case BoxType.CONSTANT4:
                {
                    xml.AddTag("Float4", String.Format("%f, %f, %f, %f", @parameters[0], @parameters[1], @parameters[2], @parameters[3]));
                    break;
                }

                case BoxType.MULTIPLY_ADD:
                {
                    xml.StartTag("MultiplyAdd");
                    xml.StartTag("Value");
                    material.GetBoxConnectedToPort(this, 0).ToXML(material, xml);
                    xml.EndTag();
                    xml.AddTag("Multiplier", Float.IntBitsToFloat(@parameters[0]));
                    xml.AddTag("Addend", Float.IntBitsToFloat(@parameters[1]));
                    xml.EndTag();
                    break;
                }

                case BoxType.MULTIPLY:
                {
                    xml.StartTag("Multiply");
                    xml.StartTag("Value");
                    material.GetBoxConnectedToPort(this, 0).ToXML(material, xml);
                    xml.EndTag();
                    xml.StartTag("Value");
                    material.GetBoxConnectedToPort(this, 1).ToXML(material, xml);
                    xml.EndTag();
                    xml.EndTag();
                    break;
                }

                case BoxType.ADD:
                {
                    xml.StartTag("Add");
                    xml.StartTag("Value");
                    material.GetBoxConnectedToPort(this, 0).ToXML(material, xml);
                    xml.EndTag();
                    xml.StartTag("Value");
                    material.GetBoxConnectedToPort(this, 1).ToXML(material, xml);
                    xml.EndTag();
                    xml.EndTag();
                    break;
                }

                default:
                {
                    xml.StartTag("Unsupported");
                    xml.AddTag("Message", "This node isn't supported, go yell at Aidan");
                    xml.EndTag();
                }

                    break;
            }
        }
    }
}