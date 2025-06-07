using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
namespace CwLibNet.IO;

public interface IValueEnum<out TE>
{
    TE GetValue();
}