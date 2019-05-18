using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Common
{
    public class Unit : GameObject
    {
        public Unit(Texture2D texture, Vector2 position, Vector2 size, int layer = 1, Vector2? scale = null) : base(texture, position, size, layer, scale)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void NotifyCollision(GameObject gameobject, ICollider source)
        {
            base.NotifyCollision(gameobject, source);
        }
    }
}
