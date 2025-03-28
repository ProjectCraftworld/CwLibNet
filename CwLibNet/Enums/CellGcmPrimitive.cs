using CwLibNet.IO;

namespace CwLibNet.Enums
{
    public enum CellGcmPrimitive
    {
        // POINTS(1)
        POINTS = 1,
        // LINES(2)
        LINES = 2,
        // LINE_LOOP(3)
        LINE_LOOP,
        // LINE_STRIP(4)
        LINE_STRIP,
        // TRIANGLES(5)
        TRIANGLES,
        // TRIANGLE_STRIP(6)
        TRIANGLE_STRIP,
        // TRIANGLE_FAN(7)
        TRIANGLE_FAN,
        // QUADS(8)
        QUADS,
        // QUAD_STRIP(9)
        QUAD_STRIP,
        // POLYGON(10)
        POLYGON 
    }

    public sealed class CellPrimitiveBodyMembers
    {

        private readonly CellGcmPrimitive value;
        CellPrimitiveBodyMembers(int value)
        {
            this.value = (CellGcmPrimitive)value;
        }

        public int getValue()
        {
            return (int)this.value;
        }




        public static CellPrimitiveBodyMembers fromValue(int value)
        {
            if (Enum.IsDefined(typeof(CellGcmPrimitive), value))
            {
                return new CellPrimitiveBodyMembers(value);
            }
            return default(CellPrimitiveBodyMembers);
        }
    }
}