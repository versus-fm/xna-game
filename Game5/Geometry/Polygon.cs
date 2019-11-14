using System;
using System.Collections.Generic;
using Game5.Data;
using Game5.Geometry.Extensions;
using Microsoft.Xna.Framework;

namespace Game5.Geometry
{
    public class Polygon
    {
        private float rotation;
        private Vector2 translationVector;
        private Vector2[] vertices;
        private readonly Vector2[] original;

        /// <summary>
        ///     Constructs a polygon from radius and scaled vertex points from -1.0 to 1.0
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="scaledVertices"></param>
        public Polygon(float radius, Vector2[] scaledVertices)
        {
            for (var i = 0; i < scaledVertices.Length; i++) scaledVertices[i] *= radius;
            vertices = scaledVertices;
            original = new Vector2[vertices.Length];
            vertices.CopyTo(original, 0);
        }

        public MinimumTranslationVector Intersects(Polygon other)
        {
            return vertices.Intersects(other);
        }

        public void Rotate(float rotation)
        {
            this.rotation += rotation;
            vertices = vertices.Rotate(rotation);
        }

        public void Translate(Vector2 vector)
        {
            translationVector += vector;
            vertices = vertices.Translate(vector);
        }

        public void SetTranslatation(Vector2 vector)
        {
            Translate(-translationVector);
            Translate(vector);
        }

        public void SetRotation(float rotation)
        {
            Rotate(-rotation);
            Rotate(rotation);
        }

        public IEnumerable<Edge> GetEdges()
        {
            return vertices.GetEdges();
        }

        public IEnumerable<Vector2> GetEdgeNormals()
        {
            return vertices.GetEdgeNormals();
        }

        public Edge GetEdge(int index)
        {
            return vertices.GetEdge(index);
        }

        public RectangleF GetAABB()
        {
            return vertices.GetAABB();
        }

        public static implicit operator Vector2[](Polygon polygon)
        {
            return polygon.vertices;
        }

        public static Polygon CreateCircle(float radius, int segments)
        {
            var vertices = new Vector2[segments];
            var segment = (float) (Math.PI * 2) / segments;
            var index = 0;
            for (float c = 0; c < Math.PI * 2 && index < segments; c += segment)
            {
                vertices[index] = new Vector2((float) Math.Cos(c), (float) Math.Sin(c));
                index++;
            }

            return new Polygon(radius, vertices);
        }

        public static Polygon CreateIcicle(float radius)
        {
            var vertices = new[]
            {
                new Vector2(-1, -1),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            return new Polygon(radius, vertices);
        }

        /// <summary>
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="widthFactor">The factor to multiply the vertices' x values by. 0.0 - 1.0</param>
        /// <param name="heightFactor">The factor to multiply the vertices' y values by. 0.0 - 1.0</param>
        /// <returns></returns>
        public static Polygon CreateRectangle(float radius, float widthFactor = 1, float heightFactor = 1)
        {
            widthFactor = MathHelper.Clamp(widthFactor, 0, 1);
            heightFactor = MathHelper.Clamp(heightFactor, 0, 1);
            var vertices = new[]
            {
                new Vector2(-1 * widthFactor, -1 * heightFactor),
                new Vector2(1 * widthFactor, -1 * heightFactor),
                new Vector2(1 * widthFactor, 1 * heightFactor),
                new Vector2(-1 * widthFactor, 1 * heightFactor)
            };
            return new Polygon(radius, vertices);
        }
    }
}