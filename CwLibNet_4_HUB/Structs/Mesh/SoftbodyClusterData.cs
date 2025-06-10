using System.Collections;
using System.Numerics;
using CwLibNet4Hub.IO;
using CwLibNet4Hub.IO.Serializer;
using static CwLibNet4Hub.IO.Serializer.Serializer;
namespace CwLibNet4Hub.Structs.Mesh;

public class SoftbodyClusterData: ISerializable, IEnumerable<SoftbodyCluster>
{
    public static readonly int BASE_ALLOCATION_SIZE = 0x60;

    public List<SoftbodyCluster> Clusters = [];

    
    public void Serialize(CwLibNet4Hub.IO.Serializer.Serializer serializer)
    {
        // The actual way this is serialized makes it annoying to
        // edit anything in a standard object oriented way, so
        // we'll just be encapsulating the data in separate cluster classes.

        if (Serializer.IsWriting())
        {
            // We need to prepare all the arrays for serialization.
            var _clusterCount = Clusters.Count;
            var _restCOM = new Vector4[_clusterCount];
            var _restDyadicSum = new Matrix4x4[_clusterCount];
            var _restQuadraticDyadicSum =
                new float[_clusterCount * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH];
            var _names = new string[_clusterCount];
            for (var i = 0; i < _clusterCount; ++i)
            {
                var cluster = Clusters[i];
                _restCOM[i] = cluster.RestCenterOfMass;
                _restDyadicSum[i] = cluster.RestDyadicSum;

                // All the rest dyadic sum values are stored
                // in the same array.
                Array.Copy(
                    cluster.RestQuadraticDyadicSum,
                    0,
                    _restQuadraticDyadicSum,
                    i * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH,
                    SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH
                );

                _names[i] = cluster.Name;
            }

            // Begin to actually serialize the data to the array.
            var _stream = Serializer.GetCurrentSerializer().GetOutput();

            _stream.I32(_clusterCount);

            _stream.I32(_restCOM.Length);
            foreach (var COM in _restCOM)
                _stream.V4(COM);

            _stream.I32(_restDyadicSum.Length);
            foreach (var dyadicSum in _restDyadicSum)
                _stream.M44(dyadicSum);

            _stream.Floatarray(_restQuadraticDyadicSum);

            _stream.I32(_names.Length);
            foreach (var name in _names)
                _stream.Str(name, SoftbodyCluster.MAX_CLUSTER_NAME_LENGTH);

            return;
        }

        var stream = Serializer.GetCurrentSerializer().GetInput();

        var clusterCount = stream.I32();
        Clusters = new List<SoftbodyCluster>(clusterCount);

        // Technically, when reading from the softbody cluster data
        // it's possible for the arrays to not match the cluster counts,
        // but if you're doing that, go fuck yourself.

        var restCOM = new Vector4[clusterCount];
        var restDyadicSum = new Matrix4x4[clusterCount];
        var restQuadraticDyadicSum =
            new float[clusterCount * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH];
        var names = new string[clusterCount];

        stream.I32(); // restCOM array length
        for (var i = 0; i < clusterCount; ++i)
            restCOM[i] = stream.V4();
        stream.I32(); // restDyadicSum array length
        for (var i = 0; i < clusterCount; ++i)
            restDyadicSum[i] = stream.M44();
        stream.I32(); // restQudraticDyadicSum array length
        for (var i = 0; i < restQuadraticDyadicSum.Length; ++i)
            restQuadraticDyadicSum[i] = stream.F32();
        stream.I32(); // name array length
        for (var i = 0; i < clusterCount; ++i)
            names[i] = stream.Str(SoftbodyCluster.MAX_CLUSTER_NAME_LENGTH);

        for (var i = 0; i < clusterCount; ++i)
        {
            var cluster = new SoftbodyCluster
            {
                RestCenterOfMass = restCOM[i],
                RestDyadicSum = restDyadicSum[i]
            };

            var offset = i * SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH;
            cluster.RestQuadraticDyadicSum = restQuadraticDyadicSum.Skip(offset).Take(SoftbodyCluster.QUAD_DYADIC_SUM_LENGTH).ToArray();
            cluster.SetName(names[i]);

            Clusters.Add(cluster);
        }
    }

    
    public int GetAllocatedSize()
    {
        return BASE_ALLOCATION_SIZE + Clusters.Count * SoftbodyCluster.BASE_ALLOCATION_SIZE;
    }


    IEnumerator<SoftbodyCluster> IEnumerable<SoftbodyCluster>.GetEnumerator()
    {
        return Clusters.GetEnumerator();

    }


    public IEnumerator GetEnumerator()
    {
        return Clusters.GetEnumerator();
    }
}