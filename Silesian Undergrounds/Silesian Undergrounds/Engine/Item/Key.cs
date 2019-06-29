using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Item {
    public class Key : PickableItem {

        private int KEY_AMOUNT_TO_ADD_PLAYER = 1;

        public Key(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, bool isBuyable = false) : base(texture, position, size, layer, scene, isBuyable)
        {
            BoxCollider collider = new BoxCollider(this, 35, 45, 0, -4, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj, ICollider source, RectCollisionSides collisionSides)
        {
            base.NotifyCollision(obj, source, collisionSides);

            if (obj is Player && !isBuyable)
            {
                Player pl = (Player)obj;
                if (pl.PlayerStatistic.PickupDouble)
                {
                    pl.AddKey(2);
                }
                else
                {
                    pl.AddKey(1);
                }
                this.scene.DeleteObject(this);
            }
        }
    }
}
