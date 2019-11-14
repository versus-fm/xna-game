using Microsoft.Xna.Framework;

namespace Game5.Geometry
{
    public class Edge
    {
        public Edge(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public Vector2 Start { get; set; }

        public Vector2 End { get; set; }

        public Vector2 Normal()
        {
            var v = End - Start;
            return new Vector2(v.Y, -v.X);
        }
    }
}