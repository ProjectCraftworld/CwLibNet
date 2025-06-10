using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.Extensions;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.Resources;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Structs.Gmat;
using CwLibNet4Hub.Structs.Things;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Gmat;

public class MaterialBox: ISerializable
{
    public const int BaseAllocationSize = 0x80;

    /**
     * Number of parameters used before r0x2a4
     */
    public const int LegacyParameterCount = 0x6;

    public const int ParameterCount = 0x8;

    public int Type;
    private int[] @params = new int[ParameterCount];
    public float X, Y, W, H;
    public int SubType;
    public MaterialParameterAnimation Anim = new();
    public MaterialParameterAnimation Anim2 = new();

    /**
     * Creates an output node
     */
    public MaterialBox() { }

    /**
     * Creates a texture sample node
     */
    public MaterialBox(Vector2 scale, Vector2 offset, int channel, int texture)
    {
        Type = BoxType.TEXTURE_SAMPLE;
        @params[0] = scale.X.FloatToIntBits();
        @params[1] = scale.Y.FloatToIntBits();
        @params[2] = offset.X.FloatToIntBits();
        @params[3] = offset.Y.FloatToIntBits();
        @params[4] = channel;
        @params[5] = texture;
    }

    /**
     * Creates a texture sample node
     */
    public MaterialBox(Vector4 transform, int channel, int texture)
    {
        Type = BoxType.TEXTURE_SAMPLE;
        @params[0] = transform.X.FloatToIntBits();
        @params[1] = transform.Y.FloatToIntBits();
        @params[2] = transform.Z.FloatToIntBits();
        @params[3] = transform.W.FloatToIntBits();
        @params[4] = channel;
        @params[5] = texture;
    }

    /***
     * Creates a color node
     */
    public MaterialBox(Vector4 color)
    {
        Type = BoxType.COLOR;
        @params[0] = color.X.FloatToIntBits();
        @params[1] = color.Y.FloatToIntBits();
        @params[2] = color.Z.FloatToIntBits();
        @params[3] = color.W.FloatToIntBits();
    }

    public float GetTextureRotation()
    {
        if (Type != BoxType.TEXTURE_SAMPLE || SubType != 1) return 0.0f;

        var col0 = new Vector2(@params[0].IntBitsToFloat(),
            @params[1].IntBitsToFloat());
        col0 /= col0.Length();
        return (float) Math.Acos(col0.X);
    }

    public Vector4 GetTextureTransform()
    {
        if (Type != BoxType.TEXTURE_SAMPLE) return new Vector4(1.0f, 1.0f, 0.0f, 0.0f);
        if (SubType != 1)
        {
            return new Vector4(
                @params[0].IntBitsToFloat(),
                @params[1].IntBitsToFloat(),
                @params[2].IntBitsToFloat(),
                @params[3].IntBitsToFloat()
            );
        }
        else
        {
            var col0 = new Vector2(@params[0].IntBitsToFloat(),
                @params[1].IntBitsToFloat());
            var col1 = new Vector2(@params[3].IntBitsToFloat(),
                @params[6].IntBitsToFloat());
            return new Vector4(col0.Length(), col1.Length(),
                @params[2].IntBitsToFloat(),
                @params[7].IntBitsToFloat());
        }
    }

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        Serializer.Serialize(ref Type);

        var head = Serializer.GetCurrentSerializer().GetRevision().GetVersion();

        if (!Serializer.IsWriting()) @params = new int[ParameterCount];
        if (head < 0x2a4)
        {
            for (var i = 0; i < LegacyParameterCount; ++i)
                Serializer.Serialize(ref @params[i]);
        }
        else
        {
            int paramCount = ParameterCount;
            Serializer.Serialize(ref paramCount);
            for (var i = 0; i < ParameterCount; ++i)
                Serializer.Serialize(ref @params[i]);
        }

        Serializer.Serialize(ref X);
        Serializer.Serialize(ref Y);
        Serializer.Serialize(ref W);
        Serializer.Serialize(ref H);

        if (head > 0x2a3)
            Serializer.Serialize(ref SubType);

