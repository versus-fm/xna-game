namespace Game5.Geometry
{
    public struct Projection
    {
        public float Min { get; }
        public float Max { get; }

        public Projection(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public bool Intersects(Projection other)
        {
            return Min < other.Max && other.Min < Max;
        }

        public float GetOverlap(Projection other)
        {
            return Max - other.Min;
        }
    }
}