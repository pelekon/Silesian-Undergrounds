using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Components;

namespace Silesian_Undergrounds.Engine.Collisions
{
    public class CircleCollider : ICollider
    {
        // Component inherited
        public Vector2 Position { get; set; }
        public Rectangle Rect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
        }

        public void CleanUp()
        {
            Parent = null;
        }

        public void Draw(SpriteBatch batch)
        {
            #if DEBUG
            // Add code to draw debug texture
            #endif
        }
        public void Update(GameTime gameTime) { }

        public void RegisterSelf()
        {
            CollisionSystem.AddColliderToSystem(this);
        }

        public void UnRegisterSelf()
        {
            CollisionSystem.RemoveColliderFromSystem(this);
        }

        public bool IsCollidingWith(BoxCollider collider)
        {
            throw new NotImplementedException();
        }

        public bool IsCollidingWith(CircleCollider collider)
        {
            throw new NotImplementedException();
        }

        public bool IsCollidingWith(BoxCollider collider, ref RectCollisionSides sides)
        {
            throw new NotImplementedException();
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
        }
    }
}
