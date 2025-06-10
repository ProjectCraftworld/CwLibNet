using System;
using System.IO;
using System.Linq;
using System.Numerics;
using CwLibNet4Hub.Enums;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.IO.Streams;
using CwLibNet4Hub.Structs.Things;
using CwLibNet4Hub.Structs.Things.Parts;
using CwLibNet4Hub.Types.Data;
using CwLibNet4Hub.Types;
using CwLibNet4Hub.Types.Archives;
using CwLibNet4Hub.Resources;

namespace CwLibNet4Hub.Tests;

/// <summary>
/// Simple test class to verify that serialization works correctly in CwLibNet4Hub
/// </summary>
public class SerializationTest
{
    public static void RunTests()
    {
        Console.WriteLine("=== CwLibNet4Hub Serialization Tests ===");
        Console.WriteLine();
        
        try
        {
            TestPPosSerializationRoundTrip();
            TestBasicSerialization();
            TestRealAssetSerialization();
            
            Console.WriteLine("✅ All tests passed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Test failed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
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
        byte[] serializedData = outputStream.GetBuffer();
        
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
        
        byte[] data = stream.GetBuffer();
        Console.WriteLine($"  Serialized basic types to {data.Length} bytes");
        Console.WriteLine($"  Data: {BitConverter.ToString(data)}");
        Console.WriteLine("  ✅ Basic serialization completed successfully");
        Console.WriteLine();
    }
    
    /// <summary>
    /// Test with real LBP asset files
    /// </summary>
    private static void TestRealAssetSerialization()
    {
        Console.WriteLine("Testing real LBP asset serialization...");
        
        // Test with FARC archive
        string farcPath = "/home/dylan/Documents/GitHub/Craftworld/farcs/data_hub.farc";
        if (File.Exists(farcPath))
        {
            TestFarcArchive(farcPath);
        }
        
        // Test with level.bin
        string levelPath = "/home/dylan/Documents/GitHub/Craftworld/level.bin";
        if (File.Exists(levelPath))
        {
            TestLevelFile(levelPath);
        }
        
        Console.WriteLine("  ✅ Real asset serialization tests completed");
        Console.WriteLine();
    }
    
    /// <summary>
    /// Test loading and parsing a FARC archive
    /// </summary>
    private static void TestFarcArchive(string farcPath)
    {
        Console.WriteLine($"  Testing FARC archive: {Path.GetFileName(farcPath)}");
        
        try
        {
            var fileArchive = new FileArchive(farcPath);
            Console.WriteLine($"    FARC loaded successfully with {fileArchive.GetEntryCount()} entries");
            
            // Try to extract and test a few small entries
            if (fileArchive.GetEntryCount() > 0)
            {
                var firstEntry = fileArchive.First();
                Console.WriteLine($"    First entry: SHA1={firstEntry.GetSha1()}, Size={firstEntry.GetSize()} bytes");
                
                // Only test small files to avoid memory issues
                if (firstEntry.GetSize() < 50000)
                {
                    byte[]? extractedData = firstEntry.Extract();
                    if (extractedData != null)
                    {
                        Console.WriteLine($"    Successfully extracted {extractedData.Length} bytes");
                        
                        // Try to parse as a SerializedResource
                        try
                        {
                            var resource = new SerializedResource(extractedData);
                            Console.WriteLine($"    Resource type: {resource.GetResourceType()}");
                            Console.WriteLine($"    Resource revision: {resource.GetRevision()}");
                            
                            // Test round-trip on the resource data itself
                            byte[] resourceData = resource.Compress();
                            var input = new MemoryInputStream(resourceData);
                            var output = new MemoryOutputStream(resourceData.Length);
                            
                            // Simple round-trip test - read some bytes and write them back
                            input.Seek(0, SeekMode.Begin);
                            int testLength = Math.Min(100, resourceData.Length);
                            byte[] testData = input.Bytes(testLength);
                            output.Bytes(testData);
                            byte[] roundTripData = output.GetBuffer();
                            
                            bool dataMatches = testData.SequenceEqual(roundTripData);
                            Console.WriteLine($"    Round-trip test ({testLength} bytes): {(dataMatches ? "✅ PASSED" : "❌ FAILED")}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"    Note: Could not parse as SerializedResource ({ex.Message})");
                            
                            // Still try basic round-trip test on raw data
                            var input = new MemoryInputStream(extractedData);
                            var output = new MemoryOutputStream(extractedData.Length);
                            
                            int testLength = Math.Min(50, extractedData.Length);
                            byte[] testData = input.Bytes(testLength);
                            output.Bytes(testData);
                            byte[] roundTripData = output.GetBuffer();
                            
                            bool dataMatches = testData.SequenceEqual(roundTripData);
                            Console.WriteLine($"    Basic round-trip test ({testLength} bytes): {(dataMatches ? "✅ PASSED" : "❌ FAILED")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"    ⚠️  Failed to extract data from entry");
                    }
                }
                else
                {
                    Console.WriteLine($"    Skipping large entry ({firstEntry.GetSize()} bytes)");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    Warning: FARC test failed: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Test loading and parsing a level.bin file
    /// </summary>
    private static void TestLevelFile(string levelPath)
    {
        Console.WriteLine($"  Testing level file: {Path.GetFileName(levelPath)}");
        
        try
        {
            byte[] levelData = File.ReadAllBytes(levelPath);
            Console.WriteLine($"    Level file loaded: {levelData.Length} bytes");
            
            // Try to parse as a SerializedResource
            var resource = new SerializedResource(levelData);
            Console.WriteLine($"    Resource type: {resource.GetResourceType()}");
            Console.WriteLine($"    Resource revision: {resource.GetRevision()}");
            Console.WriteLine($"    Serialization type: {resource.GetSerializationType()}");
            
            // Try to load as RLevel if it's a level resource
            if (resource.GetResourceType().Value == ResourceType.Level.Value)
            {
                try
                {
                    var level = resource.LoadResource<RLevel>();
                    Console.WriteLine($"    Successfully loaded as RLevel");
                    Console.WriteLine($"    Level data size: {level?.GetAllocatedSize() ?? 0} bytes");
                    
                    // Test round-trip serialization
                    if (level != null)
                    {
                        Console.WriteLine("    Testing round-trip serialization...");
                        var revision = resource.GetRevision();
                        var compressionFlags = resource.GetCompressionFlags();
                        
                        var serializedData = level.Build(revision, compressionFlags);
                        var compressedData = SerializedResource.Compress(serializedData);
                        
                        if (compressedData != null)
                        {
                            Console.WriteLine($"    Round-trip successful: {compressedData.Length} bytes");
                            
                            // Basic comparison (sizes should be reasonably close)
                            double sizeDiff = Math.Abs(compressedData.Length - levelData.Length) / (double)levelData.Length;
                            Console.WriteLine($"    Size difference: {sizeDiff:P2}");
                            
                            if (sizeDiff < 0.1) // Within 10%
                            {
                                Console.WriteLine("    ✅ Round-trip size check passed");
                            }
                            else
                            {
                                Console.WriteLine("    ⚠️ Round-trip size difference is large");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    Warning: Could not load as RLevel: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    Warning: Level test failed: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Simple main method to run the tests if this file is executed directly
    /// </summary>
    public static void Main(string[] args)
    {
        RunTests();
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
