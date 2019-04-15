using System;
using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Collisions;
using System.Diagnostics;
using Silesian_Undergrounds.Engine.CommonF;

namespace Silesian_Undergrounds.Engine.Item {
    public class Heart : PickableItem {

        private int liveRegenerationValue = 25;

        public Heart(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, bool isBuyable = false) : base(texture, position, size, layer, scene, isBuyable)
        {
            BoxCollider collider = new BoxCollider(this, 35, 35, 0, -4, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj)
        {
            base.NotifyCollision(obj);

            if (obj is Player && !isBuyable)
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
