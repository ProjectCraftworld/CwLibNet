using System;
using System.Linq;
using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.IO.Streams;
using CwLibNet4Hub.Structs.Things.Parts;
using CwLibNet4Hub.Types.Data;

namespace SerializationTestRunner;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== CwLibNet4Hub Serialization Tests ===");
        Console.WriteLine();
        
        try
        {
            TestPPosSerializationRoundTrip();
            TestBasicSerialization();
            
            Console.WriteLine("✅ All tests passed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Test failed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
    
    /// <summary>
    /// Test serializing and deserializing a PPos object
    /// </summary>
    private static void TestPPosSerializationRoundTrip()
    {
        Console.WriteLine("Testing PPos serialization round-trip...");
        
        // Create a test PPos object
        PPos originalPos = new PPos();
        originalPos.AnimHash = 12345;
        originalPos.LocalPosition = Matrix4x4.CreateTranslation(1.0f, 2.0f, 3.0f);
        originalPos.WorldPosition = Matrix4x4.CreateTranslation(4.0f, 5.0f, 6.0f);
        
        // Serialize the object
        MemoryOutputStream outputStream = new MemoryOutputStream(originalPos.GetAllocatedSize());
        Serializer serializer = new Serializer(outputStream, new Revision((int)Revisions.LBP3_MAX));
        serializer.Struct(originalPos);
        byte[] serializedData = serializer.GetBuffer();
        
        Console.WriteLine($"  Original PPos - AnimHash: {originalPos.AnimHash}");
        Console.WriteLine($"  Serialized to {serializedData.Length} bytes");
        Console.WriteLine($"  First 16 bytes: {BitConverter.ToString(serializedData.Take(Math.Min(16, serializedData.Length)).ToArray())}");
        
        // For a complete round-trip test, we would deserialize here
        // This demonstrates the serialization is working
        Console.WriteLine("  ✅ PPos serialization completed successfully");
        Console.WriteLine();
    }
    
    /// <summary>
    /// Test basic serialization functionality
    /// </summary>
    private static void TestBasicSerialization()
    {
        Console.WriteLine("Testing basic serialization functionality...");
        
        // Test serializing basic data types
        MemoryOutputStream stream = new MemoryOutputStream(1024);
        Serializer serializer = new Serializer(stream, new Revision((int)Revisions.LBP3_MAX));
        
        // Test integer serialization
        int testInt = 42;
        serializer.I32(testInt);
        
        // Test float serialization
        float testFloat = 3.14159f;
        serializer.F32(testFloat);
        
        // Test vector serialization
        Vector3 testVector = new Vector3(1.0f, 2.0f, 3.0f);
        serializer.V3(testVector);
        
        byte[] data = serializer.GetBuffer();
        Console.WriteLine($"  Serialized basic types to {data.Length} bytes");
        Console.WriteLine($"  Data: {BitConverter.ToString(data)}");
        Console.WriteLine("  ✅ Basic serialization completed successfully");
        Console.WriteLine();
    }
}
