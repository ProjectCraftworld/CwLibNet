namespace CwLibNet.Structs.Animation
{
    public class AnimatedMorph
    {
        public float value;
        public float[] values;
        public bool isAnimated = false;
        public AnimatedMorph(float value, int count)
        {
            this.value = value;
            this.values = new float[count];
        }

        public virtual float GetValueAtFrame(int frame)
        {
            if (values == null || !isAnimated)
                return value;
            if (frame < values.length)
                return values[frame];
            return value;
        }
    }
}