using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public class SerializationType
{
    // UNKNOWN(null)
    public static SerializationType UNKNOWN = new(null),
        // BINARY("b")
        BINARY = new("b"),
        // TEXT("t")
        TEXT = new("t"),
        // ENCRYPTED_BINARY("e")
        ENCRYPTED_BINARY = new("e"),
        // COMPRESSED_TEXTURE(" ")
        COMPRESSED_TEXTURE = new (" "),
        // GTF_SWIZZLED("s")
        GTF_SWIZZLED = new ("s"),
        // GXT_SWIZZLED("S")
        GXT_SWIZZLED = new ("S");

    private readonly string? value;

    private SerializationType(string? value) {
        this.value = value;
    }
    public string? GetValue() => value;
    public static SerializationType FromValue(string? value)
    {

        return value switch
        {
            null => UNKNOWN,
            "b" => BINARY,
            "t" => TEXT,
            "e" => ENCRYPTED_BINARY,
            " " => COMPRESSED_TEXTURE,
            "s" => GTF_SWIZZLED,
            "S" => GXT_SWIZZLED,
            _ => UNKNOWN
        };
    }
}