using System.Numerics;

namespace CwLibNet.Structs.Mesh;

public class SoftbodyCluster
{
    public static readonly int MAX_CLUSTER_NAME_LENGTH = 0x20;
    public static readonly int QUAD_DYADIC_SUM_LENGTH = 81;

    public static readonly int BASE_ALLOCATION_SIZE =
        MAX_CLUSTER_NAME_LENGTH + (QUAD_DYADIC_SUM_LENGTH * 0x4) + 0x60;

    public String name;
    public Vector4 restCenterOfMass;
    public Matrix4x4 restDyadicSum;
    public float[] restQuadraticDyadicSum = new float[QUAD_DYADIC_SUM_LENGTH];

    public SoftbodyCluster()
    {
        // float9x9
        for (int i = 0; i < QUAD_DYADIC_SUM_LENGTH; ++i)
            if (i % 10 == 0)
                restQuadraticDyadicSum[i] = 1.0f;
    }

    public void setName(String name)
    {
        if (name != null && name.Length > MAX_CLUSTER_NAME_LENGTH)
            throw new ArgumentException("Cluster name cannot be longer than 32 " +
                                        "characters.");
        this.name = name;
    }

    public void setRestQuadraticDyadicSum(float[] sum)
    {
        if (sum == null || sum.Length != QUAD_DYADIC_SUM_LENGTH)
            throw new ArgumentException("Rest quadratic dyadic sum array length must" +
                                        " be " +
                                        "81!");
        restQuadraticDyadicSum = sum;
    }
}