using System.Collections;
using System.Numerics;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;

namespace CwLibNet.Structs.Mesh;

public class SoftbodyClusterData: ISerializable, IEnumerable<SoftbodyCluster>
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x60;

    public List<SoftbodyCluster> Clusters = [];

    
    public void Serialize(Serializer serializer)
    {
        // The actual way this is serialized makes it annoying to
        // edit anything in a standard object oriented way, so
        // we'll just be encapsulating the data in separate cluster classes.

        if (serializer.IsWriting())
        {
            // We need to prepare all the arrays for serialization.
            int _clusterCount = Clusters.Count;
            Vector4[] _restCOM = new Vector4[_clusterCount];
            Matrix4x4[] _restDyadicSum = new Matrix4x4[_clusterCount];
            float[] _restQuadraticDyadicSum =
                new float[_clusterCount * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH];
            String[] _names = new String[_clusterCount];
            for (int i = 0; i < _clusterCount; ++i)
            {
                SoftbodyCluster cluster = Clusters[i];
                _restCOM[i] = cluster.restCenterOfMass;
                _restDyadicSum[i] = cluster.restDyadicSum;

                // All the rest dyadic sum values are stored
                // in the same array.
                Array.Copy(
                    cluster.restQuadraticDyadicSum,
                    0,
                    _restQuadraticDyadicSum,
                    i * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH,
                    SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH
                );

                _names[i] = cluster.name;
            }

            // Begin to actually serialize the data to the array.
            MemoryOutputStream _stream = serializer.GetOutput();

            _stream.I32(_clusterCount);

            _stream.I32(_restCOM.Length);
            foreach (Vector4 COM in _restCOM)
                _stream.V4(COM);

            _stream.I32(_restDyadicSum.Length);
            foreach (Matrix4x4 dyadicSum in _restDyadicSum)
                _stream.M44(dyadicSum);

            _stream.Floatarray(_restQuadraticDyadicSum);

            _stream.I32(_names.Length);
            foreach (String name in _names)
                _stream.Str(name, SoftbodyCluster.MAX_CLUSTER_NAME_LENGTH);

            return;
        }

        MemoryInputStream stream = serializer.GetInput();

        int clusterCount = stream.I32();
        Clusters = new List<SoftbodyCluster>(clusterCount);

        // Technically, when reading from the softbody cluster data
        // it's possible for the arrays to not match the cluster counts,
        // but if you're doing that, go fuck yourself.

        Vector4[] restCOM = new Vector4[clusterCount];
        Matrix4x4[] restDyadicSum = new Matrix4x4[clusterCount];
        float[] restQuadraticDyadicSum =
            new float[clusterCount * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH];
        String[] names = new String[clusterCount];

        stream.I32(); // restCOM array length
        for (int i = 0; i < clusterCount; ++i)
            restCOM[i] = stream.V4();
        stream.I32(); // restDyadicSum array length
        for (int i = 0; i < clusterCount; ++i)
            restDyadicSum[i] = stream.M44();
        stream.I32(); // restQudraticDyadicSum array length
        for (int i = 0; i < restQuadraticDyadicSum.Length; ++i)
            restQuadraticDyadicSum[i] = stream.F32();
        stream.I32(); // name array length
        for (int i = 0; i < clusterCount; ++i)
            names[i] = stream.Str(SoftbodyCluster.MAX_CLUSTER_NAME_LENGTH);

        for (int i = 0; i < clusterCount; ++i)
        {
            SoftbodyCluster cluster = new SoftbodyCluster();

            cluster.restCenterOfMass = (restCOM[i]);
            cluster.restDyadicSum = (restDyadicSum[i]);
            int offset = i * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH;
            cluster.restQuadraticDyadicSum = restQuadraticDyadicSum.Skip(offset).Take(SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH).ToArray();
            cluster.setName(names[i]);

            Clusters.Add(cluster);
        }
    }

    
    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE + (this.Clusters.Count * SoftbodyCluster.BASE_ALLOCATION_SIZE);
    }


    IEnumerator<SoftbodyCluster> IEnumerable<SoftbodyCluster>.GetEnumerator()
    {
        return this.Clusters.GetEnumerator();

    }


    public IEnumerator GetEnumerator()
    {
        return this.Clusters.GetEnumerator();
    }
}