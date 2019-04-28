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
    public class CircleCollider : ICollider
    {
        // Component inherited
        public Vector2 Position { get; set; }
        public Rectangle Rect { get ; set; }
        public GameObject Parent { get; private set; }

        // Circle
        public float OffsetX { get; private set; }
        public float OffsetY { get; private set; }
        public float Radius { get; private set; }
        private Texture2D circleTexture;

        public CircleCollider(GameObject parent, float r, float offsetX, float offsetY)
        {
            Parent = parent;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Radius = r;
            CalculatePosition();
            circleTexture = TextureMgr.Instance.GetTexture("debug_circle");
        }

        public void CleanUp()
        {
            Parent = null;
        }

        public void Draw(SpriteBatch batch)
        {
            #if DEBUG
            batch.Draw(circleTexture, Rect, Color.White);
            #endif
        }
        public void Update(GameTime gameTime)
        {
            CalculatePosition();
        }

        public void RegisterSelf()
        {
            CollisionSystem.AddColliderToSystem(this);
        }

        public void UnRegisterSelf()
        {
            CollisionSystem.RemoveColliderFromSystem(this);
        }

        public bool IsCollidingWith(CircleCollider collider)
        {
            float totalRadius = Radius + collider.Radius;
            float totalDiff = GetDistanceBetweenPoints(Position.X, Position.Y, collider.Position.X, collider.Position.Y);

            if (totalDiff <= totalRadius)
                return true;

            return false;
        }

        public bool IsCollidingWith(BoxCollider collider, ref RectCollisionSides sides)
        {
            // look for closest point in rectangle of collider
            float posX = MathHelper.Clamp(Position.X, collider.Rect.Left, collider.Rect.Right);
            float posY = MathHelper.Clamp(Position.Y, collider.Rect.Top, collider.Rect.Bottom);

            float dist = GetDistanceBetweenPoints(Position.X, Position.Y, posX, posY);

            if (dist <= Radius)
                return true;

            return false;
        }

        public void Move(Vector2 moveForce)
        {
            throw new NotImplementedException();
        }

        private void CalculatePosition()
        {
            float posX = Parent.Rectangle.X + (Parent.Rectangle.Width / 2) + OffsetX;
            float posY = Parent.Rectangle.Y + (Parent.Rectangle.Height / 2) + OffsetY;

            Position = new Vector2(posX, posY);

            float x = posX - Radius;
            float y = posY - Radius;
            float size = Radius * 2;
            Rect = new Rectangle((int)x, (int)y, (int)size, (int)size);
        }

        public static float GetDistanceBetweenPoints(float aX, float aY, float bX, float bY)
        {
            double diffX = Math.Pow(Math.Abs(aX - bX), 2);
            double diffY = Math.Pow(Math.Abs(aY - bY), 2);
            float totalDiff = (float)Math.Sqrt(diffX + diffY);

            return totalDiff;
        }
    }
}
