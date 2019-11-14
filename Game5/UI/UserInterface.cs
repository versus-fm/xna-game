using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Game5.Data.Attributes.UI;
using Game5.Data.Helper;
using Game5.Graphics;
using Game5.Resource;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using InterfaceStyle = Game5.UI.Styling.InterfaceStyle;

namespace Game5.UI
{
    public class UserInterface
    {
        private static Dictionary<string, Type> uiTypes;
        private RenderTarget2D renderTarget;
        private float scale;
        private Point size;
        private readonly List<UserControl> ui;
        private readonly DrawUtils utils;

        public UserInterface()
        {
            ui = new List<UserControl>();
            size = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            utils = new DrawUtils();
        }

        public static UserInterface Active { get; internal set; }

        private SpriteBatch SpriteBatch => ServiceLocator.Get<SpriteBatch>();

        private GraphicsDeviceManager Graphics => ServiceLocator.Get<GraphicsDeviceManager>();

        private InterfaceStyle Style => ServiceLocator.Get<IResourceService>().GetStyle(StyleName);

        public float Scale
        {
            get => scale;
            set
            {
                scale = value;
                foreach (var ctrl in ui) RecursiveControls(ctrl, x => x.MarkDirty());
            }
        }

        public string StyleName { get; set; }

        public Rectangle View => new Rectangle(new Point(0, 0), size);

        ~UserInterface()
        {
            if (renderTarget != null) renderTarget.Dispose();
        }

        public void AddControl(UserControl ctrl)
        {
            ui.Add(ctrl);
        }

        public void RemoveControl(UserControl ctrl)
        {
            ui.Remove(ctrl);
        }

        public void ForEach(Action<UserControl> action)
        {
            foreach (var v in ui)
            {
                var r = RecursiveControls(v, action);
            }
        }

        public UserControl DeepFindControl(string name)
        {
            foreach (var v in ui)
            {
                var r = RecursiveSearch(v, name);
                if (r != null) return r;
            }

            return null;
        }

        private UserControl RecursiveSearch(UserControl ctrl, string name)
        {
            if (ctrl.Name == name) return ctrl;
            if (ctrl.HasChildren)
                foreach (var v in ctrl.Children)
                {
                    var n = RecursiveSearch(v, name);
                    if (n != null) return n;
                }

            return null;
        }

        private UserControl RecursiveControls(UserControl ctrl, Action<UserControl> action)
        {
            action(ctrl);
            if (ctrl.HasChildren)
                foreach (var v in ctrl.Children)
                    RecursiveControls(v, action);
            return null;
        }

        public void PreDraw()
        {
            if (renderTarget == null || size.X != Graphics.PreferredBackBufferWidth ||
                size.Y != Graphics.PreferredBackBufferHeight)
            {
                size = new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
                renderTarget = new RenderTarget2D(SpriteBatch.GraphicsDevice, size.X, size.Y,
                    false, SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                    SpriteBatch.GraphicsDevice.PresentationParameters.DepthStencilFormat, 0,
                    RenderTargetUsage.PreserveContents);
            }

            utils.Bind(renderTarget);
            SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            SpriteBatch.GraphicsDevice.Clear(Color.Transparent);
            ui.ForEach(x =>
            {
                if (x.Visible && x.Size.X != 0 && x.Size.Y != 0)
                {
                    x.PreDrawChildren();
                    x.Draw();
                    x.PostDrawChildren();
                }
            });
            SpriteBatch.GraphicsDevice.SetRenderTarget(null);
        }

        public void PostDraw()
        {
            utils.Clear();
            SpriteBatch.Begin();
            SpriteBatch.Draw(renderTarget, new Rectangle(0, 0, size.X, size.Y), Color.White);
            SpriteBatch.End();
        }

        public void Update()
        {
            for (var i = ui.Count - 1; i >= 0; i--) ui[i].Update();
        }

        public DrawUtils GetDrawUtils()
        {
            return utils;
        }

        public static void ChangeActive(UserInterface ui)
        {
            if (Active != null) Active.ForEach(x => x.OnDeactivated());
            Active = ui;
            if (Active != null) Active.ForEach(x => x.OnActivated());
            GC.Collect();
        }

        public static Point[] GetAnchorPoints(Rectangle area, Point size)
        {
            return new[]
            {
                area.Center - new Point(size.X / 2, size.Y / 2), //Center
                new Point(area.Right - size.X, area.Top + area.Height / 2 - size.Y / 2), //Right
                new Point(area.Left, area.Top + area.Height / 2 - size.Y / 2), //Left
                new Point(area.Center.X - size.X / 2, area.Top), //TopCenter
                new Point(area.Right - size.X, area.Top), //TopRight
                new Point(area.Left, area.Top), //TopLeft
                new Point(area.Center.X - size.X / 2, area.Bottom - size.Y), //BottomCenter
                new Point(area.Right - size.X, area.Bottom - size.Y), //BottomRight
                new Point(area.Left, area.Bottom - size.Y) //BottomLeft
            };
        }

        public static UserInterface FromDocument(string xml, string style = "")
        {
            return FromDocument(new UserInterface(), xml, style);
        }

