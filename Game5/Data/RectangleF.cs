using System;
using Microsoft.Xna.Framework;

namespace Game5.Data
{
    public class RectangleF
    {
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF(Vector2 pos)
        {
            X = pos.X;
            Y = pos.Y;
            Width = 0;
            Height = 0;
        }

        public RectangleF(float x, float y)
        {
            X = x;
            Y = y;
            Width = 0;
            Height = 0;
        }

        public RectangleF(Vector2 pos, float width, float height)
        {
            X = pos.X;
            Y = pos.Y;
            Width = width;
            Height = height;
        }

        public RectangleF()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        public float Top => Y;

        public float Bottom => Y + Height;

        public float Right => X + Width;

        public float Left => X;

        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public bool Intersects(RectangleF other)
        {
            return !(Left >= other.Right || Right <= other.Left ||
                     Top >= other.Bottom || Bottom <= other.Top);
        }

        public static implicit operator Rectangle(RectangleF rect)
        {
            return new Rectangle((int) Math.Round(rect.X), (int) Math.Round(rect.Y), (int) Math.Round(rect.Width),
                (int) Math.Round(rect.Height));
        }

        public override string ToString()
        {
            return "{X: " + X + ", Y: " + Y + ", W: " + Width + ", H: " + Height + "}";
        }

        public static RectangleF Intersection(RectangleF value1, RectangleF value2)
        {
            RectangleF result;
            if (value1.Intersects(value2))
            {
                var right_side = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                var left_side = Math.Max(value1.X, value2.X);
                var top_side = Math.Max(value1.Y, value2.Y);
                var bottom_side = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                result = new RectangleF(left_side, top_side, right_side - left_side, bottom_side - top_side);
            }
            else
            {
                result = new RectangleF(0, 0, 0, 0);
            }

            return result;
        }
    }
}