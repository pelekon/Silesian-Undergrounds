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

        public bool triggerOnly { get; private set; }
        public bool canIgnoreTraps { get; private set; }
        public bool isAggroArea { get; private set; }
        public bool ignoreAggroArea { get; private set; }

        public CircleCollider(GameObject parent, float r, float offsetX, float offsetY, bool trigger, bool ignoreTraps = true, bool ignoreAggroArea = true)
        {
            Parent = parent;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Radius = r;
            CalculatePosition();
            circleTexture = TextureMgr.Instance.GetTexture("debug_circle");

            triggerOnly = trigger;
            canIgnoreTraps = ignoreTraps;
            isAggroArea = false;
            this.ignoreAggroArea = ignoreAggroArea;
        }

        public void CleanUp()
        {
            Parent = null;
        }

        public void MarkAsAggroArea()
        {
            isAggroArea = true;
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
            if (CheckConditions(collider))
                return false;

            float totalRadius = Radius + collider.Radius;
            float totalDiff = Vector2.Distance(Position, collider.Position);

            if (totalDiff <= totalRadius)
                return true;

            return false;
        }

        public bool IsCollidingWith(BoxCollider collider, ref RectCollisionSides sides, Vector2 moveForce)
        {
            if (CheckConditions(collider))
                return false;

            // look for closest point in rectangle of collider
            float posX = MathHelper.Clamp(Position.X, collider.Rect.Left, collider.Rect.Right);
            float posY = MathHelper.Clamp(Position.Y, collider.Rect.Top, collider.Rect.Bottom);
            Vector2 pointOnRect = new Vector2(posX, posY);

            float dist = Vector2.Distance(Position, pointOnRect);

            if (dist <= Radius)
                return true;

            return false;
        }

        private bool CheckConditions(ICollider collider)
        {
            if (collider.isAggroArea && ignoreAggroArea)
                return true;

            if (canIgnoreTraps && collider.Parent is Traps.Spike)
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
    }
}
