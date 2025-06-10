using System;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.Resources;
using CwLibNet4Hub.Types.Data;

namespace CwLibNetTests;

public class RPaletteTests
{

    private readonly byte[]? _bytes =
    [
        12, 0, 2, 178, 211, 2, 0, 2, 179, 211, 2, 0, 2, 180, 211, 2, 
        0, 2, 203, 211, 2, 0, 2, 207, 212, 2, 0, 2, 208, 212, 2, 0, 
        2, 209, 212, 2, 0, 2, 210, 212, 2, 0, 2, 211, 212, 2, 0, 2, 
        212, 212, 2, 0, 2, 213, 212, 2, 0, 2, 214, 212, 2, 0, 0
    ];

    private static readonly GUID[] _plans = [new(43442), new(43443), new (43444), new(43467), new(43599), new(43600), new(43601), new(43602), new(43603), new(43604), new(43605), new(43606)];
    
    [Fact]
    public void Serialize()
    {
        RPalette palette = new RPalette
        {
            PlanList = _plans.Select(guid => new ResourceDescriptor(guid, ResourceType.Plan)).ToList()!,
            Location = 0,
            Description = 0
        };
        SerializationData data = palette.Build(new Revision(Branch.Leerdammer.Head, Branch.Leerdammer.Id, 8), 7);
        Assert.Equal(_bytes, data.Buffer);
    }


    [Fact]
    public void Deserialize()
    {
        RPalette palette = new RPalette();
        Serializer serializer = new Serializer(_bytes, new Revision(Branch.Leerdammer.Head, Branch.Leerdammer.Id, 8), 7);
        palette.Serialize(serializer);
        bool success = palette.PlanList.ToHashSet().SetEquals(_plans.Select(e => new ResourceDescriptor(e, ResourceType.Plan)));
        Assert.True(success);
    }
}