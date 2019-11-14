using Game5.Data.Attributes.DependencyInjection;
using Game5.Data.Attributes.Service;
using Game5.Env;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(ICamera))]
    public class Camera : ICamera
    {
        private Vector2 camPos;

        private GraphicsDeviceManager graphics;
        private Matrix invertedMatrix;
        private bool locked;
        private Matrix matrix;
        private float scale;

        private float scrollLerp;
        private float toZoom;

        [FactoryConstructor]
        public Camera(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            ViewSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            locked = false;
            camPos = new Vector2();
            scale = 1f;
        }

        private Vector2 Origin => new Vector2(ViewSize.X / 2.0f, ViewSize.Y / 2.0f);

        public Entity Focus { get; set; }

        public Rectangle VisibleArea
        {
            get
            {
                var tl = Vector2.Transform(Vector2.Zero, invertedMatrix);
                var tr = Vector2.Transform(new Vector2(ViewSize.X, 0), invertedMatrix);
                var bl = Vector2.Transform(new Vector2(0, ViewSize.Y), invertedMatrix);
                var br = Vector2.Transform(new Vector2(ViewSize.X, ViewSize.Y), invertedMatrix);
                var min = new Vector2(
                    MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                    MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
                var max = new Vector2(
                    MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                    MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
                return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }

        public Point ViewSize { get; set; }

        public void Update()
        {
            //if (Focus != null) camPos = -Focus.Position;
            matrix = Matrix.CreateTranslation(new Vector3(camPos, 0.0f)) *
                     Matrix.CreateScale(scale, scale, 1) *
                     Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
            invertedMatrix = Matrix.Invert(matrix);
        }

        public Matrix GetMatrix()
        {
            return matrix;
        }

        public Matrix GetInverseMatrix()
        {
            return invertedMatrix;
        }

        public float GetScale()
        {
            return scale;
        }

        public void Scale(float scale)
        {
            this.scale += scale;
        }

        public Vector2 GetPosition()
        {
            return camPos;
        }

        public void Translate(Vector2 vector)
        {
            camPos += vector;
        }

        public void LockCamera()
        {
            locked = true;
            camPos = new Vector2();
        }

        public void UnlockCamera()
        {
            locked = false;
        }
    }
}