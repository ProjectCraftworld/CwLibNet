using System.Numerics;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;

namespace CwLibNet4Hub.Structs.Mesh;

public class SoftbodyCluster
{
    public static readonly int MAX_CLUSTER_NAME_LENGTH = 0x20;
    public static readonly int QUAD_DYADIC_SUM_LENGTH = 81;

    public static readonly int BASE_ALLOCATION_SIZE =
        MAX_CLUSTER_NAME_LENGTH + QUAD_DYADIC_SUM_LENGTH * 0x4 + 0x60;

    public string Name;
    public Vector4 RestCenterOfMass;
    public Matrix4x4 RestDyadicSum;
    public float[] RestQuadraticDyadicSum = new float[QUAD_DYADIC_SUM_LENGTH];

    public SoftbodyCluster()
    {
        // float9x9
        for (var i = 0; i < QUAD_DYADIC_SUM_LENGTH; ++i)
            if (i % 10 == 0)
                RestQuadraticDyadicSum[i] = 1.0f;
    }

    public void SetName(string name)
    {
        if (name != null && name.Length > MAX_CLUSTER_NAME_LENGTH)
            throw new ArgumentException("Cluster name cannot be longer than 32 " +
                                        "characters.");
        Name = name;
    }

    public void SetRestQuadraticDyadicSum(float[] sum)
    {
        if (sum == null || sum.Length != QUAD_DYADIC_SUM_LENGTH)
            throw new ArgumentException("Rest quadratic dyadic sum array length must" +
                                        " be " +
                                        "81!");
        RestQuadraticDyadicSum = sum;
    }
}