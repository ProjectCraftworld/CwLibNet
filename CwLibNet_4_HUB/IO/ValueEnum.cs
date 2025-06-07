using static net.torutheredfox.craftworld.serialization.Serializer;
namespace CwLibNet.IO;

public interface IValueEnum<out TE>
{
    TE GetValue();
}