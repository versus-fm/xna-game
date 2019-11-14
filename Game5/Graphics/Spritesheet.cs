using System.Collections.Generic;
using Game5.Data.DataStructures;
using Game5.Service;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.Graphics
{
    public class Spritesheet
    {
        private readonly Dictionary<string, (int x, int y)> mappedIndices;
        private readonly Texture2D texture;
        private readonly Array2D<Texture2D> textures;
        private readonly int textureWidth;
        private readonly int textureHeight;

        public Spritesheet(Texture2D texture, int textureWidth, int textureHeight)
        {
            mappedIndices = new Dictionary<string, (int x, int y)>();
            textures = new Array2D<Texture2D>(texture.Width / textureWidth, texture.Height / textureHeight);
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            this.texture = texture;
        }

        public Texture2D this[int x, int y] => textures[x, y];

        public Texture2D this[string key]
        {
            get
            {
                if (!mappedIndices.ContainsKey(key)) throw new KeyNotFoundException();
                var (x, y) = mappedIndices[key];
                return textures[x, y];
            }
        }

        public void Unpack()
        {
            var spriteBatch = ServiceLocator.Get<SpriteBatch>();
            for (var x = 0; x < textures.GetWidth(); x++)
            for (var y = 0; y < textures.GetHeight(); y++)
            {
                var tex = new Texture2D(spriteBatch.GraphicsDevice, textureWidth, textureHeight);
                var texData = new Color[textureWidth * textureHeight];
                texture.GetData(0, new Rectangle(x * textureWidth, y * textureHeight, textureWidth, textureHeight),
                    texData, 0, textureWidth * textureHeight);
                tex.SetData(texData);

                textures[x, y] = tex;
            }
        }

        public void MapIndex(int x, int y, string key)
        {
            mappedIndices.Add(key, (x, y));
        }
    }
}