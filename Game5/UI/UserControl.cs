using System;
using System.Collections.Generic;
using System.Linq;
using Game5.Data.Attributes.UI;
using Game5.Input;
using Game5.Resource;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Game5.UI.Components;
using Game5.UI.UIEventArgs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;
using SpriteFont = Microsoft.Xna.Framework.Graphics.SpriteFont;

namespace Game5.UI
{
    public class UserControl
    {
        public bool Active;
        private UserControl aligned;
        private string alignedName;

        protected List<UserControl> children;
        private Rectangle dest;
        private bool dirty;

        private int fontSize;
        protected int height;
        protected bool ignoreScaling;


        protected Matrix? matrix;
        private int paddingX;
        private int paddingY;
        private RenderTarget2D renderTarget;

        protected int width;

        public UserControl()
        {
            width = height = -1;
            children = new List<UserControl>();
            fontSize = 14;
            dirty = true;
        }

        [UIDocField("backcolor")] public Color BackColor { get; set; }

        [UIDocField("forecolor")] public Color ForeColor { get; set; }

        [UIDocField("texture")] public string TextureName { get; set; }

        [UIDocField("textureBack")] public string TextureBackName { get; set; }

        [UIDocField("font")] public string FontName { get; set; }

        [UIDocField("anchor")] public AnchorPoint Anchor { get; set; }

        [UIDocField("width")] public string DynamicWidth { get; set; }

        [UIDocField("height")] public string DynamicHeight { get; set; }

        [UIDocField("name")] public string Name { get; set; }

        [UIDocField("clipped")] public bool IsClipped { get; set; }

        [UIDocField("padding-x")]
        public int HorizontalPadding
        {
            get => (int) (paddingX * UserInterface.Active.Scale);
            set => paddingX = value;
        }

        [UIDocField("padding-y")]
        public int VerticalPadding
        {
            get => (int) (paddingY * UserInterface.Active.Scale);
            set => paddingY = value;
        }