        if (head > 0x2a1)
            Serializer.Serialize(ref Anim);
        if (head > 0x2a3)
            Serializer.Serialize(ref Anim2);
    }

    
    public int GetAllocatedSize()
    {
        var size = BaseAllocationSize;
        size += Anim.GetAllocatedSize();
        size += Anim2.GetAllocatedSize();
        return size;
    }

    public int[] GetParameters()
    {
        return @params;
    }

    public bool IsColor()
    {
        return Type == BoxType.COLOR;
    }

    public bool IsTexture()
    {
        return Type == BoxType.TEXTURE_SAMPLE;
    }

    public bool IsMultiply()
    {
        return Type == BoxType.MULTIPLY;
    }

    public bool IsSimpleMultiply(RGfxMaterial material)
    {
        if (!IsMultiply()) return false;
        var ports = material.GetBoxesConnected(this);
        if (ports.Length != 2) return false;

        if (ports[0].Type == BoxType.COLOR && ports[1].Type == BoxType.TEXTURE_SAMPLE)
            return true;
        return ports[0].Type == BoxType.TEXTURE_SAMPLE && ports[1].Type == BoxType.COLOR;
    }

    /* public void toXML(RGfxMaterial material, XmlFormatter xml)
    {
        switch (type)
        {
            case BoxType.OUTPUT:
                return;
            case BoxType.TEXTURE_SAMPLE:
            {

                xml.startTag("Texture");

                float rotation = 0.0f;
                int channel = params[4];
                int texunit = params[5];
                Vector2 scale, offset;

                if (subType == 0)
                {
                    scale = new Vector2(
                        Float.intBitsToFloat(params[0]),
                        Float.intBitsToFloat(params[1])
                    );

                    offset = new Vector2(
                        Float.intBitsToFloat(params[2]),
                        Float.intBitsToFloat(params[3])
                    );
                }
                else
                {
                    float[] rotation_matrix = new float[]
                        {
                            Float.intBitsToFloat(params[0]),
                            Float.intBitsToFloat(params[3]),
                            Float.intBitsToFloat(params[1]),
                            Float.intBitsToFloat(params[6]),
                            Float.intBitsToFloat(params[2]),
                            Float.intBitsToFloat(params[7])
                        };

                    Vector2 col0 = new Vector2(rotation_matrix[0], rotation_matrix[1]);
                    Vector2 col1 = new Vector2(rotation_matrix[2], rotation_matrix[3]);

                    scale = new Vector2(col0.Length, col1.Length);

                    col0.div(scale.x);
                    col1.div(scale.y);

                    rotation = (float) Math.acos(col0.x);

                    offset = new Vector2(rotation_matrix[4], rotation_matrix[5]);

                    xml.startTag("RotationMatrix");
                    xml.addTag("Row", String.format("%f, %f, %f", rotation_matrix[0],
                        rotation_matrix[2], rotation_matrix[4]));
                    xml.addTag("Row", String.format("%f, %f, %f", rotation_matrix[1],
                        rotation_matrix[3], rotation_matrix[5]));
                    xml.endTag();
                }

                offset.x = (float) (offset.x + scale.y * Math.sin(rotation));
                offset.y = (float) (1.0f - offset.y - scale.y * Math.cos(rotation));

                xml.startTag("Scale");
                xml.addTag("x", scale.x);
                xml.addTag("y", scale.y);
                xml.endTag();

                xml.startTag("Offset");
                xml.addTag("x", offset.x);
                xml.addTag("y", offset.y);
                xml.endTag();

                xml.addTag("Rotation", String.format("%.1fdeg", Math.toDegrees(rotation)));

                xml.addTag("Channel", channel);
                xml.addTag("Texture", texunit);

                xml.endTag();

                break;
            }
            case BoxType.THING_COLOR:
            {
                xml.addTag("Color", "type=\"Thing\"", null);
                break;
            }
            case BoxType.COLOR:
            {
                float[] rgba = new float[]
                    {
                        Float.intBitsToFloat(params[0]),
                        Float.intBitsToFloat(params[1]),
                        Float.intBitsToFloat(params[2]),
                        Float.intBitsToFloat(params[3]),
                    };

                String hex = String.format("#%02X%02X%02X%02X",
                    (int) (rgba[0] * 255.0f),
                    (int) (rgba[1] * 255.0f),
                    (int) (rgba[2] * 255.0f),
                    (int) (rgba[3] * 255.0f)
                );

                xml.startTag("Color", "type=\"RGBA\"");
                xml.addTag("Float4", String.format("%f, %f, %f, %f", rgba[0], rgba[1],
                    rgba[2],
                    rgba[3]));
                xml.addTag("Hex", hex);
                xml.endTag();

                break;
            }

            case BoxType.CONSTANT:
            {
                xml.addTag("Float", params[0]);
                break;
            }

            case BoxType.CONSTANT2:
            {
                xml.addTag("Float2", String.format("%f, %f", params[0], params[1]));
                break;
            }

            case BoxType.CONSTANT3:
            {
                xml.addTag("Float3", String.format("%f, %f", params[0], params[1],
                    params[2]));
                break;
            }

            case BoxType.CONSTANT4:
            {
                xml.addTag("Float4", String.format("%f, %f, %f, %f", params[0], params[1],
                    params[2], params[3]));
                break;
            }

            case BoxType.MULTIPLY_ADD:
            {
                xml.startTag("MultiplyAdd");
                xml.startTag("Value");
                material.getBoxConnectedToPort(this, 0).toXML(material, xml);
                xml.endTag();
                xml.addTag("Multiplier", Float.intBitsToFloat(params[0]));
                xml.addTag("Addend", Float.intBitsToFloat(params[1]));
                xml.endTag();

                break;
            }

            case BoxType.MULTIPLY:
            {
                xml.startTag("Multiply");
                xml.startTag("Value");
                material.getBoxConnectedToPort(this, 0).toXML(material, xml);
                xml.endTag();
                xml.startTag("Value");
                material.getBoxConnectedToPort(this, 1).toXML(material, xml);
                xml.endTag();
                xml.endTag();
                break;
            }

            case BoxType.ADD:
            {
                xml.startTag("Add");
                xml.startTag("Value");
                material.getBoxConnectedToPort(this, 0).toXML(material, xml);
                xml.endTag();
                xml.startTag("Value");
                material.getBoxConnectedToPort(this, 1).toXML(material, xml);
                xml.endTag();
                xml.endTag();
                break;
            }

            default:
            {
                xml.startTag("Unsupported");
                xml.addTag("Message", "This node isn't supported, go yell at Aidan");
                xml.endTag();
            }
        }
    } */


}