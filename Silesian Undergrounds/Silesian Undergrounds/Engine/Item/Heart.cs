using Silesian_Undergrounds.Engine.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Silesian_Undergrounds.Engine.Collisions;
using Silesian_Undergrounds.Engine.Config;
using Silesian_Undergrounds.Engine.Utils;

namespace Silesian_Undergrounds.Engine.Item {
    public class Heart : PickableItem {

        public Heart(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene, bool isBuyable = false) : base(texture, position, size, layer, scene, isBuyable)
        {
            BoxCollider collider = new BoxCollider(this, 35, 35, 0, -4, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj, ICollider source, RectCollisionSides collisionSides)
        {
            base.NotifyCollision(obj, source, collisionSides);
            if (!(obj is Player) || isBuyable) return;
            var pl = (Player)obj;
            if (pl.MaxLiveValue <= pl.LiveValue) return;
            AudioPlayerMgr.Instance.AddSoundEffect("Music/items/item_picking");
            pl.RefilLive(pl.PlayerStatistic.PickupDouble ? ConfigMgr.HeartConfig.LiveRegenerationValue  * 2 : ConfigMgr.HeartConfig.LiveRegenerationValue);
            this.scene.DeleteObject(this);
        }
    }
}
