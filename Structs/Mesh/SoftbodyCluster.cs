using System;
using System.Numerics;

namespace CwLibNet.Structs.Mesh
{
    /// <summary>
    /// Data used for softbody calculations.
    /// I'm going to be honest, if anybody can tell me what
    /// any of these words mean, that'd be great.
    /// </summary>
    public class SoftbodyCluster
    {
        public static readonly int MAX_CLUSTER_NAME_LENGTH = 0x20;
        public static readonly int QUAD_DYADIC_SUM_LENGTH = 81;
        public static readonly int BASE_ALLOCATION_SIZE = MAX_CLUSTER_NAME_LENGTH + (QUAD_DYADIC_SUM_LENGTH * 0x4) + 0x60;
        private string name;
        private Vector4 restCenterOfMass;
        private Matrix4x4 restDyadicSum;
        private float[] restQuadraticDyadicSum = new float[QUAD_DYADIC_SUM_LENGTH];
        public SoftbodyCluster()
        {

            // float9x9
            for (int i = 0; i < QUAD_DYADIC_SUM_LENGTH; ++i)
                if (i % 10 == 0)
                    this.restQuadraticDyadicSum[i] = 1F;
        }

        public virtual string GetName()
        {
            return this.name;
        }

        public virtual Vector4 GetRestCenterOfMass()
        {
            return this.restCenterOfMass;
        }

        public virtual Matrix4x4 GetRestDyadicSum()
        {
            return this.restDyadicSum;
        }

        public virtual float[] GetRestQuadraticDyadicSum()
        {
            return this.restQuadraticDyadicSum;
        }

        public virtual void SetName(string name)
        {
            if (name != null && name.Length() > MAX_CLUSTER_NAME_LENGTH)
                throw new ArgumentException("Cluster name cannot be longer than 32 " + "characters.");
            this.name = name;
        }

        public virtual void SetRestCenterOfMass(Vector4 COM)
        {
            this.restCenterOfMass = COM;
        }

        public virtual void SetRestDyadicSum(Matrix4x4 sum)
        {
            this.restDyadicSum = sum;
        }

        public virtual void SetRestQuadraticDyadicSum(float[] sum)
        {
            if (sum == null || sum.length != QUAD_DYADIC_SUM_LENGTH)
                throw new ArgumentException("Rest quadratic dyadic sum array length must" + " be " + "81!");
            this.restQuadraticDyadicSum = sum;
        }
    }
}