using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.Enums;

public enum CellGcmEnumForGtf
{
    // B8(0x81)
    B8 = 0x81,
    // A1R5G5B5(0x82)
    A1R5G5B5 = 0x82,
    // A4R4G4B4(0x83)
    A4R4G4B4 = 0x83,
    // R5G6B5(0x84)
    R5G6B5 = 0x84,
    // A8R8G8B8(0x85)
    A8R8G8B8 = 0x85,
    // DXT1(0x86)
    DXT1 = 0x86,
    // DXT3(0x87)
    DXT3 = 0x87,
    // DXT5(0x88)
    DXT5 = 0x88,
    // G8B8(0x8b)
    G8B8 = 0x8b,
    // R5G5B5(0x8f)
    R5G5B5 = 0x8f
}