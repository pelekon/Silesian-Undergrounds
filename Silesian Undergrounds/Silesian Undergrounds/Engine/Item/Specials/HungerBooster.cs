using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silesian_Undergrounds.Engine.Common;
using Silesian_Undergrounds.Engine.Config;
using Silesian_Undergrounds.Engine.Collisions;

namespace Silesian_Undergrounds.Engine.Item.Specials
{
    class HungerBooster : SpecialItem {

        public HungerBooster(Texture2D texture, Vector2 position, Vector2 size, int layer, Scene.Scene scene) : base(texture, position, size, layer, scene)
        {
            BoxCollider collider = new BoxCollider(this, 20, 20, 0, 0, true);
            AddComponent(collider);
        }

        public override void NotifyCollision(GameObject obj, ICollider source, RectCollisionSides collisionSides)
        {
            base.NotifyCollision(obj, source, collisionSides);

            if (obj is Player)
            {
                Player pl = (Player)obj;
                pl.IncreaseHungerMaxValueBy(ConfigMgr.HungerBoosterConfig.PlayerMaxHungerValueIncreaseBy);
                this.scene.DeleteObject(this);
            }
        }
    }
}