        [UIDocField("fontsize")]
        public int FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                MarkDirty();
            }
        }

        [UIDocField("aligned")]
        public string Aligned
        {
            get => alignedName;
            set
            {
                alignedName = value;
                AlignAfter(value);
            }
        }

        [UIDocField("visible")] public bool Visible { get; set; } = true;

        public int Width
        {
            get
            {
                if (width == -1 || dirty)
                {
                    if (DynamicWidth.EndsWith("em"))
                    {
                        var s = new string(DynamicWidth.Take(DynamicWidth.Length - 2).ToArray());
                        width = (int) (double.Parse(s) * fontSize);
                        ignoreScaling = false;
                    }
                    else if (DynamicWidth.EndsWith("px"))
                    {
                        var s = new string(DynamicWidth.Take(DynamicWidth.Length - 2).ToArray());
                        width = int.Parse(s);
                        ignoreScaling = false;
                    }
                    else if (DynamicWidth.EndsWith("tx"))
                    {
                        var s = new string(DynamicWidth.Take(DynamicWidth.Length - 2).ToArray());
                        width = (int) (double.Parse(s) * Texture.Width);
                        ignoreScaling = false;
                    }
                    else if (DynamicWidth.EndsWith("%"))
                    {
                        var s = new string(DynamicWidth.Take(DynamicWidth.Length - 1).ToArray());
                        var factor = float.Parse(s) / 100.0f;
                        var parentWidth = UserInterface.Active.View.Width;
                        ignoreScaling = false;
                        if (Parent != null)
                        {
                            parentWidth = Parent.Size.X;
                            ignoreScaling = true;
                        }

                        width = (int) (parentWidth * factor);
                    }
                    else
                    {
                        width = int.Parse(DynamicWidth);
                        ignoreScaling = false;
                    }
                }

                return width;
            }
        }

        public int Height
        {
            get
            {
                if (height == -1 || dirty)
                {
                    if (DynamicHeight.EndsWith("em"))
                    {
                        var s = new string(DynamicHeight.Take(DynamicHeight.Length - 2).ToArray());
                        height = (int) (double.Parse(s) * fontSize);
                        ignoreScaling = false;
                    }
                    else if (DynamicHeight.EndsWith("px"))
                    {
                        var s = new string(DynamicHeight.Take(DynamicHeight.Length - 2).ToArray());
                        height = int.Parse(s);
                        ignoreScaling = false;
                    }
                    else if (DynamicWidth.EndsWith("tx"))
                    {
                        var s = new string(DynamicWidth.Take(DynamicWidth.Length - 2).ToArray());
                        height = (int) (double.Parse(s) * Texture.Height);
                        ignoreScaling = false;
                    }
                    else if (DynamicHeight.EndsWith("%"))
                    {
                        var s = new string(DynamicHeight.Take(DynamicHeight.Length - 1).ToArray());
                        var factor = float.Parse(s) / 100.0f;
                        var parentHeight = UserInterface.Active.View.Height;
                        ignoreScaling = false;
                        if (Parent != null)
                        {
                            parentHeight = Parent.Size.Y;
                            ignoreScaling = true;
                        }

                        height = (int) (parentHeight * factor);
                    }
                    else
                    {
                        height = int.Parse(DynamicHeight);
                        ignoreScaling = false;
                    }
                }

                return height;
            }
        }

        public Closure OnClick { get; set; }
        public Closure OnHover { get; set; }
        public Closure OnDown { get; set; }
        public Closure OnLeave { get; set; }
        public Closure OnEnter { get; set; }

        protected SpriteFont Font => ServiceLocator.Get<IResourceService>().GetFont(FontName);

        protected Texture2D Texture => ServiceLocator.Get<IResourceService>().GetTexture(TextureName);

        protected Texture2D TextureBack => ServiceLocator.Get<IResourceService>().GetTexture(TextureBackName);

        protected Rectangle DestRect
        {
            get
            {
                if (dirty || dest == default(Rectangle))
                {
                    dest = CalcDestRect();
                    dirty = false;
                }

                return dest;
            }
        }

        protected SpriteBatch SpriteBatch => ServiceLocator.Get<SpriteBatch>();

        protected GraphicsDeviceManager Graphics => ServiceLocator.Get<GraphicsDeviceManager>();

        protected IInput Input => ServiceLocator.Get<IInput>();

        public UserControl Parent { get; protected set; }

        public Point Size
        {
            get
            {
                if (!ignoreScaling)
                    return new Point((int) (Width * UserInterface.Active.Scale),
                        (int) (Height * UserInterface.Active.Scale));
                return new Point(Width, Height);
            }
        }

        public bool HasChildren => children.Count > 0;

        public List<UserControl> Children => children;

        public event EventHandler OnMouseClick;
        public event EventHandler OnMouseDown;
        public event EventHandler OnMouseUp;
        public event EventHandler OnMouseHover;
        public event EventHandler OnMouseEnter;
        public event EventHandler OnMouseLeave;

        ~UserControl()
        {
            if (renderTarget != null) renderTarget.Dispose();
        }

        public void SetMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }

        public Matrix GetMatrix()
        {
            return (Matrix) matrix;
        }

        public virtual void PreDrawChildren()
        {
            if (IsClipped) BindRenderTarget();
        }

        public virtual void PostDrawChildren()
        {
            if (IsClipped) UnbindRenderTarget();
        }

        public virtual void Draw()
        {
            children.ForEach(x =>
            {
                if (x.Visible && x.Size.X != 0 && x.Size.Y != 0)
                {
                    x.PreDrawChildren();
                    x.Draw();
                    x.PostDrawChildren();
                }
            });
        }

        public virtual void Update()
        {
            if (Size.X != DestRect.Width || Size.Y != DestRect.Height) MarkDirty();
            var mousePos = Input.GetMousePos();
            (int x, int y) lastMousePos = (Input.GetPreviousMouseState().X, Input.GetPreviousMouseState().Y);
            if (matrix != null)
            {
                var invertedMatrix = Matrix.Invert((Matrix) matrix);
                var v1 = Vector2.Transform(new Vector2(mousePos.x, mousePos.y), invertedMatrix);
                var v2 = Vector2.Transform(new Vector2(lastMousePos.x, lastMousePos.y), invertedMatrix);
                mousePos.x = (int) v1.X;
                mousePos.y = (int) v1.Y;
                lastMousePos.x = (int) v2.X;
                lastMousePos.y = (int) v2.Y;
            }

            var lastMouseInside =
                lastMousePos.x > DestRect.X
                && lastMousePos.x < DestRect.X + DestRect.Width
                && lastMousePos.y > DestRect.Y
                && lastMousePos.y < DestRect.Y + DestRect.Height;

            var mouseInside =
                mousePos.x > DestRect.X
                && mousePos.x < DestRect.X + DestRect.Width
                && mousePos.y > DestRect.Y
                && mousePos.y < DestRect.Y + DestRect.Height;

            if (Visible)
            {
                if (mouseInside && Input.GetCurrentMouseState().LeftButton == ButtonState.Pressed &&
                    Input.GetPreviousMouseState().LeftButton == ButtonState.Released)
                {
                    OnMouseClick?.Invoke(this, new MouseEventArgs(Input.GetCurrentMouseState()));
                    MouseClick();
                }

                if (mouseInside && Input.IsDown(MouseButton.LeftButton))
                {
                    UserInterface.Active.ForEach(x =>
                    {
                        if (this is Panel && x is Panel) x.Active = false;
                    });
                    Active = true;
                    MouseDown();
                    OnMouseDown?.Invoke(this, new MouseEventArgs(Input.GetCurrentMouseState()));
                }
                else if (!mouseInside && Input.IsDown(MouseButton.LeftButton))
                {
                    Active = false;
                }

                if (mouseInside && Input.IsUp(MouseButton.LeftButton))
                {
                    OnMouseUp?.Invoke(this, new MouseEventArgs(Input.GetCurrentMouseState()));
                    MouseUp();
                }

                if (mouseInside)
                {
                    OnMouseHover?.Invoke(this, new MouseEventArgs(Input.GetCurrentMouseState()));
                    MouseHover();
                }

                if (mouseInside && !lastMouseInside)
                {
                    OnMouseEnter?.Invoke(this, new MouseEventArgs(Input.GetCurrentMouseState()));
                    MouseEnter();
                }

                if (!mouseInside && lastMouseInside)
                {
                    OnMouseLeave?.Invoke(this, new MouseEventArgs(Input.GetCurrentMouseState()));
                    MouseLeave();
                }
            }

            for (var i = children.Count - 1; i >= 0; i--) children[i].Update();
        }

        public Point GetTransformedMousePosition(MouseState state)
        {
            (int x, int y) mousePos = (state.X, state.Y);

            if (matrix != null)
            {
                var invertedMatrix = Matrix.Invert((Matrix) matrix);
                var v1 = Vector2.Transform(new Vector2(mousePos.x, mousePos.y), invertedMatrix);
                mousePos.x = (int) v1.X;
                mousePos.y = (int) v1.Y;
            }

            return new Point(mousePos.x, mousePos.y);
        }

        public bool IntersectsDestRect(Point p)
        {
            return
                p.X > DestRect.X
                && p.X < DestRect.X + DestRect.Width
                && p.Y > DestRect.Y
                && p.Y < DestRect.Y + DestRect.Height;
        }

        public bool IntersectsRect(Point p, Rectangle rect)
        {
            return
                p.X > rect.X
                && p.X < rect.X + rect.Width
                && p.Y > rect.Y
                && p.Y < rect.Y + rect.Height;
        }

        public void MarkDirty()
        {
            dirty = true;
        }

        protected virtual void MouseHover()
        {
            OnHover?.Call(this);
        }

        protected virtual void MouseDown()
        {
            OnDown?.Call(this);
        }

        protected virtual void MouseClick()
        {
            OnClick?.Call(this);
        }

        protected virtual void MouseUp()
        {
        }

        protected virtual void MouseEnter()
        {
            OnEnter?.Call(this);
        }

        protected virtual void MouseLeave()
        {
            OnLeave?.Call(this);
        }

        public void SetParent(UserControl control)
        {
            Parent = control;
        }

        /// <summary>
        ///     Called when the active UI changes. A good place to clear up some memory that can be recreated when the UI is again
        ///     activated. GC.Collect is called after this method.
        /// </summary>
        public virtual void OnDeactivated()
        {
        }

        /// <summary>
        ///     Called when the UI this control belongs to is activated
        /// </summary>
        public virtual void OnActivated()
        {
        }

        private void AlignAfter(string name)
        {
            if (name == "") aligned = null;
            else aligned = UserInterface.Active.DeepFindControl(name);

            if (aligned == null) alignedName = "";
            MarkDirty();
        }

        public Point TryGetParentLocation()
        {
            if (Parent == null) return new Point(0, 0);
            return Parent.DestRect.Location;
        }

        /// <summary>
        ///     Uses the transform matrix to start a batch
        /// </summary>
        public void BeginDraw(SamplerState samplerState = null)
        {
            if (!IsClipped) SpriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null, matrix);
            else SpriteBatch.Begin(SpriteSortMode.Deferred, null, samplerState, null, null, null);
        }

        public Rectangle CalcDestRect()
        {
            if (aligned != null)
                switch (Anchor)
                {
                    case AnchorPoint.Left:
                        return new Rectangle(aligned.DestRect.Location - new Point(Size.X + HorizontalPadding, 0),
                            Size);
                    case AnchorPoint.Right:
                        return new Rectangle(
                            aligned.DestRect.Location - new Point(-aligned.Size.X - HorizontalPadding, 0), Size);
                    case AnchorPoint.TopCenter:
                        return new Rectangle(aligned.DestRect.Location - new Point(0, Size.Y + VerticalPadding), Size);
                    case AnchorPoint.BottomCenter:
                        return new Rectangle(
                            aligned.DestRect.Location - new Point(0, -aligned.Size.Y - VerticalPadding), Size);
                    case AnchorPoint.BottomLeft:
                        return new Rectangle(
                            aligned.DestRect.Location - new Point(HorizontalPadding, -aligned.Size.Y - VerticalPadding),
                            Size);
                    case AnchorPoint.BottomRight:
                        return new Rectangle(
                            aligned.DestRect.Location - new Point(-aligned.Size.X - HorizontalPadding + Size.X,
                                -aligned.Size.Y - VerticalPadding), Size);
                    case AnchorPoint.TopLeft:
                        return new Rectangle(
                            aligned.DestRect.Location - new Point(Size.X + HorizontalPadding, Size.Y + VerticalPadding),
                            Size);
                    case AnchorPoint.TopRight:
                        return new Rectangle(
                            aligned.DestRect.Location - new Point(-aligned.Size.X - HorizontalPadding,
                                Size.Y + VerticalPadding), Size);
                    default:
                        alignedName = "";
                        aligned = null;
                        MarkDirty();
                        return DestRect;
                }

            if (Parent == null)
            {
                var rect = new Rectangle(UserInterface.GetAnchorPoints(UserInterface.Active.View, Size)[(int) Anchor],
                    Size);
                rect.X += HorizontalPadding;
                rect.Y += VerticalPadding;
                return rect;
            }
            else
            {
                var rect = new Rectangle(UserInterface.GetAnchorPoints(Parent.DestRect, Size)[(int) Anchor], Size);
                rect.X += HorizontalPadding;
                rect.Y += VerticalPadding;
                return rect;
            }
        }

        public Point GetRenderLocation()
        {
            if (IsClipped)
                return new Point(0, 0);
            return DestRect.Location - GetFirstClippedPosition();
        }

        public Point GetSumLocation()
        {
            var parent = Parent;
            var loc = new Point();
            while (parent != null)
            {
                loc += parent.DestRect.Location;
                parent = parent.Parent;
            }

            return loc;
        }

        public Point GetFirstClippedPosition()
        {
            var parent = Parent;
            while (parent != null)
            {
                if (parent.IsClipped)
                {
                    parent.MarkDirty();
                    return parent.DestRect.Location;
                }

                parent = parent.Parent;
            }

            return new Point();
        }

        public int GetMaxChildHeight()
        {
            if (children.Count == 0) return DestRect.Height;
            return Math.Max(DestRect.Height, children.Max(x => x.DestRect.Bottom));
        }

        public int GetMaxChildWidth()
        {
            if (children.Count == 0) return DestRect.Height;
            return Math.Max(DestRect.Height, children.Max(x => x.DestRect.Right));
        }

        public void AddChild(UserControl ui)
        {
            children.Add(ui);
            ui.Parent = this;
        }

        public void RemoveChild(UserControl ui)
        {
            children.Remove(ui);
            ui.Parent = null;
            ui.matrix = null;
        }

        protected void BindRenderTarget()
        {
            if (renderTarget == null || renderTarget.Width != Size.X || renderTarget.Height != Size.Y)
            {
                if (renderTarget != null) renderTarget.Dispose();
                renderTarget = new RenderTarget2D(SpriteBatch.GraphicsDevice, Size.X, Size.Y,
                    false, SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                    SpriteBatch.GraphicsDevice.PresentationParameters.DepthStencilFormat, 0,
                    RenderTargetUsage.PreserveContents);
            }

            UserInterface.Active.GetDrawUtils().Bind(renderTarget);
            SpriteBatch.GraphicsDevice.SetRenderTarget(UserInterface.Active.GetDrawUtils().Top());
            SpriteBatch.GraphicsDevice.Clear(Color.Transparent);
        }

        protected void UnbindRenderTarget()
        {
            UserInterface.Active.GetDrawUtils().Unbind();
            SpriteBatch.GraphicsDevice.SetRenderTargets(UserInterface.Active.GetDrawUtils().Top());
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, matrix);
            var target = new Rectangle(DestRect.Location - GetFirstClippedPosition(), DestRect.Size);
            SpriteBatch.Draw(renderTarget, target, Color.White);
            SpriteBatch.End();
        }
    }
}