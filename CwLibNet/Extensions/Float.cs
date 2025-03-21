namespace CwLibNet.Extensions;

public static class Float
{
    public static float ToRadians (this float angleIn10thofaDegree) {
        // Angle in 10th of a degree
        return (float)((angleIn10thofaDegree * Math.PI)/1800); 
    }
}