using Game5.Env;
using Game5.UI.Styling;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.Service.Services.Interfaces
{
    public interface IResourceService
    {
        void FlushQueryCache();
        Entity GetEntity(string name);
        Env.EntityModel GetEntityModel(string name);
        Microsoft.Xna.Framework.Graphics.SpriteFont GetFont(string font);
        InterfaceStyle GetStyle(string name);
        Texture2D GetTexture(string query);
        void Load(string resourceFile);
        void LoadFont(string font, string name);
        void Register(InterfaceStyle style, string name);
        void Register(Microsoft.Xna.Framework.Media.Song song, string name);
        void Register(Microsoft.Xna.Framework.Graphics.SpriteFont font, string name);
        void Register(Graphics.Spritesheet spriteSheet, string name);
        void Register(Texture2D texture, string name);
    }
}