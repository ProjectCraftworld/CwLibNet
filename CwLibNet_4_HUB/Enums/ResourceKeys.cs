using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public class ResourceKeys
{
    public static readonly int RESOURCE_TYPE = 0;
    public static readonly int RESOURCE_REVISION = 1;
    public static readonly int COMPRESSION_FLAGS = 2;
    public static readonly int DEPENDENCY_MODEL = 3;
    public static readonly int DEPENDENCY_SET = 4;
    public static readonly int HAS_MISSING_DEPENDENCIES = 5;
    public static readonly int CAN_REPLACE_DECOMPRESSED = 6;
    public static readonly int MAX_ENTRIES = 10;
}