using System;
using System.Collections.Generic;
using Game5.Data;
using Microsoft.Xna.Framework;

namespace Game5.Geometry.Extensions
{
    public static class VectorExtensions
    {
        public static Edge GetEdge(this Vector2[] vectors, int index)
        {
            return new Edge(vectors[index % vectors.Length], vectors[(index + 1) % vectors.Length]);
        }

        public static IEnumerable<Edge> GetEdges(this Vector2[] vertices)
        {
            var edges = new List<Edge>();
            for (var i = 0; i < vertices.Length; i++) edges.Add(vertices.GetEdge(i));
            return edges;
        }

        public static RectangleF GetAABB(this Vector2[] vertices)
        {
            if (vertices.Length == 0) return new RectangleF(0, 0, 0, 0);

            float vmax, vmin = vmax = vertices[0].Y;
            float hmax, hmin = hmax = vertices[0].X;
            foreach (var vertex in vertices)
            {
                if (vertex.Y > vmax) vmax = vertex.Y;
                if (vertex.X > hmax) hmax = vertex.X;
                if (vertex.Y < vmin) vmin = vertex.Y;
                if (vertex.X < hmin) hmin = vertex.X;
            }

            return new RectangleF(hmin, vmin, hmax - hmin, vmax - vmin);
        }

        public static Vector2[] Translate(this Vector2[] vertices, Vector2 vector)
        {
            for (var i = 0; i < vertices.Length; i++) vertices[i] += vector;
            return vertices;
        }

        public static Vector2[] Rotate(this Vector2[] vertices, float rotation)
        {
            var origin = vertices.GetOrigin();
            for (var i = 0; i < vertices.Length; i++)
            {
                var v = vertices[i];
                var temp = v;
                temp.X = (float) (origin.X + (v.X - origin.X) * Math.Cos(rotation) -
                                  (v.Y - origin.Y) * Math.Sin(rotation));
                temp.Y = (float) (origin.Y + (v.X - origin.X) * Math.Sin(rotation) +
                                  (v.Y - origin.Y) * Math.Cos(rotation));
                v = temp;
                vertices[i] = v;
            }

            return vertices;
        }

        public static Vector2 GetOrigin(this Vector2[] vertices)
        {
            var origin = new Vector2();
            for (var i = 0; i < vertices.Length; i++) origin += vertices[i];
            return origin / vertices.Length;
        }

        public static Vector2 Project(this Vector2 vector, Vector2 axis)
        {
            var m = vector.Magnitude();
            return vector * (Vector2.Dot(vector, axis) / (m * m));
        }

        public static Projection Project(this Vector2[] vertices, Vector2 axis)
        {
            if (vertices.Length == 0) return new Projection();
            var min = Vector2.Dot(axis, vertices[0]);
            var max = min;
            for (var i = 1; i < vertices.Length; i++)
            {
                var p = Vector2.Dot(axis, vertices[i]);
                if (p < min)
                    min = p;
                else if (p > max) max = p;
            }

            var proj = new Projection(min, max);
            return proj;
        }

        public static MinimumTranslationVector Intersects(this Vector2[] vertices, Vector2[] other)
        {
            var overlap = float.MaxValue;
            var smallestAxis = new Vector2();
            foreach (var edge in vertices.GetEdgeNormals())
            {
                edge.Normalize();
                var proj1 = vertices.Project(edge);
                var proj2 = other.Project(edge);

                if (!proj1.Intersects(proj2)) return new MinimumTranslationVector(false, new Vector2(0), 0);

                var o = proj1.GetOverlap(proj2);
                if (o < overlap)
                {
                    overlap = o;
                    smallestAxis = edge;
                }
            }

            foreach (var edge in other.GetEdgeNormals())
            {
                edge.Normalize();
                var proj1 = vertices.Project(edge);
                var proj2 = other.Project(edge);

                if (!proj1.Intersects(proj2))
                {
                    return new MinimumTranslationVector(false, new Vector2(0), 0);
                }

                var o = proj1.GetOverlap(proj2);
                if (o < overlap)
                {
                    overlap = o;
                    smallestAxis = edge;
                }
            }

            return new MinimumTranslationVector(true, smallestAxis, overlap);
        }

        public static IEnumerable<Vector2> GetEdgeNormals(this Vector2[] vertices)
        {
            var list = new List<Vector2>();
            for (var i = 0; i < vertices.Length; i++) list.Add(vertices.GetEdge(i).Normal());
            return list;
        }

        public static float Magnitude(this Vector2 vector)
        {
            return (float) Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2 NegateX(this Vector2 vector)
        {
            return new Vector2(-vector.X, vector.Y);
        }

        public static Vector2 NegateY(this Vector2 vector)
        {
            return new Vector2(vector.X, -vector.Y);
        }

        public static Vector2 Negate(this Vector2 vector)
        {
            return new Vector2(-vector.X, -vector.Y);
        }
    }
}