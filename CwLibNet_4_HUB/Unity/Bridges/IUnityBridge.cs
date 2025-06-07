using static CwLibNet.IO.Serializer.Serializer;
using System;
using System.Numerics;

#if UNITY_2021_3_OR_NEWER
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using CwLibNet.IO.Serializer;
#endif

namespace CwLibNet.Unity.Bridges
{
    /// <summary>
    /// Interface for Unity compatibility bridges.
    /// Provides conversion between authentic LBP data structures and Unity types.
    /// </summary>
    public interface IUnityBridge<TLbpType, TUnityType>
    {
        /// <summary>Gets the authentic LBP data structure.</summary>
        TLbpType LbpData { get; }
        
        /// <summary>Converts to Unity-compatible format.</summary>
        TUnityType ToUnity();
        
        /// <summary>Updates from Unity format.</summary>
        void FromUnity(TUnityType unityData);
    }
    
    /// <summary>
    /// Static utility class for common Unity/LBP conversions.
    /// Preserves the authentic LBP Method while providing Unity integration.
    /// </summary>
    public static class UnityConversions
    {
#if UNITY_2021_3_OR_NEWER
        
        /// <summary>Convert System.Numerics.Vector4 to UnityEngine.Vector3.</summary>
        public static Vector3 ToUnityVector3(Vector4 lbpVector)
        {
            return new Vector3(lbpVector.X, lbpVector.Y, lbpVector.Z);
        }
        
        /// <summary>Convert UnityEngine.Vector3 to System.Numerics.Vector4 (with w=1).</summary>
        public static Vector4 ToLbpVector4(Vector3 unityVector)
        {
            return new Vector4(unityVector.x, unityVector.y, unityVector.z, 1.0f);
        }
        
        /// <summary>Convert System.Numerics.Vector2 to UnityEngine.Vector2.</summary>
        public static UnityEngine.Vector2 ToUnityVector2(Vector2 lbpVector)
        {
            return new UnityEngine.Vector2(lbpVector.X, lbpVector.Y);
        }
        
        /// <summary>Convert UnityEngine.Vector2 to System.Numerics.Vector2.</summary>
        public static Vector2 ToLbpVector2(UnityEngine.Vector2 unityVector)
        {
            return new Vector2(unityVector.x, unityVector.y);
        }
        
        /// <summary>Convert System.Numerics.Quaternion to UnityEngine.Quaternion.</summary>
        public static Quaternion ToUnityQuaternion(System.Numerics.Quaternion lbpQuaternion)
        {
            return new Quaternion(lbpQuaternion.X, lbpQuaternion.Y, lbpQuaternion.Z, lbpQuaternion.W);
        }
        
        /// <summary>Convert UnityEngine.Quaternion to System.Numerics.Quaternion.</summary>
        public static System.Numerics.Quaternion ToLbpQuaternion(Quaternion unityQuaternion)
        {
            return new System.Numerics.Quaternion(unityQuaternion.x, unityQuaternion.y, unityQuaternion.z, unityQuaternion.w);
        }
        
        /// <summary>Convert array of LBP Vector4 to Unity Vector3 array.</summary>
        public static Vector3[] ToUnityVector3Array(Vector4[]? lbpVectors)
        {
            if (lbpVectors == null) return Array.Empty<Vector3>();
            
            var result = new Vector3[lbpVectors.Length];
            for (int i = 0; i < lbpVectors.Length; i++)
            {
                result[i] = ToUnityVector3(lbpVectors[i]);
            }
            return result;
        }
        
        /// <summary>Convert Unity Vector3 array to LBP Vector4 array.</summary>
        public static Vector4[] ToLbpVector4Array(Vector3[]? unityVectors)
        {
            if (unityVectors == null) return Array.Empty<Vector4>();
            
            var result = new Vector4[unityVectors.Length];
            for (int i = 0; i < unityVectors.Length; i++)
            {
                result[i] = ToLbpVector4(unityVectors[i]);
            }
            return result;
        }
        
#else
        
        // Fallback implementations for non-Unity environments
        public static (float x, float y, float z) ToUnityVector3(Vector4 lbpVector)
        {
            return (lbpVector.X, lbpVector.Y, lbpVector.Z);
        }
        
        public static Vector4 ToLbpVector4((float x, float y, float z) unityVector)
        {
            return new Vector4(unityVector.x, unityVector.y, unityVector.z, 1.0f);
        }
        
#endif
    }
}
