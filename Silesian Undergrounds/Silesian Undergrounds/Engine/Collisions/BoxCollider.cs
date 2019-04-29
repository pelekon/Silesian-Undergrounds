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

        private bool triggerOnly;

        public BoxCollider(GameObject owner, float w, float h, float offsetX, float offsetY, bool trigger)
        {
            Parent = owner;
            triggerOnly = trigger;
            Width = w;
            Height = h;
            OffsetX = offsetX;
            OffsetY = offsetY;
            CalculatePosition();
            boxTexture = TextureMgr.Instance.GetTexture("debug_box");
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
                    isColliding = IsCollidingWith(collider as BoxCollider, ref collisionSides);
                else
                    isColliding = IsCollidingWith(collider as CircleCollider);

                if (isColliding)
                {
                    collider.Parent.NotifyCollision(Parent, collider);
                    Parent.NotifyCollision(collider.Parent, this);
                }
            }

            AdjustMoveForceAndMoveParent(moveForce, collisionSides);
            CalculatePosition();
        }

        public bool IsCollidingWith(CircleCollider collider)
        {
            // look for closest point in rectangle of collider
            float posX = MathHelper.Clamp(collider.Position.X, Rect.Left, Rect.Right);
            float posY = MathHelper.Clamp(collider.Position.Y, Rect.Top, Rect.Bottom);
            Vector2 posOnMe = new Vector2(posX, posY);

            float dist = Vector2.Distance(collider.Position, posOnMe);

            if (dist <= collider.Radius)
                return true;

            return false;
        }

        public bool IsCollidingWith(BoxCollider collider, ref RectCollisionSides collisionSides)
        {
            bool isColliding = false;

            if (TouchingBottom(collider))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | RectCollisionSides.SIDE_BOTTOM;
            }

            if (TouchingLeftSide(collider))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | RectCollisionSides.SIDE_LEFT;
            }

            if (TouchingRightSide(collider))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | RectCollisionSides.SIDE_RIGHT;
            }

            if (TouchingTop(collider))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | RectCollisionSides.SIDE_UP;
            }

            return isColliding;
        }

        private bool TouchingLeftSide(BoxCollider collider)
        {
            return Rect.Right > collider.Rect.Left &&
              Rect.Left < collider.Rect.Left &&
              Rect.Bottom > collider.Rect.Top &&
              Rect.Top < collider.Rect.Bottom;
        }

        private bool TouchingRightSide(BoxCollider collider)
        {
            return Rect.Left < collider.Rect.Right &&
              Rect.Right > collider.Rect.Right &&
              Rect.Bottom > collider.Rect.Top &&
              Rect.Top < collider.Rect.Bottom;
        }

        private bool TouchingTop(BoxCollider collider)
        {
            return Rect.Bottom > collider.Rect.Top &&
              Rect.Top < collider.Rect.Top &&
              Rect.Right > collider.Rect.Left &&
              Rect.Left < collider.Rect.Right;
        }

        private bool TouchingBottom(BoxCollider collider)
        {
            return Rect.Top < collider.Rect.Bottom &&
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
            batch.Draw(boxTexture, destinationRectangle: Rect, color: Color.White, layerDepth: Parent.layer);
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
