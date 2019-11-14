using Microsoft.Xna.Framework;

namespace Game5.Geometry
{
    public struct MinimumTranslationVector
    {
        public Vector2 Axis { get; set; }
        public float Overlap { get; set; }
        public bool Collision { get; set; }

        public MinimumTranslationVector(bool collision, Vector2 axis, float overlap)
        {
            Collision = collision;
            Axis = axis;
            Overlap = overlap;
        }
    }
}