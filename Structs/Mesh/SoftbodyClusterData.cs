using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serialization;
using CwLibNet.IO.Streams;

namespace CwLibNet.Structs.Mesh
{
    /// <summary>
    /// Softbody clusters container.
    /// </summary>
    public class SoftbodyClusterData : ISerializable, Iterable<SoftbodyCluster>
    {
        public static readonly int BASE_ALLOCATION_SIZE = 0x60;
        private List<SoftbodyCluster> clusters = new List();
        public override void Serialize(Serializer serializer)
        {

            // The actual way this is serialized makes it annoying to
            // edit anything in a standard object oriented way, so
            // we'll just be encapsulating the data in separate cluster classes.
            if (serializer.IsWriting())
            {

                // We need to prepare all the arrays for serialization.
                int clusterCount = clusters.Count;
                Vector4[] restCOM = new Vector4[clusterCount];
                Matrix4x4[] restDyadicSum = new Matrix4x4[clusterCount];
                float[] restQuadraticDyadicSum = new float[clusterCount * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH];
                String[] names = new string[clusterCount];
                for (int i = 0; i < clusterCount; ++i)
                {
                    SoftbodyCluster cluster = clusters[i];
                    restCOM[i] = cluster.GetRestCenterOfMass();
                    restDyadicSum[i] = cluster.GetRestDyadicSum();

                    // All the rest dyadic sum values are stored
                    // in the same array.
                    System.Arraycopy(cluster.GetRestQuadraticDyadicSum(), 0, restQuadraticDyadicSum, i * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH, SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH);
                    names[i] = cluster.GetName();
                }


                // Begin to actually serialize the data to the array.
                MemoryOutputStream stream = serializer.GetOutput();
                stream.i32(clusterCount);
                stream.i32(restCOM.length);
                foreach (Vector4 COM in restCOM)
                    stream.v4(COM);
                stream.i32(restDyadicSum.length);
                foreach (Matrix4x4 dyadicSum in restDyadicSum)
                    stream.m44(dyadicSum);
                stream.Floatarray(restQuadraticDyadicSum);
                stream.i32(names.length);
                foreach (string name in names)
                    stream.Str(name, SoftbodyCluster.MAX_CLUSTER_NAME_LENGTH);
                return;
            }

            MemoryInputStream stream = serializer.GetInput();
            int clusterCount = stream.i32();
            clusters = new List(clusterCount);

            // Technically, when reading from the softbody cluster data
            // it's possible for the arrays to not match the cluster counts,
            // but if you're doing that, go fuck yourself.
            Vector4[] restCOM = new Vector4[clusterCount];
            Matrix4x4[] restDyadicSum = new Matrix4x4[clusterCount];
            float[] restQuadraticDyadicSum = new float[clusterCount * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH];
            String[] names = new string[clusterCount];
            stream.i32(); // restCOM array length
            for (int i = 0; i < clusterCount; ++i)
                restCOM[i] = stream.v4();
            stream.i32(); // restDyadicSum array length
            for (int i = 0; i < clusterCount; ++i)
                restDyadicSum[i] = stream.m44();
            stream.i32(); // restQudraticDyadicSum array length
            for (int i = 0; i < restQuadraticDyadicSum.length; ++i)
                restQuadraticDyadicSum[i] = stream.f32();
            stream.i32(); // name array length
            for (int i = 0; i < clusterCount; ++i)
                names[i] = stream.Str(SoftbodyCluster.MAX_CLUSTER_NAME_LENGTH);
            for (int i = 0; i < clusterCount; ++i)
            {
                SoftbodyCluster cluster = new SoftbodyCluster();
                cluster.SetRestCenterOfMass(restCOM[i]);
                cluster.SetRestDyadicSum(restDyadicSum[i]);
                int offset = i * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH;
                cluster.SetRestQuadraticDyadicSum(Arrays.CopyOfRange(restQuadraticDyadicSum, offset, offset + SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH));
                cluster.SetName(names[i]);
                clusters.Add(cluster);
            }
        }

        // The actual way this is serialized makes it annoying to
        // edit anything in a standard object oriented way, so
        // we'll just be encapsulating the data in separate cluster classes.
        // We need to prepare all the arrays for serialization.
        // All the rest dyadic sum values are stored
        // in the same array.
        // Begin to actually serialize the data to the array.
        // Technically, when reading from the softbody cluster data
        // it's possible for the arrays to not match the cluster counts,
        // but if you're doing that, go fuck yourself.
        // restCOM array length
        // restDyadicSum array length
        // restQudraticDyadicSum array length
        // name array length
        public virtual int GetAllocatedSize()
        {
            return BASE_ALLOCATION_SIZE + (this.clusters.Count * SoftbodyCluster.BASE_ALLOCATION_SIZE);
        }

        // The actual way this is serialized makes it annoying to
        // edit anything in a standard object oriented way, so
        // we'll just be encapsulating the data in separate cluster classes.
        // We need to prepare all the arrays for serialization.
        // All the rest dyadic sum values are stored
        // in the same array.
        // Begin to actually serialize the data to the array.
        // Technically, when reading from the softbody cluster data
        // it's possible for the arrays to not match the cluster counts,
        // but if you're doing that, go fuck yourself.
        // restCOM array length
        // restDyadicSum array length
        // restQudraticDyadicSum array length
        // name array length
        public virtual IEnumerator<SoftbodyCluster> Iterator()
        {
            return this.clusters.Iterator();
        }

        // The actual way this is serialized makes it annoying to
        // edit anything in a standard object oriented way, so
        // we'll just be encapsulating the data in separate cluster classes.
        // We need to prepare all the arrays for serialization.
        // All the rest dyadic sum values are stored
        // in the same array.
        // Begin to actually serialize the data to the array.
        // Technically, when reading from the softbody cluster data
        // it's possible for the arrays to not match the cluster counts,
        // but if you're doing that, go fuck yourself.
        // restCOM array length
        // restDyadicSum array length
        // restQudraticDyadicSum array length
        // name array length
        public virtual List<SoftbodyCluster> GetClusters()
        {
            return this.clusters;
        }
    }
}