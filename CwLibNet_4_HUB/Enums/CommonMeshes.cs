using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Enums;

public sealed class CommonMeshes
{
        
    public static readonly GUID POLAROID_GUID = new(27162);
    // GUID reference to level theme badge mesh
    public static readonly GUID BUBBLE_GUID = new(68653);
    // GUID reference to DLC Pack badge mesh
    public static readonly GUID LEVEL_BADGE_GUID = new(16006);
    // GUID reference to standard level badge mesh
    public static readonly GUID ADVENTURE_BADGE_GUID = new(642431);
    // GUID reference to adventure level badge mesh
}