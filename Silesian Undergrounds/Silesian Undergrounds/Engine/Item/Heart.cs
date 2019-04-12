using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Item {
    class Heart : PickableItem{

        private int healtRegenerationValue = 0;

        public Heart(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene)
        {
            BoxCollider collider = new BoxCollider(this, 35, 35, 0, -4, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj)
        {
            if (obj is Player)
            {
                this.scene.DeleteObject(this);
            }
        }
    }
}
