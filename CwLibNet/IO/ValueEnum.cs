namespace CwLibNet.IO;

public interface IValueEnum<out TE>
{
    TE GetValue();
}