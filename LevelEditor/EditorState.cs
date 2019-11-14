using System;
using System.Collections.Generic;
using System.Linq;
using Game5;
using Game5.Data.Attributes;
using Game5.Data.Attributes.State;
using Game5.Env;
using Game5.Env.World;
using Game5.Input;
using Game5.Service;
using Game5.Service.Services.Implementations;
using Game5.Service.Services.Interfaces;
using Game5.StateBased;
using Game5.UI;
using Game5.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    [State("editor")]
    public class EditorState : GameState
    {
        private List<string> items;
        //private ThreeTierWorld world;

        public override void Cleanup()
        {
        }

        public override void DrawWorld()
        {
        }

        public override string GetName()
        {
            return "editor";
        }

        public override void Init()
        {
            UserInterface.Active.StyleName = "editorStyle";
            var v = UserInterface.CreateEmpty<GridView<Tile>>("tileList");
            v.DynamicWidth = "30%";
            v.DynamicHeight = "30%";
            v.Anchor = AnchorPoint.TopRight;
            v.IsClipped = true;
            v.SetData((TileRepository) ServiceLocator.Get<ITileRepository>());
            v.ScrollColor = Color.Black;
            v.GrabColor = Color.White;
            v.ScrollWidth = 6;
            v.ScrollHeight = 20;
            v.DynamicGridElementWidth = "32px";
            v.DynamicGridElementHeight = "32px";
            v.GridElementPadding = 2;
            v.BackColor = Color.White;
            v.TextureSelector = x => { return x.TextureName; };
            UserInterface.Active.AddControl(v);
            Console.WriteLine("");
        }

        public override void PostAddonInit()
        {
            CheckButton.MakeGroup("tileTool", "colliderTool");
        }

        private void B1_OnMouseClick(object sender, EventArgs e)
        {
            if (items.Count > 0) items.Remove(items.Last());
        }

        private void B_OnMouseClick(object sender, EventArgs e)
        {
            items.Add(Guid.NewGuid().ToString());
        }

        public override void Update()
        {
            if (ServiceLocator.Get<IInput>().IsClicked(Keys.E)) items.Add(Guid.NewGuid().ToString());
        }
    }
}