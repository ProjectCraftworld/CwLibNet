using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Things;
using CwLibNet.Structs.Things.Parts;
using CwLibNet.Types;

namespace CwLibNetTests;

public class SerializerTests
{
    [Fact]
    public void SerializePPos()
    {
        PPos posTest = new PPos();
        MemoryOutputStream outputStream = new MemoryOutputStream(posTest.GetAllocatedSize());
        Serializer serializer = new Serializer(outputStream, new Revision((int)Revisions.LBP1_MAX));
        serializer.Struct(posTest);
        byte[]? bytes = serializer.GetBuffer();
        byte[]? originalBytes =
        [
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            63,
            128,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            63,
            128,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            63,
            128,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            63,
            128,
            0,
            0,
            63,
            128,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            63,
            128,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            63,
            128,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            63,
            128,
            0,
            0,
        ];
        Assert.Equal(originalBytes, bytes);
    }

    [Fact]
    public void SerializeThing()
    {
        Thing exampleThing = new Thing(2171);
        MemoryOutputStream outputStream = new MemoryOutputStream(exampleThing.GetAllocatedSize());
        Serializer serializer = new Serializer(outputStream, new Revision((int)Revisions.LBP1_MAX));
        serializer.Thing(exampleThing);
        byte[]? bytes = serializer.GetBuffer();
        byte[]? originalBytes =
        [
            0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 8, 123, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0
        ];
        Assert.Equal(originalBytes, bytes);
    }

    [Fact]
    public void DeserializeThing()
    {
        byte[]? originalBytes =
        [
            0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 8, 123, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0
        ];
        MemoryInputStream stream = new MemoryInputStream(originalBytes);
        Serializer serializer = new Serializer(stream, new Revision((int)Revisions.LBP1_MAX));
        Thing? thing = null;
        thing = serializer.Thing(thing);
        Assert.Equal(2171, thing!.Uid);
    }

    [Fact]
    public void SerializeThingWithParts()
    {
        Thing exampleThing = new Thing(2171);
        byte[]? originalBytes =
        [
            0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 8, 123, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 5,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 63, 128, 0, 0, 64, 0, 0,
            0, 64, 64, 0, 0, 64, 128, 0, 0, 64, 160, 0, 0, 64, 192, 0, 0, 64, 224, 0, 0, 65, 0, 0, 0, 65, 16, 0, 0, 65,
            32, 0, 0, 65, 48, 0, 0, 65, 64, 0, 0, 65, 80, 0, 0, 65, 96, 0, 0, 65, 112, 0, 0, 65, 128, 0, 0, 63, 128, 0,
            0, 64, 0, 0, 0, 64, 64, 0, 0, 64, 128, 0, 0, 64, 160, 0, 0, 64, 192, 0, 0, 64, 224, 0, 0, 65, 0, 0, 0, 65,
            16, 0, 0, 65, 32, 0, 0, 65, 48, 0, 0, 65, 64, 0, 0, 65, 80, 0, 0, 65, 96, 0, 0, 65, 112, 0, 0, 65, 128, 0, 0
        ];
        exampleThing.SetPart(Part.Parts["POS"],
            new PPos(new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)));
        MemoryOutputStream outputStream = new MemoryOutputStream(exampleThing.GetAllocatedSize());
        Serializer serializer = new Serializer(outputStream, new Revision((int)Revisions.LBP1_MAX));
        serializer.Thing(exampleThing);
        byte[]? bytes = serializer.GetBuffer();
        Assert.Equal(originalBytes, bytes);
    }

    [Fact]
    public void DeserializeThingWithParts()
    {
        Thing exampleThing = new Thing(2171);
        exampleThing.SetPart(Part.Parts["POS"],
            new PPos(new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16)));
        byte[]? originalBytes =
        [
            0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 8, 123, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0, 0, 0, 0, 0, 5,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 63, 128, 0, 0, 64, 0, 0,
            0, 64, 64, 0, 0, 64, 128, 0, 0, 64, 160, 0, 0, 64, 192, 0, 0, 64, 224, 0, 0, 65, 0, 0, 0, 65, 16, 0, 0, 65,
            32, 0, 0, 65, 48, 0, 0, 65, 64, 0, 0, 65, 80, 0, 0, 65, 96, 0, 0, 65, 112, 0, 0, 65, 128, 0, 0, 63, 128, 0,
            0, 64, 0, 0, 0, 64, 64, 0, 0, 64, 128, 0, 0, 64, 160, 0, 0, 64, 192, 0, 0, 64, 224, 0, 0, 65, 0, 0, 0, 65,
            16, 0, 0, 65, 32, 0, 0, 65, 48, 0, 0, 65, 64, 0, 0, 65, 80, 0, 0, 65, 96, 0, 0, 65, 112, 0, 0, 65, 128, 0, 0
        ];
        MemoryInputStream stream = new MemoryInputStream(originalBytes);
        Serializer serializer = new Serializer(stream, new Revision((int)Revisions.LBP1_MAX));
        Thing? thing = serializer.Thing(null);
        Assert.Equal(2171, thing!.Uid);
        Assert.Equal(new Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
            thing.GetPart<PPos>(Part.Parts["POS"])?.WorldPosition);
    }
}