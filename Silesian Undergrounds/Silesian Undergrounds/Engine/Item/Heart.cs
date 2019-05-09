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
        public const int HEART_INCREASE_VALUE = 10;

        public Heart(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, bool isBuyable = false) : base(texture, position, size, layer, scene, isBuyable)
        {
            BoxCollider collider = new BoxCollider(this, 35, 35, 0, -4, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj, ICollider source)
        {
            base.NotifyCollision(obj, source);

            if (obj is Player && !isBuyable)
            {
                Player pl = (Player)obj;
                if (pl.MaxLiveValue > pl.LiveValue)
                {
                    if (pl.PlayerStatistic.PickupDouble)
                    {
                        pl.RefilLive(this.liveRegenerationValue * 2);
                    }
                    else
                    {
                        pl.RefilLive(this.liveRegenerationValue);
                    }
                    this.scene.DeleteObject(this);
                }

            }
        }
    }
}
