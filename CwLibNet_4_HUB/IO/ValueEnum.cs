using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.IO;

public interface IValueEnum<out TE>
{
    TE GetValue();
}