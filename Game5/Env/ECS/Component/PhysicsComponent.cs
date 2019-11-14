using Game5.Data;
using Game5.Data.Attributes;
using Game5.Graphics;
using Microsoft.Xna.Framework;
using StorageAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5.Env.Components
{
    [Component("physics")]
    public class PhysicsComponent : EntityComponent
    {
        [StorageField]
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }
        [StorageField]
        public float Drag { get; set; }
        [StorageField]
        public Vector2 Gravity { get; set; }
        public bool IsGrounded { get; set; }
        [StorageField]
        public Vector2[] CollisionMask { get => collisionMask; set => collisionMask = value; }
        [StorageField]
        private Vector2 velocity;


        private List<RectangleF> tt;

        private Collider collider;
        private Vector2[] collisionMask;

        public PhysicsComponent()
        {
        }
        public virtual void OnCollision()
        {

        }
        public override void Start()
        {
            this.collisionMask = new Vector2[]
            {
                new Vector2(-1, -1 * (Parent.Size.Y / Parent.Size.X)),
                new Vector2(1, -1 * (Parent.Size.Y / Parent.Size.X)),
                new Vector2(1, 1 * (Parent.Size.Y / Parent.Size.X)),
                new Vector2(-1, 1 * (Parent.Size.Y / Parent.Size.X))
            };
            this.Drag = 1;
            base.Start();
        }
        public override void EnterWorld()
        {
            //collider = Parent.world.CreateCollider(collisionMask, Parent.size.X / 2, ColliderType.Entity);
            collider.SetSolid(true);
            base.EnterWorld();
        }
        public override void LeaveWorld()
        {
            //Parent.world.DestroyCollider(collider);
            base.LeaveWorld();
        }
        public override void Update()
        {
            lock(componentLock)
            {
                var gTime = ServiceLocator.GetGameTime();

                //IsGrounded = Parent.world.QueryColliders(new Rectangle(collider.GetBounds().Location + new Point(0, 1), Parent.size)).Count(x => collider.GetCollision(x, new Vector2(0, 1)).Collision && x != collider) > 0;

                if (!IsGrounded) Velocity += Gravity * gTime.Delta();
                if (Drag != 1) Velocity *= Drag * gTime.Delta();
                Parent.position += velocity;
                UpdateCollider();
                ResolveCollisions();
                UpdateCollider();
            }
        }
        public void AddForce(Vector2 velocity)
        {
            this.velocity += velocity;
        }

        private void UpdateCollider()
        {
            collider.SetTranslation(Parent.position + Parent.size.ToVector2() / 2);
        }

        private void ResolveCollisions()
        {
            //var colliders = Parent.world.QueryColliders(collider.GetBounds()).Where(x => x != collider);
            //foreach(var collider in colliders)
            //{
            //    var collision = this.collider.GetCollision(collider);
            //    if(collision.Collision && collider.IsSolid() && this.collider.IsSolid())
            //    {
            //        Parent.position -= collision.Axis * collision.Overlap;
            //        UpdateCollider();
            //    }
            //}

        }
        private void HandleMovementWithCollision(List<RectangleF> tiles)
        {
            foreach (var rect in tiles)
            {
                var prediction = new RectangleF(Parent.position.X + 1, Parent.position.Y + velocity.Y, Parent.size.X - 2, Parent.size.Y);
                if (prediction.Intersects(rect))
                {
                    //down
                    if (velocity.Y > 0)
                    {
                        velocity.Y -= prediction.Bottom - rect.Top;
                        if (velocity.Y < 0) velocity.Y = 0;
                    }

                    else if (velocity.Y < 0)
                    {
                        velocity.Y -= prediction.Top - (rect.Top + rect.Height);
                        if (velocity.Y > 0) velocity.Y = 0;
                    }
                }
            }
            var old = Parent.position.Y;
            Parent.position.Y += velocity.Y;
            var delta = old - Parent.position.Y;
            foreach (var rect in tiles)
            {
                var prediction = new RectangleF(Parent.position.X + Parent.size.X / 4, Parent.position.Y, Parent.size.X - Parent.size.X / 2, Parent.size.Y);
                if (prediction.Intersects(rect))
                {
                    var intersect = RectangleF.Intersection(prediction, rect);
                    if (delta > 0) Parent.position.Y += intersect.Height;
                    else Parent.position.Y -= intersect.Height;
                }
            }

            foreach (var rect in tiles)
            {
                var prediction = new RectangleF(Parent.position.X + velocity.X, Parent.position.Y + 1, Parent.size.X, Parent.size.Y - 2);
                if (prediction.Intersects(rect))
                {
                    if (velocity.X > 0)
                    {
                        velocity.X -= prediction.Right - rect.Left;
                        if (velocity.X < 0) velocity.X = 0;
                    }

                    else if (velocity.X < 0)
                    {
                        velocity.X -= prediction.Left - (rect.Left + rect.Width);
                        if (velocity.X > 0) velocity.X = 0;
                    }
                }
            }
            old = Parent.position.X;
            Parent.position.X += velocity.X;
            delta = old - Parent.position.X;
            foreach (var rect in tiles)
            {
                var prediction = new RectangleF(Parent.position.X, Parent.position.Y + Parent.size.Y / 4, Parent.size.X, Parent.size.Y - Parent.size.Y / 2);
                if (prediction.Intersects(rect))
                {
                    var intersect = RectangleF.Intersection(prediction, rect);
                    if (delta > 0) Parent.position.X += intersect.Width;
                    else Parent.position.X -= intersect.Width;
                    break;
                }
            }
        }
        private (float x, float y, float w, float h) GetXPrediction()
        {
            return (Parent.position.X + Velocity.X, Parent.position.Y, Parent.size.X, Parent.size.Y);
        }
        private (float x, float y, float w, float h) GetYPrediction()
        {
            return (Parent.position.X, Parent.position.Y + Velocity.Y, Parent.size.X, Parent.size.Y);
        }
        private float Left((float x, float y, float w, float h) rec)
        {
            return rec.x;
        }
        private float Right((float x, float y, float w, float h) rec)
        {
            return rec.x + rec.w;
        }
        private float Top((float x, float y, float w, float h) rec)
        {
            return rec.y;
        }
        private float Bottom((float x, float y, float w, float h) rec)
        {
            return rec.y + rec.h;
        }
        private Rectangle Rect((float x, float y, float w, float h) rec)
        {
            return new Rectangle((int)Math.Round(rec.x, 0),
                (int)Math.Round(rec.y, 0),
                (int)Math.Round(rec.w, 0),
                (int)Math.Round(rec.h, 0));
        }
        public override void Destroy()
        {
            base.Destroy();
        }
        public override void Draw()
        {
            //tt.ForEach(x => SpriteBatch.DrawRectangle(x.ToRectangle(), Color.Red));
            Console.WriteLine(collider.GetBounds());
            SpriteBatch.DrawBorder(collider.GetBounds(), 3, Color.Red);
            base.Draw();
        }
    }
}
