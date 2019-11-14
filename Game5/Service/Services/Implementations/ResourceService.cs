using System.Collections.Generic;
using System.IO;
using Game5.Data.Attributes.DependencyInjection;
using Game5.Data.Attributes.Service;
using Game5.Env;
using Game5.Resource;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using InterfaceStyle = Game5.UI.Styling.InterfaceStyle;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IResourceService))]
    public class ResourceService : IResourceService
    {
        private readonly Dictionary<string, Texture2D> cachedTextureQueries;
        private readonly ContentManager content;
        private readonly Dictionary<string, Microsoft.Xna.Framework.Audio.SoundEffect> effects;
        private readonly Dictionary<string, Env.EntityModel> entityModels;
        private readonly Dictionary<string, Microsoft.Xna.Framework.Graphics.SpriteFont> fonts;
        private string resourceFile;
        private readonly Dictionary<string, Microsoft.Xna.Framework.Media.Song> songs;
        private readonly Dictionary<string, Graphics.Spritesheet> spritesheets;

        private readonly Dictionary<string, InterfaceStyle> styles;
        private readonly Dictionary<string, Texture2D> textures;

        [FactoryConstructor]
        public ResourceService(ContentManager content)
        {
            textures = new Dictionary<string, Texture2D>();
            songs = new Dictionary<string, Microsoft.Xna.Framework.Media.Song>();
            effects = new Dictionary<string, Microsoft.Xna.Framework.Audio.SoundEffect>();
            spritesheets = new Dictionary<string, Graphics.Spritesheet>();
            fonts = new Dictionary<string, Microsoft.Xna.Framework.Graphics.SpriteFont>();
            entityModels = new Dictionary<string, Env.EntityModel>();
            styles = new Dictionary<string, InterfaceStyle>();
            cachedTextureQueries = new Dictionary<string, Texture2D>();
            this.content = content;
        }

        public void Load(string resourceFile)
        {
            this.resourceFile = resourceFile;
            if (!File.Exists(resourceFile)) throw new FileNotFoundException();
            var json = File.ReadAllText(resourceFile);
            var obj = JsonConvert.DeserializeObject<ResourceObject>(json);
            if (obj.Resources.Songs != null)
                obj.Resources.Songs.ForEach(x =>
                {
                    songs.Add(x.name, content.Load<Microsoft.Xna.Framework.Media.Song>(x.name));
                });
            if (obj.Resources.SoundEffects != null)
                obj.Resources.SoundEffects.ForEach(x =>
                {
                    effects.Add(x.name, content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>(x.path));
                });
            if (obj.Resources.Spritesheets != null)
                obj.Resources.Spritesheets.ForEach(x =>
                {
                    var ss = new Graphics.Spritesheet(content.Load<Texture2D>(x.path), x.width, x.height);
                    ss.Unpack();
                    x.Aliases.ForEach(y => { ss.MapIndex(y.x, y.y, y.name); });
                    spritesheets.Add(x.name, ss);
                });
            if (obj.Resources.Textures != null)
                obj.Resources.Textures.ForEach(x => { textures.Add(x.name, content.Load<Texture2D>(x.path)); });
            if (obj.Resources.Fonts != null)
                obj.Resources.Fonts.ForEach(x =>
                {
                    fonts.Add(x.name, content.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>(x.path));
                });
            if (obj.Resources.EntityModels != null)
                obj.Resources.EntityModels.ForEach(x =>
                {
                    var j = File.ReadAllBytes(@"Content\" + x.path + ".json");
                    //entityModels.Add(x.name, new Env.EntityModel(j));
                });
            if (obj.Resources.InterfaceStyles != null)
                obj.Resources.InterfaceStyles.ForEach(x =>
                {
                    var text = File.ReadAllText(@"Content\" + x.path + ".json");
                    styles.Add(x.name, InterfaceStyle.CreateFrom(text));
                });
        }

        public Texture2D GetTexture(string query)
        {
            if (string.IsNullOrEmpty(query)) return null;
            if (cachedTextureQueries.ContainsKey(query)) return cachedTextureQueries[query];
            var splitQuery = query.Split('/');
            if (splitQuery[0] == "spritesheet")
            {
                var name = splitQuery[1];
                if (splitQuery[2] == "index")
                {
                    var texture = spritesheets[name][int.Parse(splitQuery[3]), int.Parse(splitQuery[4])];
                    cachedTextureQueries.Add(query, texture);
                    return texture;
                }

                if (splitQuery[2] == "name")
                {
                    var texture = spritesheets[name][splitQuery[3]];
                    cachedTextureQueries.Add(query, texture);
                    return texture;
                }
            }
            else if (splitQuery[0] == "texture")
            {
                var texture = textures[splitQuery[1]];
                cachedTextureQueries.Add(query, texture);
                return texture;
            }
            else
            {
                var texture = textures[splitQuery[0]];
                cachedTextureQueries.Add(query, texture);
                return texture;
            }

            return null;
        }

        /// <summary>
        ///     Allows you to preload a font before loading any other resources. Useful for drawing text in a loading screen before
        ///     loading fonts
        /// </summary>
        /// <param name="font"></param>
        /// <param name="name"></param>
        public void LoadFont(string font, string name)
        {
            fonts.Add(name, content.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>(font));
        }

        public Microsoft.Xna.Framework.Graphics.SpriteFont GetFont(string font)
        {
            if (!fonts.ContainsKey(font)) return null;
            return fonts[font];
        }

        public InterfaceStyle GetStyle(string name)
        {
            if (!styles.ContainsKey(name)) return null;
            return styles[name];
        }

        public Env.EntityModel GetEntityModel(string name)
        {
            if (!entityModels.ContainsKey(name)) return null;
            return entityModels[name];
        }

        public Entity GetEntity(string name)
        {
            if (!entityModels.ContainsKey(name)) return null;
            return null;// entityModels[name].GetEntity();
        }

        public void FlushQueryCache()
        {
            cachedTextureQueries.Clear();
        }

        public void Register(Texture2D texture, string name)
        {
            textures.Add(name, texture);
        }

        public void Register(Microsoft.Xna.Framework.Graphics.SpriteFont font, string name)
        {
            fonts.Add(name, font);
        }

        public void Register(Microsoft.Xna.Framework.Media.Song song, string name)
        {
            songs.Add(name, song);
        }

        public void Register(InterfaceStyle style, string name)
        {
            styles.Add(name, style);
        }

        public void Register(Graphics.Spritesheet spriteSheet, string name)
        {
            spritesheets.Add(name, spriteSheet);
        }
    }
}