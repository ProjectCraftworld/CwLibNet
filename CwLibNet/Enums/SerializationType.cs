namespace CwLibNet.Enums
{
    public enum SerializationType
    {
        // UNKNOWN(null)
        UNKNOWN,
        // BINARY("b")
        BINARY,
        // TEXT("t")
        TEXT,
        // ENCRYPTED_BINARY("e")
        ENCRYPTED_BINARY,
        // COMPRESSED_TEXTURE(" ")
        COMPRESSED_TEXTURE,
        // GTF_SWIZZLED("s")
        GTF_SWIZZLED,
        // GXT_SWIZZLED("S")
        GXT_SWIZZLED 

        // --------------------
        // TODO enum body members
        // private final String value;
        // SerializationType(String value) {
        //     this.value = value;
        // }
        // public String getValue() {
        //     return this.value;
        // }
        // public static SerializationType fromValue(String value) {
        //     for (SerializationType type : SerializationType.values()) {
        //         if (value.equals(type.value))
        //             return type;
        //     }
        //     return SerializationType.UNKNOWN;
        // }
        // --------------------
    }
}