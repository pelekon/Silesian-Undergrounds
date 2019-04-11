using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Components;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.Collisions
{
    [Flags]
    internal enum CollisionSides
    {
        SIDE_LEFT = 1,
        SIDE_RIGHT = 2,
        SIDE_BOTTOM = 4,
        SIDE_UP = 8,
    }
    
    public class BoxCollider : IComponent
    {
        public Vector2 Position { get; set; }
        public Rectangle Rect { get; set; }
        public GameObject Parent { get; private set; }
        private Texture2D boxTexture;

        private float Width;
        private float Height;
        private float OffsetX;
        private float OffsetY;

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

        public void Update(GameTime gameTime)
        {
        }

        public void Move(Vector2 moveForce)
        {
            if (moveForce.X == 0 && moveForce.Y == 0)
                return;

            CollisionSides collisionSides = new CollisionSides();

            foreach (var collider in CollisionSystem.Colliders)
            {
                if (collider == this)
                    continue;

                if (CheckCollisionWith(collider, ref collisionSides))
                {
                    collider.Parent.NotifyCollision(Parent);
                    Parent.NotifyCollision(collider.Parent);
                }
            }

            AdjustMoveForceAndMoveParent(moveForce, collisionSides);
            CalculatePosition();
        }

        private bool CheckCollisionWith(BoxCollider collider, ref CollisionSides collisionSides)
        {
            bool isColliding = false;

            if (TouchingBottom(collider))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | CollisionSides.SIDE_BOTTOM;
            }

            if (TouchingLeftSide(collider))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | CollisionSides.SIDE_LEFT;
            }

            if (TouchingRightSide(collider))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | CollisionSides.SIDE_RIGHT;
            }

            if (TouchingTop(collider))
            {
                isColliding = true;

                if (!collider.triggerOnly)
                    collisionSides = collisionSides | CollisionSides.SIDE_UP;
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

        private void AdjustMoveForceAndMoveParent(Vector2 force, CollisionSides collisionSides)
        {
            if (force.X < 0 && ((collisionSides & CollisionSides.SIDE_RIGHT) != 0))
                force.X = 0;
            else if (force.X > 0 && ((collisionSides & CollisionSides.SIDE_LEFT) != 0))
                force.X = 0;

            if (force.Y < 0 && ((collisionSides & CollisionSides.SIDE_BOTTOM) != 0))
                force.Y = 0;
            else if (force.Y > 0 && ((collisionSides & CollisionSides.SIDE_UP) != 0))
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
