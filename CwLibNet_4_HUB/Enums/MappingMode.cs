namespace CwLibNet.Enums;

public enum MappingMode
{
    // HIDDEN(0)
    HIDDEN,
    // /**
    //  * Standard XYZ UV Mapping.
    //  */
    // DAVE(1)
    DAVE,
    // /**
    //  * Wraps vertically; separate faces.
    //  */
    // CYLINDER_01(2)
    CYLINDER_01,
    // /**
    //  * Wraps vertical; all one face.
    //  */
    // CYLINDER(3)
    CYLINDER,
    // /**
    //  * Wraps vertically; seperate faces, stretches on Y axis.
    //  */
    // PLANARXZ_01(4)
    PLANARXZ_01,
    // /**
    //  * Wraps vertically; all one face, stretches on Y axis.
    //  */
    // PLANARXZ(5)
    PLANARXZ 
}

public sealed class MappingBody
{
    private readonly MappingMode value;

    public MappingBody(int value)
    {
        this.value = (MappingMode)value;
    }

    public MappingMode getValue()
    {
        return value;
    }

    public static MappingBody? fromValue(int value)
    {
        return Enum.IsDefined(typeof(MappingMode), value) ? new MappingBody(value) : null;
    }
}