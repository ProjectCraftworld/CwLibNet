using System.IO;
using System.Text.Json;
using CwLibNet4Hub.IO.Serializer;
using CwLibNet4Hub.IO.Streams;
using CwLibNet4Hub.Resources;
using CwLibNet4Hub.Types;
using CwLibNet4Hub.Types.Data;

namespace CwLibNetTests;

public class PodTests
{
    private const string PodMolPath = "/home/dylan/Documents/GitHub/Craftworld/gamedata/mesh_library/cosmos/pod/pod.mol";
    private const string PodJsonPath = "/home/dylan/Documents/pod.json";
    
    [Fact]
    public void LoadPodMolFile()
    {
        // Arrange
        if (!File.Exists(PodMolPath))
        {
            throw new FileNotFoundException($"Pod MOL file not found at: {PodMolPath}");
        }
        
        // Act
        byte[] molData = File.ReadAllBytes(PodMolPath);
        
        // Assert
        Assert.True(molData.Length > 0, "MOL file should not be empty");
        
        // Verify it's a valid SerializedResource
        var resource = new SerializedResource(molData);
        Assert.NotNull(resource);
        
        Console.WriteLine($"✅ Pod MOL file loaded successfully: {molData.Length} bytes");
    }
    
    [Fact]
    public void DeserializePodMolAsRMesh()
    {
        // Arrange
        if (!File.Exists(PodMolPath))
        {
            throw new FileNotFoundException($"Pod MOL file not found at: {PodMolPath}");
        }
        
        byte[] molData = File.ReadAllBytes(PodMolPath);
        var resource = new SerializedResource(molData);
        
        // Debug output
        Console.WriteLine($"Resource Type: {resource.GetResourceType()}");
        Console.WriteLine($"Serialization Type: {resource.GetSerializationType()}");
        Console.WriteLine($"Revision: {resource.GetRevision()}");
        Console.WriteLine($"Compression Flags: {resource.GetCompressionFlags()}");
        
        // Act
        RMesh? mesh = resource.LoadResource<RMesh>();
        
        // Assert
        Assert.NotNull(mesh);
        
        // Verify mesh properties
        Assert.True(mesh.NumVerts > 0, "Mesh should have vertices");
        Assert.True(mesh.NumIndices > 0, "Mesh should have indices");
        
        // Print some debug info
        Console.WriteLine($"Pod mesh - Vertices: {mesh.NumVerts}, Indices: {mesh.NumIndices}");
    }
    
    [Fact]
    public void ComparePodMolWithJsonData()
    {
        // Arrange
        if (!File.Exists(PodMolPath))
        {
            throw new FileNotFoundException($"Pod MOL file not found at: {PodMolPath}");
        }
        
        if (!File.Exists(PodJsonPath))
        {
            throw new FileNotFoundException($"Pod JSON file not found at: {PodJsonPath}");
        }
        
        // Load MOL file
        byte[] molData = File.ReadAllBytes(PodMolPath);
        var resource = new SerializedResource(molData);
        RMesh? mesh = resource.LoadResource<RMesh>();
        
        // Load JSON file
        string jsonContent = File.ReadAllText(PodJsonPath);
        using JsonDocument jsonDoc = JsonDocument.Parse(jsonContent);
        JsonElement root = jsonDoc.RootElement;
        
        // Act & Assert
        Assert.NotNull(mesh);
        
        // Compare basic properties if they exist in JSON
        if (root.TryGetProperty("resource", out JsonElement resourceElement))
        {
            if (resourceElement.TryGetProperty("numVerts", out JsonElement numVertsElement))
            {
                int jsonNumVerts = numVertsElement.GetInt32();
                Assert.Equal(jsonNumVerts, mesh.NumVerts);
                Console.WriteLine($"✅ Vertex count matches: MOL={mesh.NumVerts}, JSON={jsonNumVerts}");
            }
            
            if (resourceElement.TryGetProperty("numIndices", out JsonElement numIndicesElement))
            {
                int jsonNumIndices = numIndicesElement.GetInt32();
                Assert.Equal(jsonNumIndices, mesh.NumIndices);
                Console.WriteLine($"✅ Index count matches: MOL={mesh.NumIndices}, JSON={jsonNumIndices}");
            }
            
            if (resourceElement.TryGetProperty("numTris", out JsonElement numTrisElement))
            {
                int jsonNumTris = numTrisElement.GetInt32();
                // Note: numTris might be calculated differently, so just log it
                Console.WriteLine($"Triangle count - MOL mesh indices/3: {mesh.NumIndices/3}, JSON: {jsonNumTris}");
            }
        }
    }
    
    [Fact]
    public void RoundTripPodMol()
    {
        // Arrange
        if (!File.Exists(PodMolPath))
        {
            throw new FileNotFoundException($"Pod MOL file not found at: {PodMolPath}");
        }
        
        byte[] originalData = File.ReadAllBytes(PodMolPath);
        var originalResource = new SerializedResource(originalData);
        RMesh? originalMesh = originalResource.LoadResource<RMesh>();
        
        // Act - Serialize back to bytes
        Assert.NotNull(originalMesh);
        
        MemoryOutputStream outputStream = new MemoryOutputStream(originalMesh.GetAllocatedSize());
        Serializer serializer = new Serializer(outputStream, new Revision(880)); // Using revision from JSON
        originalMesh.Serialize(serializer);
        byte[]? serializedData = serializer.GetBuffer();
        
        // Create new resource from serialized data
        Assert.NotNull(serializedData);
        var newResource = new SerializedResource(serializedData);
        RMesh? newMesh = newResource.LoadResource<RMesh>();
        
        // Assert
        Assert.NotNull(newMesh);
        Assert.Equal(originalMesh.NumVerts, newMesh.NumVerts);
        Assert.Equal(originalMesh.NumIndices, newMesh.NumIndices);
        
        Console.WriteLine($"✅ Round-trip successful - Vertices: {newMesh.NumVerts}, Indices: {newMesh.NumIndices}");
    }
}
