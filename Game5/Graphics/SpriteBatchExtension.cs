using System;
using Game5.Data;
using Game5.Geometry;
using Game5.Geometry.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.Graphics
{
    public static class SpriteBatchExtension
    {
        /// <summary>
        ///     A cached texture. Texture should not be recreated every frame
        /// </summary>
        private static Texture2D cachedTexture;

        /// <summary>
        ///     Draws a rectangle
        /// </summary>
        /// <param name="spriteBatch">Object extended</param>
        /// <param name="rectangle">The rectangle to draw</param>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="overlay">Overlay texture, if null no overlay is drawn</param>
        public static void DrawRectangle(
            this SpriteBatch spriteBatch,
            Rectangle rectangle,
            Color color,
            Texture2D overlay = null)
        {
            if (cachedTexture == null)
            {
                cachedTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                cachedTexture.SetData(new[] {Color.White});
            }

            spriteBatch.Draw(overlay ?? cachedTexture, rectangle, color);
        }

        /// <summary>
        ///     Draws a rectangle with a border
        /// </summary>
        /// <param name="spriteBatch">Object extended</param>
        /// <param name="rectangle">The rectangle to draw</param>
        /// <param name="borderSize">Size of the border</param>
        /// <param name="borderColor">Color of the border</param>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="overlay">Overlay texture, if null no overlay is drawn</param>
        public static void DrawBorderedRectangle(
            this SpriteBatch spriteBatch,
            Rectangle rectangle,
            int borderSize,
            Color borderColor,
            Color color,
            Texture2D overlay = null)
        {
            if (cachedTexture == null)
            {
                cachedTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                cachedTexture.SetData(new[] {Color.White});
            }

            var border = new Rectangle(rectangle.X - borderSize, rectangle.Y - borderSize,
                rectangle.Width + borderSize * 2, rectangle.Height + borderSize * 2);
            spriteBatch.Draw(cachedTexture, border, borderColor);
            spriteBatch.Draw(overlay ?? cachedTexture, rectangle, color);
        }

        /// <summary>
        ///     Draws a line
        /// </summary>
        /// <param name="spriteBatch">Object extended</param>
        /// <param name="start">Start position of line</param>
        /// <param name="end">End position of line</param>
        /// <param name="lineWidth">Width of line</param>
        /// <param name="color">Color of line</param>
        public static void DrawLine(
            this SpriteBatch spriteBatch,
            Vector2 start,
            Vector2 end,
            int lineWidth,
            Color color)
        {
            if (cachedTexture == null)
            {
                cachedTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                cachedTexture.SetData(new[] {Color.White});
            }

            var length = Vector2.Distance(start, end);
            var line = new Rectangle((int) start.X, (int) start.Y, lineWidth, (int) length);
            var rotation = (float) (Math.Atan2(end.Y - start.Y, end.X - start.X) - Math.PI / 2);

            spriteBatch.Draw(cachedTexture, line, null, color, rotation, Vector2.Zero, SpriteEffects.None, 0);
        }

        public static void DrawBorder(this SpriteBatch spriteBatch, Rectangle rectangle, int size, Color color)
        {
            spriteBatch.DrawLine(
                new Vector2(rectangle.Left, rectangle.Top),
                new Vector2(rectangle.Right, rectangle.Top),
                size, color);
            spriteBatch.DrawLine(
                new Vector2(rectangle.Right, rectangle.Top),
                new Vector2(rectangle.Right, rectangle.Bottom),
                size, color);
            spriteBatch.DrawLine(
                new Vector2(rectangle.Left, rectangle.Bottom),
                new Vector2(rectangle.Right, rectangle.Bottom),
                size, color);
            spriteBatch.DrawLine(
                new Vector2(rectangle.Left, rectangle.Top),
                new Vector2(rectangle.Left, rectangle.Bottom),
                size, color);
        }

        public static void DrawEdge(this SpriteBatch spriteBatch, Edge edge, int width, Color color)
        {
            spriteBatch.DrawLine(edge.Start, edge.End, width, color);
        }

        public static void DrawPolygon(this SpriteBatch spriteBatch, Vector2[] polygon, int width, Color color)
        {
            foreach (var edge in polygon.GetEdges()) spriteBatch.DrawLine(edge.Start, edge.End, width, color);
        }

        public static void DrawPolygon(this SpriteBatch spriteBatch, Polygon polygon, int width, Color color)
        {
            foreach (var edge in polygon.GetEdges()) spriteBatch.DrawLine(edge.Start, edge.End, width, color);
        }

        public static void DrawFormattedString(this SpriteBatch spriteBatch, SpriteFont font, FormattedString text,
            Vector2 pos, float scale = 1.0f, float rotation = 0f, Vector2 origin = default(Vector2))
        {
            text.ForEach(x =>
            {
                spriteBatch.DrawString(font, x.text, pos, x.color, rotation, origin, scale, SpriteEffects.None, 1);
                pos.X += font.MeasureString(x.text).X * scale;
            });
        }

        public static void DrawFormattedString(this SpriteBatch spriteBatch, SpriteFont font, FormattedString text,
            Vector2 pos, int fontSize, float rotation = 0f, Vector2 origin = default(Vector2))
        {
            var scale = fontSize / (float) font.GetGlyphs()['S'].BoundsInTexture.Height;
            text.ForEach(x =>
            {
                spriteBatch.DrawString(font, x.text, pos, x.color, rotation, origin, scale, SpriteEffects.None, 1);
                pos.X += font.MeasureString(x.text).X * scale;
            });
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 pos,
            int fontSize, Color color = default(Color), float rotation = 0f, Vector2 origin = default(Vector2))
        {
            var scale = fontSize / (float) font.GetGlyphs()['S'].BoundsInTexture.Height;
            spriteBatch.DrawString(font, text, pos, color, rotation, origin, scale, SpriteEffects.None, 1);
        }
    }
}