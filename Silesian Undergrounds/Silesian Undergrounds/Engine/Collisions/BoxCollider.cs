using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.Collisions
{   
    public class BoxCollider : ICollider
    {
        public Vector2 Position { get; set; }
        public Rectangle Rect { get; set; }
        public GameObject Parent { get; private set; }
        private Texture2D boxTexture;

        private float Width;
        private float Height;
        public float OffsetX { get; private set; }
        public float OffsetY { get; private set; }
        public bool canIgnoreTraps { get; private set; }
        public bool triggerOnly { get; private set; }
        public bool isAggroArea { get; private set; }
        public bool ignoreAggroArea { get; private set; }

        public BoxCollider(GameObject owner, float w, float h, float offsetX, float offsetY, bool trigger, bool ignoreTraps = false, bool ignoreAggroArea = true)
        {
            Parent = owner;
            triggerOnly = trigger;
            Width = w;
            Height = h;
            OffsetX = offsetX;
            OffsetY = offsetY;
            CalculatePosition();
            boxTexture = TextureMgr.Instance.GetTexture("debug_box");

            canIgnoreTraps = ignoreTraps;
            isAggroArea = false;
            this.ignoreAggroArea = ignoreAggroArea;
        }

        public void RegisterSelf()
        {
            CollisionSystem.AddColliderToSystem(this);
        }

        public void UnRegisterSelf()
        {
            CollisionSystem.RemoveColliderFromSystem(this);
        }

        public void CleanUp()
        {
            Parent = null;
            boxTexture = null;
        }

        public void MarkAsAggroArea()
        {
            isAggroArea = true;
        }

        public void Update(GameTime gameTime) { }

        public void Move(Vector2 moveForce)
        {
            if (moveForce.X == 0 && moveForce.Y == 0)
                return;

            RectCollisionSides collisionSides = new RectCollisionSides();

            foreach (var collider in CollisionSystem.Colliders)
            {
                // ignore self and other colliders of parent object
                if (collider == this || Parent == collider.Parent)
                    continue;

                bool isColliding = false;

                if (collider is BoxCollider)
                    isColliding = IsCollidingWith(collider as BoxCollider, ref collisionSides, moveForce);
                else
                    isColliding = IsCollidingWith(collider as CircleCollider);

                if (isColliding)
                {
                    Console.WriteLine("Kolizja: ");
                    Console.WriteLine(collisionSides);

                    collider.Parent.NotifyCollision(Parent, collider);
                    Parent.NotifyCollision(collider.Parent, this);
                }
            }

            AdjustMoveForceAndMoveParent(moveForce, collisionSides);
            CalculatePosition();
        }

        public bool IsCollidingWith(CircleCollider collider)
        {
            if (CheckConditions(collider))
                return false;

            // look for closest point in rectangle of collider
            float posX = MathHelper.Clamp(collider.Position.X, Rect.Left, Rect.Right);
            float posY = MathHelper.Clamp(collider.Position.Y, Rect.Top, Rect.Bottom);
            Vector2 posOnMe = new Vector2(posX, posY);

            float dist = Vector2.Distance(collider.Position, posOnMe);

            if (dist <= collider.Radius)
                return true;

            return false;
        }

        public bool IsCollidingWith(BoxCollider collider, ref RectCollisionSides collisionSides, Vector2 moveForce)
        {
            if (CheckConditions(collider))
                return false;

            bool isColliding = false;

            if (TouchingBottom(collider, moveForce))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | RectCollisionSides.SIDE_BOTTOM;
            }

            if (TouchingLeftSide(collider, moveForce))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | RectCollisionSides.SIDE_LEFT;
            }

            if (TouchingRightSide(collider, moveForce))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | RectCollisionSides.SIDE_RIGHT;
            }

            if (TouchingTop(collider, moveForce))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | RectCollisionSides.SIDE_UP;
            }

            return isColliding;
        }

        private bool CheckConditions(ICollider collider)
        {
            if (collider.isAggroArea && ignoreAggroArea)
                return true;

            if (canIgnoreTraps && collider.Parent is Traps.Spike)
                return true;

            return false;
        }

        private bool TouchingLeftSide(BoxCollider collider, Vector2 moveForce)
        {
            return Rect.Right + moveForce.X > collider.Rect.Left &&
              Rect.Left < collider.Rect.Left &&
              Rect.Bottom > collider.Rect.Top &&
              Rect.Top < collider.Rect.Bottom;
        }

        private bool TouchingRightSide(BoxCollider collider, Vector2 moveForce)
        {
            return Rect.Left + moveForce.X < collider.Rect.Right &&
              Rect.Right > collider.Rect.Right &&
              Rect.Bottom > collider.Rect.Top &&
              Rect.Top < collider.Rect.Bottom;
        }

        private bool TouchingTop(BoxCollider collider, Vector2 moveForce)
        {
            /* return collider.Rect.Bottom > Rect.Top &&
                collider.Rect.Top < Rect.Top &&
                collider.Rect.Right > Rect.Left &&
                collider.Rect.Left < Rect.Right; */

            return Rect.Bottom + moveForce.Y > collider.Rect.Top &&
              Rect.Top < collider.Rect.Top &&
              Rect.Right > collider.Rect.Left &&
              Rect.Left < collider.Rect.Right;
        }

        private bool TouchingBottom(BoxCollider collider, Vector2 moveForce)
        {
            /*return collider.Rect.Top < Rect.Bottom &&
                collider.Rect.Bottom > Rect.Bottom &&
                collider.Rect.Right > Rect.Left &&
                collider.Rect.Left < Rect.Right; */

            return Rect.Top + moveForce.Y < collider.Rect.Bottom &&
              Rect.Bottom > collider.Rect.Bottom &&
              Rect.Right > collider.Rect.Left &&
              Rect.Left < collider.Rect.Right;
        }

        private void AdjustMoveForceAndMoveParent(Vector2 force, RectCollisionSides collisionSides)
        {
            if (force.X < 0 && ((collisionSides & RectCollisionSides.SIDE_RIGHT) != 0))
                force.X = 0;
            else if (force.X > 0 && ((collisionSides & RectCollisionSides.SIDE_LEFT) != 0))
                force.X = 0;

            if (force.Y < 0 && ((collisionSides & RectCollisionSides.SIDE_BOTTOM) != 0))
                force.Y = 0;
            else if (force.Y > 0 && ((collisionSides & RectCollisionSides.SIDE_UP) != 0))
                force.Y = 0;

            Parent.position += force;
        }

        public void Draw(SpriteBatch batch)
        {
            #if DEBUG
            batch.Draw(boxTexture, destinationRectangle: Rect, color: Color.White, layerDepth: 1);
            #endif
        }

        // Calculates center point of parent rectangle and build collision area
        private void CalculatePosition()
        {
            float posX = Parent.Rectangle.X + (Parent.Rectangle.Width / 2) + OffsetX;
            float posY = Parent.Rectangle.Y + (Parent.Rectangle.Height / 2) + OffsetY;

            Position = new Vector2(posX, posY);

            float x = posX - (Width / 2);
            float y = posY - (Height / 2);

            Rect = new Rectangle((int)x, (int)y, (int)Width, (int)Height);
        }
    }
}
