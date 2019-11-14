using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Game5.Data.Attributes.Lua;
using Game5.Env;
using Game5.Resource;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = System.Drawing.Color;
using InterfaceStyle = Game5.UI.Styling.InterfaceStyle;
using Point = System.Drawing.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Song = Microsoft.Xna.Framework.Media.Song;
using SpriteFont = Microsoft.Xna.Framework.Graphics.SpriteFont;
using Spritesheet = Game5.Graphics.Spritesheet;

namespace Game5.Data.LuaAPI.API.Statics
{
    [LuaStaticClass("resources")]
    public class LuaResources
    {
        //Provides access to the resource service to register image.
        public static void LoadImage(string path, string name)
        {
            using (var fs = new FileStream(path, FileMode.Open))
            {
                ServiceLocator.Get<IResourceService>()
                    .Register(Texture2D.FromStream(ServiceLocator.Get<GraphicsDeviceManager>().GraphicsDevice, fs),
                        name);
            }
        }

        /// <summary>
        ///     Will perform some reflection to load the specified ttf file into the resource service. This method will first
        ///     rasterize the font using System.Drawing, and will then construct a SpriteFont, as a result this method is very
        ///     slow.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static void LoadFont(string path, int size, string style, char start, char end, string name)
        {
            //Get SpriteFont Constructor
            var constructor = typeof(SpriteFont).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public,
                null, new[]
                {
                    typeof(Texture2D), //texture
                    typeof(List<Rectangle>), //glyphBounds
                    typeof(List<Rectangle>), //cropping
                    typeof(List<char>), //characters
                    typeof(int), //lineSpacing
                    typeof(float), //spacing
                    typeof(List<Vector3>), //kerning
                    typeof(char?) //defaultChar
                }, new ParameterModifier[0]);

            //constructor.Invoke(new object[] { null, null, null, null, null, null, null, null });

            var pfcoll = new PrivateFontCollection();
            pfcoll.AddFontFile(path);
            var ff = pfcoll.Families[0];
            var targetSize = (int) Math.Sqrt(end - start);
            var width = (targetSize + 10) * size;
            var height = (targetSize + 10) * size;

            var target = new Bitmap(width, height);
            //for (int i = 0; i < width; i++)
            //{
            //    for (int o = 0; o < height; o++)
            //    {
            //        //Debug.WriteLine(target.GetPixel(i, o));
            //    }
            //}
            target.MakeTransparent();


            var bounds = new List<Rectangle>();
            var cropping = new List<Rectangle>();
            var chars = new List<char>();
            var kerning = new List<Vector3>();
            var rect = new System.Drawing.RectangleF(0, 0, 0, 0);
            var lineSpacing = 0;
            var spacing = 0f;
            var transparent = Color.FromArgb(0, 0, 0, 0);
            using (var graphics = System.Drawing.Graphics.FromImage(target))
            {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                graphics.SmoothingMode = SmoothingMode.None;
                using (var font = new Font(ff, size, GraphicsUnit.Pixel))
                {
                    lineSpacing = font.FontFamily.GetLineSpacing(FontStyle.Regular);
                    for (int i = start; i <= end; i++)
                    {
                        var measure = TextRenderer.MeasureText(((char) i).ToString(), font);
                        rect.Width = measure.Width;
                        rect.Height = measure.Height;
                        if (rect.Right > width)
                        {
                            rect.X = 0;
                            rect.Y += measure.Height + 10;
                        }

                        TextRenderer.DrawText(graphics, ((char) i).ToString(), font,
                            new Point((int) rect.X, (int) rect.Y), Color.White);
                        var minX = (int) rect.Right;
                        var minY = (int) rect.Top;
                        var maxX = (int) rect.Left;
                        var maxY = (int) rect.Bottom;
                        var solidPixels = 0;
                        for (var x = (int) rect.X; x < Math.Min((int) rect.Right, target.Width); x++)
                        for (var y = (int) rect.Y; y < Math.Min((int) rect.Bottom, target.Width); y++)
                        {
                            var pixel = target.GetPixel(x, y);
                            if (pixel != transparent)
                            {
                                if (pixel != Color.White) target.SetPixel(x, y, Color.Transparent);
                                if (x > maxX) maxX = x;
                                if (x < minX) minX = x;
                                solidPixels++;
                            }
                        }

                        if (solidPixels == 0)
                        {
                            maxX = (int) (rect.Right / 2f);
                            minX = (int) rect.Left;
                        }

                        chars.Add((char) i);
                        bounds.Add(new Rectangle((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height));
                        var crop = new Rectangle(minX - (int) rect.X, minY - (int) rect.Y, maxX - minX, maxY - minY);
                        Console.WriteLine("Crop for {0} is {1}", ((char) i).ToString(), crop);
                        cropping.Add(crop);
                        if (maxX - minX > spacing) spacing = maxX - minX;
                        rect.X = rect.X + measure.Width + 10;
                    }
                }
            }

            var j = 0;
            foreach (var b in cropping)
            {
                var letterWidth = (int) Math.Max(b.Width + size / 10, spacing / 3);
                var kerningLeft = 0;
                var kerningRight = 0;
                var k = new Vector3(kerningLeft, letterWidth, kerningRight);
                kerning.Add(k);
                j++;
            }

            var texture = new Texture2D(ServiceLocator.Get<GraphicsDeviceManager>().GraphicsDevice, target.Width,
                target.Height);
            var data = new Microsoft.Xna.Framework.Color[target.Width * target.Height];
            for (var x = 0; x < target.Width; x++)
            for (var y = 0; y < target.Height; y++)
            {
                var pixel = target.GetPixel(x, y);
                data[target.Width * y + x] = new Microsoft.Xna.Framework.Color(pixel.R, pixel.G, pixel.B, pixel.A);
            }

            target.Dispose();
            texture.SetData(data);
            using (var stream = new FileStream("test.png", FileMode.Create))
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
            }

            var f = (SpriteFont) constructor.Invoke(
                new object[] {texture, bounds, cropping, chars, 0, 0, kerning, null});

            ServiceLocator.Get<IResourceService>().Register(f, name);
            GC.Collect();
        }

        public static void LoadStyle(string path, string name)
        {
            var text = File.ReadAllText(path + ".json");
            ServiceLocator.Get<IResourceService>().Register(InterfaceStyle.CreateFrom(text), name);
        }

        public static void LoadSong(string path, string name)
        {
            var s = Song.FromUri(name, new Uri(path));
            ServiceLocator.Get<IResourceService>().Register(s, name);
        }

        public static void LoadTiles(string path)
        {
            var json = File.ReadAllText(path);
            ServiceLocator.Get<ITileRepository>().RegisterJson(json);
        }

        public static void LoadSpritesheet(string path, int textureWidth, int textureHeight, string name)
        {
            using (var fs = new FileStream(path, FileMode.Open))
            {
                var texture = Texture2D.FromStream(ServiceLocator.Get<GraphicsDeviceManager>().GraphicsDevice, fs);
                var spriteSheet = new Spritesheet(texture, textureWidth, textureHeight);
                spriteSheet.Unpack();
                ServiceLocator.Get<IResourceService>().Register(spriteSheet, name);
            }
        }
    }
}