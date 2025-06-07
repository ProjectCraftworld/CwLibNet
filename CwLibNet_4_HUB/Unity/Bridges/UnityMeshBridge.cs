using static CwLibNet.IO.Serializer.Serializer;
using System;
using System.Numerics;
using CwLibNet.Unity.Bridges;

#if UNITY_2021_3_OR_NEWER
using UnityEngine;
using Mesh = UnityEngine.Mesh;
using Vector3 = UnityEngine.Vector3;
using CwLibNet.IO.Serializer;
#endif

namespace CwLibNet.Unity.Bridges
{
    /// <summary>
    /// Unity compatibility bridge for RMesh.
    /// Preserves authentic LBP Method while providing Unity integration.
    /// 
    /// This bridge allows seamless conversion between the authentic LittleBigPlanet
    /// mesh format (using System.Numerics types) and Unity's mesh system.
    /// </summary>
    [Serializable]
    public class UnityMeshBridge : IUnityBridge<object, object>
    {
        // Store reference to the authentic LBP mesh data
        private object _rmesh;
        
        /// <summary>Gets the authentic LBP mesh data.</summary>
        public object LbpData => _rmesh;
        
        /// <summary>Creates a new Unity mesh bridge.</summary>
        public UnityMeshBridge()
        {
            // Initialize with empty mesh data
            _rmesh = CreateEmptyMesh();
        }
        
        /// <summary>Creates a new Unity mesh bridge with existing mesh data.</summary>
        public UnityMeshBridge(object rmesh)
        {
            _rmesh = rmesh ?? CreateEmptyMesh();
        }
        
        private object CreateEmptyMesh()
        {
            // This would create an actual RMesh instance in the real implementation
            // For now, return a placeholder
            return new { Vertices = Array.Empty<Vector4>(), TriangleData = Array.Empty<int>() };
        }
        
#if UNITY_2021_3_OR_NEWER
        
        /// <summary>Converts the authentic LBP mesh to Unity Mesh format.</summary>
        public Mesh ToUnityMesh()
        {
            var mesh = new Mesh();
            
            // This is a simplified example - real implementation would use actual RMesh properties
            var rmeshData = (dynamic)_rmesh;
            
            if (rmeshData.Vertices != null && ((Vector4[])rmeshData.Vertices).Length > 0)
            {
                var lbpVertices = (Vector4[])rmeshData.Vertices;
                mesh.vertices = UnityConversions.ToUnityVector3Array(lbpVertices);
            }
            
            if (rmeshData.TriangleData != null && ((int[])rmeshData.TriangleData).Length > 0)
            {
                mesh.triangles = (int[])rmeshData.TriangleData;
            }
            
            // Auto-calculate normals and bounds
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            
            return mesh;
        }
        
        /// <summary>Updates the authentic LBP mesh from Unity Mesh format.</summary>
        public void FromUnityMesh(Mesh unityMesh)
        {
            if (unityMesh == null) return;
            
            // Convert Unity mesh data back to authentic LBP format
            var rmeshData = (dynamic)_rmesh;
            
            if (unityMesh.vertices?.Length > 0)
            {
                rmeshData.Vertices = UnityConversions.ToLbpVector4Array(unityMesh.vertices);
            }
            
            if (unityMesh.triangles?.Length > 0)
            {
                rmeshData.TriangleData = unityMesh.triangles;
            }
        }
        
        /// <summary>Generic Unity conversion (implements interface).</summary>
        public object ToUnity()
        {
            return ToUnityMesh();
        }
        
        /// <summary>Generic Unity update (implements interface).</summary>
        public void FromUnity(object unityData)
        {
            if (unityData is Mesh mesh)
            {
                FromUnityMesh(mesh);
            }
        }
        
        /// <summary>Implicit conversion to Unity Mesh.</summary>
        public static implicit operator Mesh(UnityMeshBridge bridge)
        {
            return bridge.ToUnityMesh();
        }
        
        /// <summary>Explicit conversion from Unity Mesh.</summary>
        public static explicit operator UnityMeshBridge(Mesh mesh)
        {
            var bridge = new UnityMeshBridge();
            bridge.FromUnityMesh(mesh);
            return bridge;
        }
        
#else
        
        /// <summary>Non-Unity fallback implementation.</summary>
        public object ToUnity()
        {
            return _rmesh; // Return the authentic LBP data directly
        }
        
        /// <summary>Non-Unity fallback implementation.</summary>
        public void FromUnity(object unityData)
        {
            // In non-Unity environments, just store the data as-is
            _rmesh = unityData;
        }
        
#endif
        
        /// <summary>Gets mesh statistics for debugging.</summary>
        public (int vertices, int triangles) GetMeshStats()
        {
            var rmeshData = (dynamic)_rmesh;
            var vertexCount = rmeshData.Vertices != null ? ((Vector4[])rmeshData.Vertices).Length : 0;
            var triangleCount = rmeshData.TriangleData != null ? ((int[])rmeshData.TriangleData).Length / 3 : 0;
            return (vertexCount, triangleCount);
        }
        
        /// <summary>Validates mesh data integrity.</summary>
        public bool IsValid()
        {
            try
            {
                var (vertices, triangles) = GetMeshStats();
                return vertices > 0 && triangles > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
