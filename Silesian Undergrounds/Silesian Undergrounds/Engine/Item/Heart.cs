using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Item {
    public class Heart : PickableItem{

        private int liveRegenerationValue = 25;

        public Heart(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene)
        {
            BoxCollider collider = new BoxCollider(this, 35, 35, 0, -4, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj, ICollider source)
        {
            if (obj is Player)
            {
                Player pl = (Player)obj;
                if (pl.MaxLiveValue > pl.LiveValue)
                {
                    pl.RefilLive(this.liveRegenerationValue);
                    this.scene.DeleteObject(this);
                }

            }
        }
    }
}