        public static UserInterface FromDocument(UserInterface ui, string xml, string style = "")
        {
            ui.StyleName = style;
            ui.Scale = 1.0f;
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var root = doc.SelectSingleNode("ui");

            RecursiveNodes(root, x =>
            {
                var ctrl = ConstructControl(ui, x);
                if (x.Parent.Name == "ui")
                {
                    ui.AddControl(ctrl);
                }
                else
                {
                    var parent = ui.DeepFindControl(x.Parent.Attributes["name"].Value);
                    parent.AddChild(ctrl);
                }
            });
            return ui;
        }

        private static void RecursiveNodes(XmlNode node, Action<UINode> action)
        {
            if (node.HasChildNodes)
                foreach (XmlNode n in node.ChildNodes)
                {
                    action(new UINode
                    {
                        Node = n,
                        Parent = node
                    });
                    RecursiveNodes(n, action);
                }
        }

        /// <summary>
        ///     This is used to create an empty <see cref="UserControl" /> while using the default style as defined by the
        ///     <see cref="UserInterface" />
        /// </summary>
        /// <param name="name">The name to give the control</param>
        /// <param name="type">The name of the type as defined by <see cref="UIDocElementAttribute" /></param>
        /// <returns></returns>
        public static UserControl CreateEmpty(string type, string name)
        {
            if (uiTypes == null)
            {
                uiTypes = new Dictionary<string, Type>();
                foreach (var v in TypeHelper.FindAllTypesWithAttribute<UIDocElementAttribute>())
                    if (!v.type.HasAttribute<UIDocHideAttribute>())
                        uiTypes.Add(v.attribute.ElementName, v.type);
            }

            var userControl = (UserControl) Activator.CreateInstance(uiTypes[type]);
            userControl.Name = name;
            ApplyActiveStyle(userControl);
            return userControl;
        }

        /// <summary>
        ///     This is used to create an empty <see cref="UserControl" /> while using the default style as defined by the
        ///     <see cref="UserInterface" />
        /// </summary>
        /// <param name="name">The name to give the control</param>
        /// <returns></returns>
        public static T CreateEmpty<T>(string name) where T : UserControl, new()
        {
            var userControl = new T();
            userControl.Name = name;
            ApplyActiveStyle(userControl);
            return userControl;
        }

        private static void ApplyActiveStyle(UserControl control)
        {
            ApplyStyle(control, Active);
        }

        private static void ApplyStyle(UserControl control, UserInterface ui)
        {
            var properties = MapFields(control.GetType());
            if (ui.Style != null)
            {
                var style = ui.Style.GetStyleFor(control);
                foreach (var styleLine in style)
                {
                    var prop = properties[styleLine.Key];
                    SetProperty(control, prop, styleLine.Value);
                }
            }
        }

        private static UserControl ConstructControl(UserInterface ui, UINode node)
        {
            if (uiTypes == null)
            {
                uiTypes = new Dictionary<string, Type>();
                foreach (var v in TypeHelper.FindAllTypesWithAttribute<UIDocElementAttribute>())
                    if (!v.type.HasAttribute<UIDocHideAttribute>())
                        uiTypes.Add(v.attribute.ElementName, v.type);
            }

            var userControl = (UserControl) Activator.CreateInstance(uiTypes[node.Node.Name]);
            var properties = MapFields(userControl.GetType());

            ApplyStyle(userControl, ui);

            foreach (XmlAttribute attr in node.Node.Attributes)
                if (properties.ContainsKey(attr.Name))
                {
                    var prop = properties[attr.Name];
                    SetProperty(userControl, prop, attr.Value);
                }

            return userControl;
        }

        private static void SetProperty(UserControl control, PropertyInfo prop, string value)
        {
            if (prop.PropertyType == typeof(Color)) prop.SetValue(control, ColorHelper.FromName(value));
            if (prop.PropertyType == typeof(int)) prop.SetValue(control, int.Parse(value));
            if (prop.PropertyType == typeof(float)) prop.SetValue(control, float.Parse(value));
            if (prop.PropertyType == typeof(bool)) prop.SetValue(control, bool.Parse(value));
            if (prop.PropertyType == typeof(long)) prop.SetValue(control, long.Parse(value));
            if (prop.PropertyType == typeof(double)) prop.SetValue(control, double.Parse(value));
            if (prop.PropertyType == typeof(string)) prop.SetValue(control, value);
            if (prop.PropertyType == typeof(byte)) prop.SetValue(control, byte.Parse(value));
            if (prop.PropertyType == typeof(AnchorPoint))
                prop.SetValue(control, Enum.Parse(typeof(AnchorPoint), value));
        }

        private static Dictionary<string, PropertyInfo> MapFields(Type t)
        {
            var map = new Dictionary<string, PropertyInfo>();
            foreach (var prop in t.GetProperties().Where(x => x.GetCustomAttributes<UIDocFieldAttribute>().Count() > 0))
                map.Add(prop.GetCustomAttribute<UIDocFieldAttribute>().AttributeName, prop);
            return map;
        }
    }
}