using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Components;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Behaviours
{
    public class HostileBehaviour : IComponent
    {
        // Component inherited
        public Vector2 Position { get; set; }
        public Rectangle Rect { get; set; }

        public GameObject Parent { get; private set; }

        // HostileBehaviour specyfic
        private bool IsInCombat;
        private GameObject enemy;

        public HostileBehaviour(GameObject parent)
        {
            Parent = parent;
            Position = new Vector2(0, 0);
            Rect = new Rectangle(0, 0, 0, 0);

            IsInCombat = false;
            enemy = null;

            Parent.AddComponent(new CircleCollider(Parent, 70, 0, 0));
            Parent.OnCollision += NotifyCollision;
        }

        public void CleanUp()
        {
            Parent = null;
        }

        public void Draw(SpriteBatch batch) { }

        public void RegisterSelf() { }

        public void UnRegisterSelf() { }

        public void Update(GameTime gameTime)
        {
            
        }

        public void NotifyCollision(object sender, CollisionNotifyData data)
        {
            if (!IsInCombat && data.source is CircleCollider)
            {
                IsInCombat = true;
                enemy = data.obj;
            }
        }
    }
}
